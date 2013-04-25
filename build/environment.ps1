param([Parameter(Mandatory=$true)][string]$rootDir)

$additionalPaths = @(
    Join-Path $rootDir ".nuget"
)

$env:path = ($additionalPaths -join ";") + ";$env:path"
