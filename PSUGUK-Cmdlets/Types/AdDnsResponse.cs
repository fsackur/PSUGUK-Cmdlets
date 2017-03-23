using Heijden.DNS;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using System;

namespace Dusty.ADConnectivity
{
    /*
     * We want a return type for the methods in AdDnsResolver that encapsulates
     * the underlying DnsResponse objects.
     * 
     * We want to match the extensibility of AdDnsResolver, so the actual DNS 
     * responses are backed by a dictionary which we can access with GetResponse 
     * and GetResponses methods
     * 
     * In the context of sanity-checking the DNS servers in an AD domain, it's
     * useful to know if all DNS servers return the same records, so we implement
     * Equals
     */
    public class AdDnsResponse : IEqualityComparer<AdDnsResponse>, IEquatable<AdDnsResponse>
    {

        //constructor
        public AdDnsResponse(
            string dnsServer,
            string adDomain,
            string adSite = null,
            DnsResponse pdc = null,
            DnsResponse domainARecords = null,
            DnsResponse siteLdap = null
        )
        {
            this.AdDomain = adDomain;
            this.DnsServer = dnsServer;
            this.AdSite = adSite;
            this.Pdc = pdc;
            this.DomainARecords = domainARecords;
            this.SiteLdap = siteLdap;
        }


        //properties
        public string AdDomain { get; private set; }
        public string DnsServer { get; private set; }
        public string AdSite { get; private set; }
        public DnsResponse Pdc { get; }
        public DnsResponse DomainARecords { get; }
        public DnsResponse SiteLdap { get; }


        //methods
        public List<string> GetErrors()
        {
            List<string> errors = new List<string>();
            if (Pdc != null) { errors.AddRange(Pdc.Errors); }
            if (DomainARecords != null) { errors.AddRange(DomainARecords.Errors); }
            if (SiteLdap != null) { errors.AddRange(SiteLdap.Errors); }
            return errors;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }


        //an overload that lets us put each discrepancy in an out variable
        public bool Equals(AdDnsResponse comparison, out List<string> differences)
        {
            if (comparison == null) { throw new ArgumentNullException(); }

            differences = new List<string>();

            if (!Pdc.Equals(comparison.Pdc)) { differences.Add("Pdc"); }
            if (!DomainARecords.Equals(comparison.DomainARecords)) { differences.Add("DomainARecords"); }
            if (!SiteLdap.Equals(comparison.SiteLdap)) { differences.Add("SiteLdap"); }

            return (differences.Count == 0);
        }

        public bool Equals(AdDnsResponse comparison)
        {
            List<string> outval;
            return Equals(comparison, out outval);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        // IEqualityComparer methods
        public bool Equals(AdDnsResponse x, AdDnsResponse y)
        {
            if (x == null && y == null) { return true; }
            if (x == null && y != null) { return false; }
            return x.Equals(y);
        }

        public int GetHashCode(AdDnsResponse obj)
        {
            return GetHashCode();
        }
    }
}