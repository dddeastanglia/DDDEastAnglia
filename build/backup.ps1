# the following parameters must be supplied to Invoke-Psake's -Parameters argument 
# (as a hashtable) for use in the connection string. See run-backup.ps1 for an example.
#   - username
#   - password
#   - hostname
#   - database

properties {
    $root = $null
}

Include .\teamcity.ps1
TaskSetup {
    TeamCity-ReportBuildProgress "Running task '$($psake.context.Peek().currentTaskName)'"
}

Task default -Depends Backup

Task Backup {
    Write-Host "Backing up the DDDEastAnglia from Azure" -ForegroundColor Green

    $dateAndTime = Get-Date -Format "yyyy-MM-dd-HH-mm"
    $backupLocation = "$root\backups"

    # Delete any existing directory at the backup location, and create a fresh one.

    if (Test-Path $backupLocation -PathType Container) 
    {
        Remove-Item -Recurse -Force $backupLocation
    }

    New-Item $backupLocation -ItemType Container | Out-Null # discard the output object representing the created directory: stops clutter of the console.

    # And now we can actually run the backup. Export the database to a BACPAC file
    # (a deployment artefact encompassing both schema and data).

    Exec { 
        & "$root\build\targets\SSDT\SqlPackage.exe" /action:export `
        /sourceconnectionstring:"Server=$hostname;Database=$database;User ID=$username;Password=$password;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;" `
        /targetfile:"$backupLocation\DDDEastAnglia-$dateAndTime.bacpac" 
    }
}
