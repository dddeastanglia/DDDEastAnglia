$rootDir = join-path (Split-Path $MyInvocation.MyCommand.Path) ".."
Import-Module (join-path $rootDir "psake\psake.psm1")
invoke-psake -framework '4.0' (join-path $rootDir "build\build.ps1") -properties @{"root"="$rootDir";}