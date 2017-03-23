
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
	$DustyDc = New-Object Dusty.ADConnectivity.AdDnsResolver ($CorpIp, $CorpDomain)
	$DustyDc
'
	$DustyDc = New-Object Dusty.ADConnectivity.AdDnsResolver ($CorpIp, $CorpDomain)
	$DustyDc | Out-String

Write-Host -ForegroundColor Magenta '
	$DustyDc.QueryAd()
'
	$DustyDc.QueryAd() | Out-String

Write-Host -ForegroundColor Magenta '
	$DustyDc.QueryAd().GetErrors()
'
	$DustyDc.QueryAd().GetErrors() | Out-String

pause

Write-Host -ForegroundColor Magenta '
	$Goog = New-Object Dusty.ADConnectivity.AdDnsResolver ($CorpIp, $CorpDomain)
	$Goog
'
	$Goog = New-Object Dusty.ADConnectivity.AdDnsResolver ("8.8.8.8", $CorpDomain)
	$Goog | Out-String

Write-Host -ForegroundColor Magenta '
	$Goog.QueryAd()
'
	$Goog.QueryAd() | Out-String

Write-Host -ForegroundColor Magenta '
	$DustyDc.QueryAd().GetErrors()
'
	$Goog.QueryAd().GetErrors() | Out-String
