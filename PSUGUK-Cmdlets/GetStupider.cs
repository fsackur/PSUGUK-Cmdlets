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
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public string[] StupidName { get; set; }

        [Parameter()]
        public SwitchParameter LearnGrammarGood { get; set; }

        private bool plural;

        protected override void BeginProcessing()
        {
            plural = StupidName != null && StupidName.Length > 1;
            plural = plural ^ LearnGrammarGood.ToBool();

            WriteObject(StupidName);
        }

        protected override void ProcessRecord()
        {
            WriteObject(string.Format(
                        "{0} {1} stupid.",
                        string.Join(" and ", StupidName),
                        plural ? "is" : "are"
                    )
                );
        }
        
    }
}
