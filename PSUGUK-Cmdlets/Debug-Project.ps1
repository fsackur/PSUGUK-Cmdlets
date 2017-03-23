
$Host.UI.RawUI.WindowTitle = 'Debugging'
Import-Module .\AdConnectivity

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
	$R1 = Get-DnsResolver $CorpDomain $CorpDomain
	$R2 = Get-DnsResolver $CorpDomain $CorpDomain
	$R3 = Get-DnsResolver "8.8.8.8" $CorpDomain
	$R1, $R2, $R3 | Test-AdDns
'

	$R1 = Get-DnsResolver $CorpDomain $CorpDomain
	$R2 = Get-DnsResolver $CorpDomain $CorpDomain
	$R3 = Get-DnsResolver "8.8.8.8" $CorpDomain
	$R1, $R2, $R3 | Test-AdDns -Verbose
