properties {
	$root = $null
}

Include .\teamcity.ps1
TaskSetup {
    TeamCity-ReportBuildProgress "Running task '$($psake.context.Peek().currentTaskName)'"
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

