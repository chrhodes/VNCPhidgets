
$folders = @(
    "VNC.Phidget21"
    , "VNCPhidget21.Configuration"
    , "VNCPhidgets21Explorer"
    , "VNCPhidgets21Explorer.Core"
    )

foreach ($folder in $folders)
{
    "Removing obj\ and bin\ folder contents in $folder"

    if (Test-Path -Path $folder\obj)
    {
        remove-item $folder\obj -Recurse -Force
    }

    if (Test-Path -Path $folder\bin)
    {
        remove-item $folder\bin -Recurse -Force
    }

}

Read-Host -Prompt "Press Enter to Exit"