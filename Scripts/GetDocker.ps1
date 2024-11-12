# PowerShell script to install Docker on Windows | FIX

# Function to check if Docker is already installed
function Check-DockerInstalled {
    try {
        $dockerVersion = docker --version
        Write-Output "Docker is already installed: $dockerVersion"
        return $true
    } catch {
        Write-Output "Docker is not installed."
        return $false
    }
}

# Function to download and install Docker Desktop
function Install-Docker {
    Write-Output "Downloading Docker Desktop..."

    # Docker Desktop download URL
    $dockerUrl = "https://desktop.docker.com/win/stable/Docker%20Desktop%20Installer.exe"
    $installerPath = "$env:TEMP\DockerDesktopInstaller.exe"

    # Download the Docker installer
    Invoke-WebRequest -Uri $dockerUrl -OutFile $installerPath

    Write-Output "Installing Docker Desktop..."
    
    # Run the installer silently
    Start-Process -FilePath $installerPath -ArgumentList "/quiet" -Wait

    # Clean up
    Remove-Item $installerPath

    Write-Output "Docker installation complete."
}

# Check for existing Docker installation
if (-not (Check-DockerInstalled)) {
    Install-Docker
} else {
    Write-Output "Docker is already installed. No action needed."
}
