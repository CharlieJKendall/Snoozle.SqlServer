if(-not $Env:BUILD_SOURCEBRANCHNAME)
{
    Write-Error "The $Build.SourceBranchName environment variable must be set"
    exit 1
}

if(-not $Env:BUILD_BUILDID)
{
    Write-Error "The $Build.BuildId environment variable must be set"
    exit 1
}

$MajorMinorFromBranch = $Env:BUILD_SOURCEBRANCHNAME
Write-Host "Major.Minor found from branch name: $MajorMinorFromBranch"

$MostRecentVersion = git describe --tags --abbrev=0
Write-Host "Most recent version from git describe: $MostRecentVersion"

$MostRecentVersionObject = New-Object System.Version($MostRecentVersion)
$MostRecentMajorMinor = $MostRecentVersionObject.Major.ToString() + '.' + $MostRecentVersionObject.Minor.ToString()

$NewVersion

if ($MostRecentMajorMinor -eq $MajorMinorFromBranch)
{
    Write-Host "Major.Minor branch version matches most recent tag version, incrementing patch number"
    $Patch = $MostRecentVersionObject.Build + 1
    $NewVersion = "$MajorMinorFromBranch.$Patch.$Env:BUILD_BUILDID"
}
else
{
    Write-Host "Major.Minor branch version does not match most recent tag version, setting patch number as 0"
    $NewVersion = "$MajorMinorFromBranch.0.$Env:BUILD_BUILDID"
}

Write-Host "New version set to $NewVersion"
Write-Host "##vso[task.setvariable variable=Version]$NewVersion"