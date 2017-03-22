
$Host.UI.RawUI.WindowTitle = 'Debugging'
Import-Module .\Dusty.AdConnectivity.dll

$CorpDomain = 'corp.dustyfox.uk'
$CorpIp = '134.213.29.116'


#==============================================#


Write-Host -ForegroundColor Magenta '
$DustyDC = Get-DnsResolver $CorpDomain -Verbose
$DustyDC
'
$DustyDC = Get-DnsResolver $CorpDomain -Verbose
$DustyDC | Out-String

Write-Host -ForegroundColor Magenta '
$Goog = "8.8.8.8" | Get-DnsResolver -Verbose
$Goog
'
$Goog = "8.8.8.8" | Get-DnsResolver -Verbose
$Goog | Out-String

Write-Host -ForegroundColor Magenta '
"8.8.8.8", $CorpDomain | Get-DnsResolver -Verbose
'
"8.8.8.8", $CorpDomain | Get-DnsResolver  -Verbose | Out-String

Write-Host -ForegroundColor Magenta '
Get-DnsResolver "8.8.8.8", $CorpDomain -Verbose
'
Get-DnsResolver "8.8.8.8", $CorpDomain  -Verbose | Out-String
