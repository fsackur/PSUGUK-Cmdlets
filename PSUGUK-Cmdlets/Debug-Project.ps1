
$Host.UI.RawUI.WindowTitle = 'Debugging'
Import-Module .\Dusty.AdConnectivity.dll

$CorpDomain = 'corp.dustyfox.uk'
$CorpIp = '134.213.29.116'


#==============================================#

<#
Write-Host -ForegroundColor Magenta '
    Get-Help Get-DnsResolver -Full
'
Get-Help Get-DnsResolver -Full | Out-String
#>


Write-Host -ForegroundColor Magenta '
    Get-DnsResolver "myyyyCOPM>UTRRZZZZplayingUPPPP"
'
Get-DnsResolver "myyyyCOPM>UTRRZZZZplayingUPPPP" | Out-String
