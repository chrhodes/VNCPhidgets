
$folders = @(
    "VNC.Phidget22"
    , "VNC.Phidget22.Configuration"
    , "VNC.Phidget22Explorer"
    , "VNC.Phidget22Explorer.Core"
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