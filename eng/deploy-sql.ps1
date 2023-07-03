# SPDX-License-Identifier: Apache-2.0
# Licensed to the Ed-Fi Alliance under one or more agreements.
# The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
# See the LICENSE and NOTICES files in the project root for more information.

#requires -version 5

<#
.SYNOPSIS
    Deploy EPPETA database scripts
#>
[CmdLetBinding()]
param (
    # SQL Server host name and (optional) port.
    [string]
    $Server = "localhost",

    # Database name.
    [string]
    $DatabaseName = "eppeta",

    # (Optional) Use to enforce encryption of the database connection in transit.
    [switch]
    $EncryptConnection,

    # (Optional) Use to trust the SQL Server certificate.
    [switch]
    $TrustCertificate,

    # (Optional) SQL User account username.
    [string]
    $Username,

    # (Optional) SQL user account password.
    [string]
    $Pass
)

$ErrorActionPreference = "Stop"

$connectionString = "Server=$Server;Database=$DatabaseName;Application Name=deploy-sql.ps1;"
if ($Username) {
    if ($Pass) {
        $connectionString += "User Id=$Username;Password=$Pass;"
    }
    else {
        throw "Must provide password when providing a username"
    }
}
else {
    $connectionString += "Integrated Security=SSPI;"
}
if ($EncryptConnection) {
    $connectionString += "Encrypt=true;"
}
if ($TrustCertificate) {
    $connectionString += "TrustServerCertificate=true;"
}

Write-Output "Deploying EPP Evaluation Tracker Application database scripts"

Get-ChildItem "$PSScriptRoot/../sql/mssql/*.sql" | `
    Sort-Object { $_.Name } | `
    ForEach-Object {
        $name = $_.Name

        try {
            # See if the deploy journal already exists
            $statement = "select 1 from eppeta.deployjournal where scriptname = '$name'"
            $result = Invoke-Sqlcmd `
                -ConnectionString $connectionString `
                -Query $statement

            if ($result.Count -gt 0) {
                # Script has already run, so skip it
                continue
            }
        }
        catch {
            if (-not $_.Exception.Message.StartsWith("Invalid object name 'eppeta.deployjournal'")) {
                # One first execution of the script, this object won't exist.
                # That is expected and fine. The deployment script will create
                # this table. But any _other_ error is not fine, and we should
                # rethrow.
                throw
            }
        }

        # Run the SQL script
        Write-Output "Installing $name"
        Invoke-Sqlcmd `
            -ConnectionString $connectionString `
            -InputFile $_

        # Record the script in the deploy journal
        $statement = "INSERT INTO eppeta.deployjournal (scriptname, applied) values ('$name', '$((Get-Date).ToUniversalTime())');"
        Invoke-Sqlcmd `
            -ConnectionString $connectionString `
            -Query $statement
    }

Write-Output "Done with database deployment."
