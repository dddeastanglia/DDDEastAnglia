$rootDir = join-path (Split-Path $MyInvocation.MyCommand.Path) ".."

& "${rootDir}\build\environment.ps1" $rootDir

Import-Module (join-path $rootDir "psake\psake.psm1")
invoke-psake -framework '4.5.1' (join-path $rootDir "build\build.ps1") -properties @{"root"="$rootDir";}
