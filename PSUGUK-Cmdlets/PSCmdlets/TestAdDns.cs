﻿using Heijden.DNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;
using Dusty.ADConnectivity;


namespace Dusty.ADConnectivity
{
    [Cmdlet(VerbsDiagnostic.Test, "AdDns")]
    [OutputType(typeof(bool))]
    public class TestAdDns : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public AdDnsResolver[] DnsServer { get; set; }

        [Parameter(Mandatory = false, Position = 1)]
        public string AdSite { get; set; }
        
        private List<AdDnsResolver> resolvers;
        private List<AdDnsResponse> responses;

        private void ProcessResolver(AdDnsResolver resolver)
        {
            var response = resolver.QueryAd();
            var errors = response.GetErrors();
            responses.Add(response);
            if (errors != null && errors.Count > 0)
            {
                errors.ForEach( e => WriteVerbose($"{response.DnsServer}: {e}") );
            }
        }
        
        protected override void BeginProcessing()
        {
            resolvers = new List<AdDnsResolver>();
            responses = new List<AdDnsResponse>();
        }

        protected override void ProcessRecord()
        {
            resolvers.AddRange(DnsServer);
        }

        protected override void EndProcessing()
        {
            if (MyInvocation.BoundParameters.ContainsKey(nameof(AdSite)))
            {
                resolvers.ForEach(r => r.AdSite = AdSite);
            }

            resolvers.ForEach(ProcessResolver);

            List<AdDnsResponse> distinctResponses = responses
                .Distinct()
                .ToList();

            KeyValuePair<AdDnsResponse, List<string>> majority;
            

            if (distinctResponses.Count() == 1)
            {
                majority = new KeyValuePair<AdDnsResponse, List<string>>(
                    distinctResponses.First(),
                    new List<string>(1) { distinctResponses.First().DnsServer }
                    );
            }
            else
            {
                //divide up the responses based on which agree with each other. Each element has a list of servers that returned the same records
                var equalitySets = new Dictionary<AdDnsResponse, List<string>>(distinctResponses.Count());

                foreach (var refobj in distinctResponses)
                {
                    equalitySets.Add(
                        refobj,
                        responses
                            .Where(r => r.Equals(refobj))
                            .Select(r => r.DnsServer)
                            .ToList()
                        );
                }

                var sb = new StringBuilder();

                majority = equalitySets
                    .OrderByDescending(kvp => kvp.Value.Count)
                    .First();
                equalitySets.Remove(majority.Key);

                sb
                    .Append("The following servers returned matching DNS records: ")
                    .Append(String.Join(", ", majority.Value.ToArray()))
                    .Append("\r\n");

                foreach (var kvp in equalitySets)
                {
                    List<string> differences;
                    majority.Key.Equals(kvp.Key, out differences);

                    sb
                        .Append("The following servers returned differing DNS records: ")
                        .Append("\r\n")
                        .Append(String.Join(", ", kvp.Value.ToArray()))
                        .Append("\r\n")

                        .Append("The differing records were: ")
                        .Append("\r\n")

                        .Append(string.Join(", ", differences.ToArray()));

                }

                WriteVerbose(sb.ToString());

            } //end if distinctresponses count


            WriteObject(majority.Key);

        } //end EndProcessing
    } //end class
}
