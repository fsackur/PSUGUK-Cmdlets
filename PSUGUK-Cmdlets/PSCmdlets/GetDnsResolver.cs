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

    [Cmdlet(VerbsCommon.Get, "DnsResolver", DefaultParameterSetName = "DnsOnly")]
    [OutputType(typeof(DnsResolver), ParameterSetName = (new string[1] { "DnsOnly" }))]
    [OutputType(typeof(AdDnsResolver), ParameterSetName = (new string[2] { "AdSpecifiedDomain", "AdMachineDomain" }))]
    public class GetDnsResolver : PSCmdlet
    {
        //for ease of use, we want to accept hostnames or ipaddresses
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateDnsHostnameOrIpAddress()]
        public string[] DnsServer { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "AdSpecifiedDomain")]
        [ValidateDnsHostname()]
        public string AdDomain { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "AdMachineDomain")]
        public SwitchParameter UseMachineDomain { get; set; }


        protected override void BeginProcessing()
        {
            if (MyInvocation.BoundParameters.ContainsKey(nameof(UseMachineDomain)))
            {
                WriteVerbose("AdDOmain not specified; attempting to fetch machine's domain from WMI");

                string ComputerName = Environment.GetEnvironmentVariable("COMPUTERNAME");
                ManagementObject cs = new ManagementObject(
                    $"Root\\CIMv2:Win32_ComputerSystem.Name='{ComputerName}'"
                    );
                AdDomain = cs.GetPropertyValue("Domain").ToString();
            }
        }


        protected override void ProcessRecord()
        {
            
            foreach (string name in DnsServer)
            {
                if (Uri.CheckHostName(name.ToString()) == UriHostNameType.IPv4 ||
                        Uri.CheckHostName(name.ToString()) == UriHostNameType.IPv6)
                {
                    WriteVerbose($"{name} is an IP address");

                    IPAddress ip = IPAddress.Parse(name);

                    if (ParameterSetName == "DnsOnly")
                    {
                        WriteObject(new DnsResolver(new IPEndPoint(ip, 53)));
                    }
                    else
                    {
                        WriteObject(new AdDnsResolver(new IPEndPoint(ip, 53), AdDomain));
                    }
                    continue;
                }

                try
                {
                    WriteVerbose($"{name} is not an IP address; attempting to resolve using the machine default DNS resolvers");

                    IPHostEntry host = Dns.GetHostEntry(name);
                    foreach (IPAddress ip in host.AddressList)
                    {
                        if (ParameterSetName == "DnsOnly")
                        {
                            WriteObject(new DnsResolver(new IPEndPoint(ip, 53)));
                        }
                        else
                        {
                            WriteObject(new AdDnsResolver(new IPEndPoint(ip, 53), AdDomain));
                        }
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
