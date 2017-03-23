
$Host.UI.RawUI.WindowTitle = 'Debugging'
Import-Module .\AdConnectivity

$CorpDomain = 'corp.dustyfox.uk'
$CorpIp = '134.213.29.116'


#==============================================#


Write-Host -ForegroundColor Magenta '
    Get-Help Get-DnsResolver -Full
'
Get-Help Get-DnsResolver -Full | Out-String



Write-Host -ForegroundColor Magenta '
	Get-DnsResolver $CorpDomain -Verbose
'
	Get-DnsResolver $CorpDomain -Verbose

Write-Host -ForegroundColor Magenta '
	Get-DnsResolver $CorpDomain -UseMachineDomain -Verbose
'
	Get-DnsResolver $CorpDomain -UseMachineDomain -Verbose | Out-String

