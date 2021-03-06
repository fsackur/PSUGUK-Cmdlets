
<#
    This will load Posh-Git and temporarily update your prompt.

    Step forward through git history with F5, backward with Alt+Shift+F5

    Pro tip: remove unneeded extra keyboard layouts, because Alt+Shift switches them. Annoying
#>


param([switch]$Reverse)


#This isn't really intended for running at the console
if ($Host.Name -notlike "*ISE*") {
    ise .\StepThrough-CommitHistory.ps1
    Write-Host -ForegroundColor Yellow "switching to ISE - press F5 to step forward through history, Ctrl-F5 to step backward"
    return
}


#Try to import git and set it up
try {
    Import-Module Posh-Git -ErrorAction Stop

} catch {
    try {
        if ((Read-Host "Attempt to install chocolatey, git and posh-git (y/n)") -notlike "y") {return}

        Install-Package Posh-Git -ForceBootstrap -Force
        iwr https://chocolatey.org/install.ps1 -UseBasicParsing | iex
        choco install git -y

    } catch {
        Write-Host -ForegroundColor Yellow "I tried to install some stuff but failed."
        return
    }
}


#Set the git prompt
if (!$GitPromptSettings.DefaultPromptSuffix) {
    $GitPromptSettings | Add-Member NoteProperty -Name 'DefaultPromptSuffix' -Value '`n$(''>'' * ($nestedPromptLevel + 1)) '
    function global:prompt {
        $realLASTEXITCODE = $LASTEXITCODE
        Write-Host($pwd.ProviderPath) -nonewline
        Write-VcsStatus
        $global:LASTEXITCODE = $realLASTEXITCODE
        return "`n> "
    }
}



#F5 will already step forwards; add an ISE shortcut to step back
try {
    [void]$psISE.CurrentPowerShellTab.AddOnsMenu.Submenus.Add(
        "Step back through commit history",
        [scriptblock]::Create("$PSScriptRoot\StepThrough-CommitHistory.ps1 -Reverse"),
        "Alt+Shift+F5"
    )
} catch [System.Management.Automation.MethodInvocationException] {
    if ($_ -notmatch "already in use by the menu or editor functionality") {
        throw $_
    }
}



Set-Location $PSScriptRoot


#git reflog is trickier than git log, but git log has the disadvatage that when you git reset
#it no longer contains the commits 'ahead' of current head
#workaround - use a global variable. This script won't step forward to commits made after
#global variable was created.
if (!$Global:GitlogText) {$Global:GitlogText = git log --oneline}

$HeadHash = (git reflog | select -First 1) -replace ' .*'

$CommitHashes = New-Object 'System.Collections.Generic.List[string]'($GitlogText.Count)
$CommitComments = @{}
$GitlogText | %{
    $hash, $comment = $_.Split(' ', 2)
    $CommitHashes.Add($hash)
    $CommitComments.$hash = $comment
}

$HeadIndex = $CommitHashes.LastIndexOf($HeadHash)

if ($Reverse) {$HeadIndex++} else {$HeadIndex--}

if ($HeadIndex -lt 0 -or $HeadIndex -ge $CommitHashes.Count) {
    Write-Host -ForegroundColor DarkYellow 'Already at the end'
} else {
    $HeadHash = $CommitHashes[$HeadIndex]
    if ((git diff) -or (git diff --cached)) {[void](git stash)}
    [void](git reset $HeadHash --hard)
}


$CommitHashes | %{
    Write-Host -ForegroundColor $(
        if ($_ -eq $HeadHash) {"Green"} else {"Gray"}
    ) ([string]::Format("{0}   {1}", $_, $CommitComments[$_]))
}

$psISE.CurrentFile.Save()