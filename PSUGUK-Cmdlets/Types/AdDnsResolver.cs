﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Heijden.DNS;
using System.Management;

namespace Dusty.ADConnectivity
{
    /* 
     * We want to represent a DNS server that resolves a given AD domain
     * 
     * The main method is Query, which can do any DNS query you want
     * 
     * In the context of AD, we usually want to further resolve SRV
     * records to IP addresses, so there is a QuerySRV method
     * 
     * The SRV record for the PDC and the A record(s) for the root of the 
     * domain are both particularly important, so we include
     * PDC and DomainARcords properties and QueryPdc and QueryDomainARecords
     * methods
     * 
     */
    public class AdDnsResolver : DnsResolver
    {
        public AdDnsResolver(IPEndPoint DnsServer, string AdDomain, string AdSite = null) : base(DnsServer)
        {
            this.AdDomain = AdDomain;
            this.AdSite = AdSite;
        }

        public AdDnsResolver(IPAddress DnsServer, string AdDomain) : base(DnsServer)
        {
            this.AdDomain = AdDomain;
            this.AdSite = AdSite;
        }

        //hide the parent's constructor, because it makes no sense to have this class without the AdDomain property
        private AdDnsResolver(IPEndPoint DnsServer) : base(DnsServer)
        {
        }

        public string AdDomain { get; private set; }

        public string AdSite { get; set; }

        public DnsResponse Pdc { get; private set; }

        public DnsResponse DomainARecords { get; private set; }

        public DnsResponse SiteLdap { get; private set; }

        public DnsResponse QueryPdc()
        {
            string locator = $"_ldap._tcp.pdc._msdcs.{AdDomain}";
            Pdc = QuerySrv(locator);

            if (Pdc.Answers.Count() > 1)
            {
                var errors = Pdc.Errors.ToList();
                errors.Insert(0, "PDC locator resolves to multiple IPs");
                Pdc = new DnsResponse(Pdc.Answers, errors);
            }

            return Pdc;
        }

        public DnsResponse QuerySrv(string locator)
        {
            var response = Query(locator, QType.SRV);
            var errors = response.Errors.ToList();

            List<string> ipAnswers = new List<string>();
            foreach (var name in response.Answers)
            {
                var secondResponse = Query(name);
                errors.AddRange(secondResponse.Errors);
                if (secondResponse.Answers.Count > 1) { errors.Insert(0, "SRV target resolves to multiple IPs"); }
                if (secondResponse.Answers.Count < 1) { errors.Insert(0, "SRV target cannot be resolved"); }
                ipAnswers.AddRange(secondResponse.Answers);
            }

            return new DnsResponse(
                ipAnswers.ToArray<string>(),
                errors.ToArray()
                );
        }

        public DnsResponse QueryDomainARecords()
        {
            DomainARecords = Query(AdDomain, QType.A);
            return DomainARecords;
        }

        public DnsResponse QuerySiteLdap()
        {
            if (AdSite == null) { return null; }
            SiteLdap = QuerySrv($"_ldap.{AdSite}.{AdDomain}");
            return SiteLdap;
        }

        public AdDnsResponse QueryAd()
        {
            QueryPdc();
            QueryDomainARecords();
            QuerySiteLdap();

            return new AdDnsResponse(
                dnsServer: this.DnsServer,
                adDomain: this.AdDomain,
                adSite: this.AdSite,
                pdc: this.Pdc,
                domainARecords: this.DomainARecords,
                siteLdap: this.SiteLdap
                );
        }


    }
}
