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
$ModuleName = (Split-Path $TargetPath -Leaf) -replace '\.dll$'


#region Update help file
Import-Module PlatyPS          #Install-Module PlatyPS (run in PowerShell x86 because, hey, Visual Studio)
Import-Module $TargetPath
$Cmdlets = Get-Command -Module $ModuleName -CommandType Cmdlet

#If the help markdown isn't there, create it
New-MarkdownHelp -Command $Cmdlets -OutputFolder $ProjectDir\docs -ErrorAction SilentlyContinue

#if the help markdown is there, update it with any changes to the parameter bindings
Update-MarkdownHelp -Path $ProjectDir\docs
[void](New-Item -ItemType Directory -Path en-US -Force)
Remove-Item .\en-US\*Help.xml -Force
New-ExternalHelp -Path $ProjectDir\docs -OutputPath .\en-US -Force #-ErrorAction Stop

#endregion Update help file