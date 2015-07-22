. .\appveyor\RegexReplaceFile.ps1

if (!$env:APPVEYOR_PULL_REQUEST_NUMBER) {
    if ($env:APPVEYOR_REPO_TAG -eq "true") {
        $match = Select-String -Path "appveyor.yml" -Pattern "version: (\d+)\.(\d+)\.(\d+)\.\{build\}"

        $newVersion = $match.Matches[0].Groups[1].Value + "." + $match.Matches[0].Groups[2].Value + "." + ([convert]::ToInt32($match.Matches[0].Groups[3].Value, 10) + 1)
        Write-Host "Prepare for next version automatically: $newVersion" -ForegroundColor "Yellow"

        Write-Host "  - Apply to appveyor.yml" -ForegroundColor "Yellow"
        $regex = "(version: ).+"
        $replace = "`${1}$newVersion.{build}"
        RegexReplaceFile -file "appveyor.xml" -regex $regex -replace $replace

        Write-Host "  - Commit and push to GitHub repository" -ForegroundColor "Yellow"
        git config --global credential.helper store
        git config --global user.name $env:APPVEYOR_REPO_COMMIT_AUTHOR
        git config --global user.email $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL
        Add-Content "$env:USERPROFILE\.git-credentials" "https://$($env:access_token):x-oauth-basic@github.com`n"
        git remote add github "https://github.com/$($env:APPVEYOR_REPO_NAME).git"
        git checkout -q $env:APPVEYOR_REPO_BRANCH
        git add "appveyor.yml"
        git commit -q -m "[AppVeyor] Prepare for version $newVersion [ci skip]"
        git push -q github master
    }
}