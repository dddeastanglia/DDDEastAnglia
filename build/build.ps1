. .\helpers.ps1

properties {
	$root = Join-Path (Get-ScriptDirectory) ".."
}

Task default -Depends Compile

Task Compile -Depends Clean {
    Write-Host "Building DDDEastAnglia.sln" -ForegroundColor Green
	Exec { msbuild "${root}\DDDEastAnglia.sln" /t:Build /p:Configuration=Release /m }
}

Task Clean {
    Write-Host "Cleaning DDDEastAnglia.sln" -ForegroundColor Green
    Exec { msbuild "${root}\DDDEastAnglia.sln" /t:Clean /m }
}

