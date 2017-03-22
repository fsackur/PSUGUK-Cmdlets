using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace Dusty.AdConnectivity
{
    /*
     * The primary reference "System.Management.Automation, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL" 
     * could not be resolved because it has an indirect dependency on the assembly 
     * "Microsoft.Management.Infrastructure.Native, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" 
     * which was built against the ".NETFramework,Version=v4.5" framework. This is a higher version than the currently 
     * targeted framework ".NETFramework,Version=v4.0".	PSUGUK-Cmdlets			
     *
     * The file in the GAC was built against .NET 4.5. That is specific to my Win10 development machine
     * 
     * Crappy hack - copied the file from a 2012 box into the packages folder and added a reference.
     * 
     */
    [Cmdlet("Get", "Stupider")]
    class GetStupider : PSCmdlet
    {

    }
}
