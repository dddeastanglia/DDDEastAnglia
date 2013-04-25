properties {
	$root = $null
}

Include .\teamcity.ps1
TaskSetup {
    TeamCity-ReportBuildProgress "Running task '$($psake.context.Peek().currentTaskName)'"
}

Task default -Depends Test

Task Test -Depends Compile {
    $nunitDir = Join-Path ((ls "${root}\packages\*" -Filter NUnit.Runners*) | select -last 1) "tools"
    Write-Host "NUnit Console Runner found at $nunitDir"
    $env:Path = $nunitDir + ";" + $env:Path

    $testAssemblies = @(ls -Recurse "${root}\DDDEastAnglia*\bin" -Filter "DDDEastAnglia*Tests.dll")
    Write-Host ("Found {0} test assemblies: {1}" -f $testAssemblies.Length, ($testAssemblies -join ", "))

    Write-Host "Running tests" -ForegroundColor Green
    Exec { nunit-console $testAssemblies /framework="4.0" /nologo /nodots /result="${root}\TestResult.xml" /process="Separate" }
}

Task Compile -Depends Clean {
    Write-Host "Restoring Solution-level NuGet packages" -ForegroundColor Green	
    Exec { nuget install "${root}\.nuget\packages.config" -OutputDirectory "${root}\packages" }

    Write-Host "Building DDDEastAnglia.sln" -ForegroundColor Green
    Exec { msbuild "${root}\DDDEastAnglia.sln" /t:Build /p:Configuration=Release /m }
}

Task Clean {
    Write-Host "Cleaning DDDEastAnglia.sln" -ForegroundColor Green
    Exec { msbuild "${root}\DDDEastAnglia.sln" /t:Clean /m }
}

