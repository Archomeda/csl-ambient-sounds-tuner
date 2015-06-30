# Restore our NuGet packages
nuget sources Add -Name CitiesSkylines -Source $env:NUGET_CSL_URL -UserName $env:NUGET_CSL_USERNAME -Password $env:NUGET_CSL_PASSWORD -Verbosity quiet -NonInteractive
if ($LASTEXITCODE -gt 0) { exit $LASTEXITCODE }
nuget restore .\appveyor\packages.config -SolutionDirectory .\ -NonInteractive
if ($LASTEXITCODE -gt 0) { exit $LASTEXITCODE }

[xml]$packages = Get-Content .\appveyor\packages.config
$referencePath = ($packages.packages.package | % {(Get-Location).ToString() + "\packages\$($_.id).$($_.version)\lib\$($_.targetFramework)\"}) -Join ";"


# Do the actual build
msbuild /logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll" /verbosity:minimal /p:ReferencePath="$referencePath"
if ($LASTEXITCODE -gt 0) { exit $LASTEXITCODE }


# Copy the files we need to .\bin
if (Test-Path bin) {
    Remove-Item bin -recurse -force
}
mkdir bin | Out-Null;
Copy-Item "CSL Ambient Sounds Tuner\bin\$env:CONFIGURATION\*" -Destination bin -Recurse -Force -Exclude @("*.pdb")


# Copy the files we need to .\workshop
if (Test-Path workshop) {
    Remove-Item workshop -recurse -force
}
mkdir workshop | Out-Null;
mkdir workshop\Content | Out-Null;
Copy-Item PreviewImage.png -destination workshop
Copy-Item "bin/*" -destination workshop\Content -recurse
