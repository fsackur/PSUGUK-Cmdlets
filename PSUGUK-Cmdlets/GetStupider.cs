using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dusty.AdConnectivity
{
    /*
     * Won't compile - needs a reference to the System.Management.Automation assembly
     * 
     * Install-Package System.Management.Automation_PowerShell_3.0
     * 
     */ 
    [Cmdlet("Get", "Stupider")]
    class GetStupider : PSCmdlet
    {

    }
}
