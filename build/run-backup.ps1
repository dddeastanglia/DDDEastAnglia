param(
    [Parameter(Mandatory=$true)][string]$Username,
    [Parameter(Mandatory=$true)][string]$Password,
    [Parameter(Mandatory=$true)][string]$Hostname,
    [Parameter(Mandatory=$true)][string]$Database
)

$rootDir = join-path (Split-Path $MyInvocation.MyCommand.Path) ".."
Import-Module (join-path $rootDir "psake\psake.psm1")
invoke-psake -framework '4.0' (join-path $rootDir "build\backup.ps1") -properties @{"root"="$rootDir";} -parameters @{
    "username"="$Username";
    "password"="$Password";
    "hostname"="$Hostname";
    "database"="$Database";
}
