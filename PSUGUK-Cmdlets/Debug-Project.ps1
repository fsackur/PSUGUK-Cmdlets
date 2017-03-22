
$Host.UI.RawUI.WindowTitle = 'Debugging'
Import-Module .\Dusty.AdConnectivity.dll



Write-Host -ForegroundColor Yellow 'By parameter:'
Get-Stupider 'Jim', 'Bob'

Write-Host ""
Write-Host -ForegroundColor Yellow 'From pipeline: '
'Zinnie', 'Lou-Lou' | Get-Stupider