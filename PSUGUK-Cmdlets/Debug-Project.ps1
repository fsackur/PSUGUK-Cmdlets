
$Host.UI.RawUI.WindowTitle = 'Debugging'
Import-Module .\Dusty.AdConnectivity.dll

 $CorpDomain = 'corp.dustyfox.uk'
 $CorpIp = '134.213.29.116'
 

 #==============================================#
 
 Write-Host '    $R = New-Object Dusty.ADConnectivity.DnsResolver $CorpIp
 '
 $R = New-Object Dusty.ADConnectivity.DnsResolver $CorpIp

 Write-Host '    $R.Query($CorpDomain)
 '
 $R.Query($CorpDomain)