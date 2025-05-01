param (
    [Parameter(Mandatory = $True)] $Name
)

# Get the current timestamp in the format YYYYmmddHHMMss
$timestamp = (Get-Date).ToString("yyyyMMddHHmmss")

# Define the folder path
$folderPath = Join-Path -Path (Split-Path -Path $PSScriptRoot -Parent) -ChildPath "Expenses.Api.Database\Migrations"

# Create the folder if it doesn't exist
if (-not (Test-Path -Path $folderPath))
{
    New-Item -ItemType Directory -Path $folderPath -Force | Out-Null
}

# Define the file name
$fileName = "{0}_{1}.cs" -f $timestamp, $Name

# Define the full path for the new file
$filePath = Join-Path -Path $folderPath -ChildPath $fileName

# Define the content template
$content = @"
using FluentMigrator;

namespace Expenses.Api.Database.Migrations
{
    [Migration($timestamp)]
    public class $Name : Migration
    {
        public override void Up()
        {
        }

        public override void Down()
        {
        }
    }
}
"@

# Write the content to the file
Set-Content -Path $filePath -Value $content

Write-Host "Migration file created: $filePath"
