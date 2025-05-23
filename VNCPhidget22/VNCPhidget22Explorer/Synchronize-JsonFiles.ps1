<#
    Synchronize-JsonFiles.ps1

    Description

    TODO(crhodes)
    Turn into proper script with description and usage
#>

#region Variables and Configuration

# Set to A for ProjectTemplatesA and ItemTemplatesA,
# Set to B for ...

# $AB = "A"

$jsonExecutionFolder = "bin\Debug\net8.0-windows\Resources\json"
# $jsonExecutionFolder = ".\bin\Release\net8.0\windows\Resources\json"
$jsonSourceFolder = "Resources\json"

#$VerbosePreference = 'Continue'
$VerbosePreference = 'Ignore'

# set to $false to have the magic just happen, $true to prompt before changes

$confirmUpdate = $false

# $ErrorActionPreference = 'Break'

#endregion

#region Functions

function WriteDelimitedMessage($msg)
{
    $delimitS = "********** >>> "
    $delimitE = " <<< **********"
    # $delimitS = ""
    # $delimitE = ""

    Write-Host ""
    # Write-Host -ForegroundColor Red $delimitS ("{0,-30}" -f $msg) $delimitE
    Write-Host -ForegroundColor Red $delimitS ("{0}" -f $msg) $delimitE
    Write-Host ""
}

function CompareAndUpdateFile ([System.IO.FileInfo]$executionFile, [System.IO.FileInfo]$sourceFile, [string] $targetTemplateFolder)
{
    if ($executionFile.LastWriteTime -ne $sourceFile.LastWriteTime)
    {
        Write-Verbose "  master: $($executionFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))"
        Write-Verbose "  target: $($sourceFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))"

        if ($executionFile.LastWriteTime -gt $sourceFile.LastWriteTime)
        {
            $newer = "Execution"
            [System.ConsoleColor]$foreGroundColor = "Green"
        }
        else
        {
            $newer = "Source"
            [System.ConsoleColor]$foreGroundColor = "DarkYellow"
        }

        # Write-Host -ForegroundColor Red "$($executionFile.Name) $($executionFile.LastWriteTime) $($executionFile.Length)"
        Write-Host -ForegroundColor Yellow " > $($sourceFile.Name) $($sourceFile.LastWriteTime) $($sourceFile.Length)"
        Write-Host "   in $($targetTemplateFolder)"
        Write-Host -ForegroundColor $foreGroundColor.ToString() "    Last Write Time Different - $newer newer"

        if ($executionFile.LastWriteTime -gt $sourceFile.LastWriteTime)
        {
            Write-Host -ForegroundColor Green "      execution: " $executionFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff")
            Write-Host "      source:    " $sourceFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff")

            Write-Host "Copying Execution to Source"

            if ($confirmUpdate)
            {
                while( -not ( ($choice= (Read-Host "Copy Execution to Source")) -match "^(y|n)$")){ "Y or N ?"}

                if ($choice -eq "y")
                {
                    Copy-Item -Path $executionFile -Destination $sourceFile
                    # $sourceFile.LastWriteTime = $executionFile.LastWriteTime

                    # Write-Verbose "  master: $($executionFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))"
                    # Write-Verbose "  target: $($sourceFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))"

                    # Read-Host "Enter to continue"
                }
                else
                {
                    "Skipping"
                }
            }
            else
            {                
                Copy-Item -Path $executionFile -Destination $sourceFile
            }
        }
    }
    else
    {
        Write-Verbose "LastWriteTime match"

        if ($executionFile.Length -ne $sourceFile.Length)
        {
            Write-Host "++++++++++ Lengths do not match ++++++++++ Using Execution"

            # Copy-Item -Path $executionFile -Destination $sourceFile
            # $sourceFile.LastWriteTime = $executionFile.LastWriteTime
        }
        else
        {
            Write-Verbose "Length Match"
        }
    }
}

function UpdateMatchingFile([string] $fileName, [string] $folderName)
{
    $jsonFilePath = "$($folderName)\$($fileName)"
    # Set-Location $PSScriptRoot
    $executionFilePath = "$($PSScriptRoot)\$($jsonExecutionFolder)\$($jsonFilePath)"
    $sourceFilePath = "$($PSScriptRoot)\$($jsonSourceFolder)\$($jsonFilePath)"

    if (Test-Path $sourceFilePath)
    {
        $executionFile = Get-ChildItem $executionFilePath

        $sourceFile = Get-ChildItem $sourceFilePath

        CompareAndUpdateFile $executionFile $sourceFile "$($folderName)"

        Write-Verbose ""
    }
    else
    {
        Write-Error "$($jsonFilePath) does not exist in $($jsonSourceFolder), ignoring."
    }
}

function GetFilesInFolder([string] $folderName)
{
    Get-ChildItem -Path $folderName -File
}

function GetDirectoriesInFolder([string] $folderName)
{
    Get-ChildItem -Path $folderName -Directory
}

function ProcessFilesInFolder([string] $folderName)
{
    Write-Host "Processing Files in $folderName"

    foreach ($file in (GetFilesInFolder $folderName))
    {
        Write-Host "Existing File - $($file.Name)"
        UpdateMatchingFile $file.Name $folderName
    }

    foreach ($subFolder in (GetDirectoriesInFolder $folderName))
    {
        if ($folderName -eq "")
        {
            ProcessFilesInFolder $subFolder.Name
        }
        else
        {
            ProcessFilesInFolder "$($folderName)\$($subFolder.Name)"
        }
    }
}

#endregion

Set-Location $PSScriptRoot\$jsonExecutionFolder

WriteDelimitedMessage "Updating $jsonSourceFolder using files in $jsonExecutionFolder"

ProcessFilesInFolder ""

# ProcessProjectFiles

# ProcessItemFiles

# if ($masterFileUpdated)
# {
#     Write-Host "Master file(s) updated.  Propagating changes"
#     ProcessProjectFiles
#     ProcessItemFiles
# }

# ProcessCoreFiles

# if ($masterCoreFileUpdated)
# {
#     Write-Host "Master Core file(s) updated.  Propagating changes"
#     ProcessCoreFiles
# }

# WriteDelimitedMessage "Synchronization Complete !"


# Read-Host -Prompt "Press Enter to Exit"

# End of .\Synchronize-VNCTemplateFiles.ps1