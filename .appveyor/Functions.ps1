Function RegexReplaceFile($file, $regex, $replace, $encoding = "ASCII") {
    (Get-Content $file -encoding $encoding) -replace $regex,$replace | Set-Content $file -encoding $encoding
}

Function SetAppVeyorYmlVersion($version) {
    $regex = "(version: ).+"
    $replace = "`${1}$version"
    RegexReplaceFile -file "appveyor.yml" -regex $regex -replace $replace
}

Function SetModVersion($version) {
    $regex = '(get { return ")dev version("; })'
    $replace = "`${1}$version`${2}"
    RegexReplaceFile -file "CSL Ambient Sounds Tuner\Mod.cs" -regex $regex -replace $replace
}
