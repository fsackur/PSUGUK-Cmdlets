<#
    .Synopsis
    TeamVillage

    .Description
    Run as a post-build event
#>
param(
	#the full path to the project .dll file
    [string]$TargetPath
)


$ProjectDir = $PSScriptRoot
$TargetDir = Split-Path $TargetPath
Set-Location $TargetDir
$PsdPath = $TargetPath -replace 'Dusty\.AdConnectivity\.dll', 'AdConnectivity.psd1'


#Move out of the "PowerShell" folder; that is just for organisation of source code
Get-ChildItem '.\PowerShell\*.*' | %{Move-Item $_ . -Force}
Remove-Item '.\PowerShell' -Force


#Increment version number in .psd1 file to match .dll
$Version = (Get-Item $TargetPath -ErrorAction Stop).VersionInfo.ProductVersion;
Update-ModuleManifest -Path $PsdPath -ModuleVersion $Version



#Move into public / private folders
Remove-Item .\System.Management.Automation.dll -Force -ErrorAction SilentlyContinue  #We don't need this in output, our cmdlets are running in PowerShell
Remove-Item .\Microsoft.Management.Infrastructure.Native.dll -Force -ErrorAction SilentlyContinue  
[void](New-Item -ItemType Directory -Path Private -Force)
[void](New-Item -ItemType Directory -Path Public -Force)
Get-ChildItem '.\*.dll' | %{Move-Item $_ .\Private -Force}
Get-ChildItem '.\*.pdb' | %{Move-Item $_ .\Private -Force}
Get-ChildItem '.\*.xml' | %{Move-Item $_ .\Private -Force}
Get-ChildItem '.\*.ps1xml' | %{Move-Item $_ .\Private -Force}
Get-ChildItem '.\*.psm1' | %{Move-Item $_ .\Public -Force}
#Get-ChildItem '.\*.ps1' | %{Move-Item $_ .\Public -Force}


#region Update help file
Import-Module PlatyPS          #Install-Module PlatyPS (run in PowerShell x86 because, hey, Visual Studio)
Import-Module $PsdPath
$Cmdlets = Get-Command -Module 'AdConnectivity' -CommandType Cmdlet

#If the help markdown isn't there, create it
New-MarkdownHelp -Command $Cmdlets -OutputFolder $ProjectDir\docs -ErrorAction SilentlyContinue

#if the help markdown is there, update it with any changes to the parameter bindings
Update-MarkdownHelp -Path $ProjectDir\docs
[void](New-Item -ItemType Directory -Path en-US -Force)
Remove-Item .\en-US\*Help.xml -Force
New-ExternalHelp -Path $ProjectDir\docs -OutputPath .\en-US -Force

#endregion Update help file
