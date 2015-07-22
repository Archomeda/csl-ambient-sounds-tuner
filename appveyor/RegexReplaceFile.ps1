Function RegexReplaceFile($file, $regex, $replace, $encoding = "ASCII") {
    (Get-Content $file -encoding $encoding) -replace $regex,$replace | Set-Content $file -encoding $encoding
}
