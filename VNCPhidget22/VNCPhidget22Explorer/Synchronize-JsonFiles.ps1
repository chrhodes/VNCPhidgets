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



$jsonOutputFolder = "bin\Debug\net8.0-windows\Resources\json"
# $jsonOutputFolder = ".\bin\Release\net8.0\windows\Resources\json"
$jsonSourceFolder = "Resources\json"

$templateMaster = "VNC_PT_APPLICATION_PrismDxWPF_EF"

$sourceMaster = "ProjectTemplates$($AB)\VNC\$($templateMaster)"

$templateCoreMaster = "VNC_PT_APPLICATION.Core_EF"

$sourceCoreMaster = "ProjectTemplates$($AB)\VNC\$($templateCoreMaster)"


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

function CompareAndUpdateFile ([System.IO.FileInfo]$masterFile, [System.IO.FileInfo]$targetFile, [string] $targetTemplateFolder)
{
    if ($masterFile.LastWriteTime -ne $targetFile.LastWriteTime)
    {
        Write-Verbose "  master: $($masterFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))"
        Write-Verbose "  target: $($targetFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))"
        if ($masterFile.LastWriteTime -gt $targetFile.LastWriteTime)
        {
            $newer = "Master"
            [System.ConsoleColor]$foreGroundColor = "Green"
        }
        else
        {
            $newer = "Target"
            [System.ConsoleColor]$foreGroundColor = "DarkYellow"
        }

        # Write-Host -ForegroundColor Red "$($masterFile.Name) $($masterFile.LastWriteTime) $($masterFile.Length)"
        Write-Host -ForegroundColor Yellow " > $($targetFile.Name) $($targetFile.LastWriteTime) $($targetFile.Length)"
        Write-Host "   in $($targetTemplateFolder)"
        Write-Host -ForegroundColor $foreGroundColor.ToString() "    Last Write Time Different - $newer newer"

        if ($masterFile.LastWriteTime -gt $targetFile.LastWriteTime)
        {
            Write-Host -ForegroundColor Green "      master: " $masterFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff")
            Write-Host "      target: " $targetFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff")

            if ($confirmUpdate)
            {
                while( -not ( ($choice= (Read-Host "Copy Master to Target")) -match "^(y|n)$")){ "Y or N ?"}

                if ($choice -eq "y")
                {
                    Write-Host "Copying Master to Target"
                    Copy-Item -Path $masterFile -Destination $targetFile
                    $targetFile.LastWriteTime = $masterFile.LastWriteTime

                    Write-Verbose "  master: $($masterFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))"
                    Write-Verbose "  target: $($targetFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))"

                    # Read-Host "Enter to continue"
                }
                else
                {
                    "Skipping"
                }
            }
            else
            {
                Write-Host "Copying Master to Target"
                Copy-Item -Path $masterFile -Destination $targetFile
            }
        }
        else
        {
            Write-Host -ForegroundColor DarkYellow "      target: " $targetFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff")
            Write-Host "      master: " $masterFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff")

            if ($confirmUpdate)
            {
                while( -not ( ($choice= (Read-Host "Copy Target to Master")) -match "^(y|n)$")){ "Y or N ?"}

                if ($choice -eq "y")
                {
                    Write-Host "Copying Target to Master"
                    Copy-Item -Path $targetFile -Destination $masterFile
                    $masterFile.LastWriteTime = $targetFile.LastWriteTime

                    Write-Verbose "  master: $($masterFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))"
                    Write-Verbose "  target: $($targetFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))"

                    # Read-Host "Enter to continue"
                }
                else
                {
                    "Skipping"
                }
            }
            else
            {
                Write-Host "Copying Target to Master"
                Copy-Item -Path $targetFile -Destination $masterFile
            }

            # This means masterFile may need to propagate to other places

            $script:masterFileUpdated = $true
        }
    }
    else
    {
        Write-Verbose "LastWriteTime match"

        if ($masterFile.Length -ne $targetFile.Length)
        {
            Write-Host "++++++++++ Lengths do not match ++++++++++ Using Master"

            Copy-Item -Path $masterFile -Destination $targetFile
            $targetFile.LastWriteTime = $masterFile.LastWriteTime
        }
        else
        {
            Write-Verbose "Length Match"
        }
    }
}

function UpdateMatchingFile([string] $fileName, [string] $folderName)
{
    # Set-Location $PSScriptRoot
    $sourceFile = "$($PSScriptRoot)\$($jsonOutputFolder)\$($folderName)\$($fileName)"
    $targetFile = "$($PSScriptRoot)\$($jsonSourceFolder)\$($folderName)\$($fileName)"

    if (Test-Path $targetFile)
    {
        $sourceFileInfo = Get-ChildItem $sourceFile

        Write-Host -ForegroundColor Blue ("{0,-60} - {1} {2}" -f $sourceFile, $sourceFileInfo.LastWriteTime, $sourceFileInfo.Length)

        $targetFileInfo = Get-ChildItem $targetFile

        Write-Host -ForegroundColor Blue ("{0,-60} - {1} {2}" -f $targetFile, $targetFileInfo.LastWriteTime, $targetFileInfo.Length)
 
        # CompareAndUpdateFile $masterFileInfo $fileInfo "$($projectTemplates)\$($targetTemplateFolder)"

        Write-Verbose ""
    }
    else
    {
        Write-Error "$targetFile does not exist"
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

Set-Location $PSScriptRoot\$jsonOutputFolder

WriteDelimitedMessage "Updating $jsonSourceFolder using files in $jsonOutputFolder"

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