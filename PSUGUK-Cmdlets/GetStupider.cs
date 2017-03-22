using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerShell.Commands;

namespace Dusty.AdConnectivity
{
    [Cmdlet(VerbsCommon.Get, "Stupider")]
    public class GetStupider : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)] //, ValueFromPipeline = true)]
        public string[] StupidName { get; set; }

        protected override void BeginProcessing()
        {
            //Parameters that accept pipeline input are not initialised in the begin block
            WriteObject(StupidName);
        }

        protected override void ProcessRecord()
        {
            WriteObject(string.Format(
                        "{0} {1} stupid.",
                        string.Join(" and ", StupidName),
                        StupidName.Length > 1 ? "is" : "are"
                    )
                );
        }
        
    }
}
