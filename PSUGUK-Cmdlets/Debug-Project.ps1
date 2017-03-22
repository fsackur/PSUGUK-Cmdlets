
$Host.UI.RawUI.WindowTitle = 'Debugging'
Import-Module .\Dusty.AdConnectivity.dll

$CorpDomain = 'corp.dustyfox.uk'
$CorpIp = '134.213.29.116'


#==============================================#


Write-Host -ForegroundColor Magenta '
$DustyDC = Get-DnsResolver $CorpDomain
$DustyDC
'
$DustyDC = Get-DnsResolver $CorpDomain
$DustyDC | Out-String

Write-Host -ForegroundColor Magenta '
$Goog = "8.8.8.8" | Get-DnsResolver
$Goog
'
$Goog = "8.8.8.8" | Get-DnsResolver
$Goog | Out-String

Write-Host -ForegroundColor Magenta '
"8.8.8.8", $CorpDomain | Get-DnsResolver
'
"8.8.8.8", $CorpDomain | Get-DnsResolver | Out-String

Write-Host -ForegroundColor Magenta '
Get-DnsResolver "8.8.8.8", $CorpDomain
'
Get-DnsResolver "8.8.8.8", $CorpDomain | Out-String
