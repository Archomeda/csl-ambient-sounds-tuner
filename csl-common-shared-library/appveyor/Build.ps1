# Restore our NuGet packages
nuget sources Add -Name CitiesSkylines -Source $env:NUGET_CSL_URL -UserName $env:NUGET_CSL_USERNAME -Password $env:NUGET_CSL_PASSWORD -Verbosity quiet -NonInteractive
nuget restore .\appveyor\packages.config -SolutionDirectory .\ -NonInteractive

[xml]$packages = Get-Content .\appveyor\packages.config
$referencePath = ($packages.packages.package | % {(Get-Location).ToString() + "\packages\$($_.id).$($_.version)\lib\$($_.targetFramework)\"}) -Join ";"


# Do the actual build
msbuild /logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll" /verbosity:minimal /p:ReferencePath="$referencePath"
