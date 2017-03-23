//https://www.codeproject.com/Articles/23673/DNS-NET-Resolver-C
//Install-Package Heijden.DNS
using Heijden.DNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;
using Dusty.ADConnectivity;
using System.Management;

namespace Dusty.ADConnectivity
{

    [Cmdlet(VerbsCommon.Get, "DnsResolver")]
    [OutputType(typeof(DnsResolver))]
    public class GetDnsResolver : PSCmdlet
    {
        //for ease of use, we want to accept hostnames or ipaddresses
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateDnsHostnameOrIpAddress()]
        public string[] DnsServer { get; set; }


        protected override void ProcessRecord()
        {
            
            foreach (string name in DnsServer)
            {
                if (Uri.CheckHostName(name.ToString()) == UriHostNameType.IPv4 ||
                        Uri.CheckHostName(name.ToString()) == UriHostNameType.IPv6)
                {
                    WriteVerbose($"{name} is an IP address");

                    IPAddress ip = IPAddress.Parse(name);
                    WriteObject(new DnsResolver(new IPEndPoint(ip, 53)));
                    continue;
                }

                try
                {
                    WriteVerbose($"{name} is not an IP address; attempting to resolve using the machine default DNS resolvers");
                    IPHostEntry host = Dns.GetHostEntry(name);
                    foreach (IPAddress ip in host.AddressList)
                    {
                        WriteObject(new DnsResolver(new IPEndPoint(ip, 53)));
                    }
                }
                catch
                {
                    WriteWarning(string.Format("Unable to resolve hostname {0}", name));
                }

            } //end foreach name in DnsServer
        }  //end ProcessRecord

    }
}
