param(
    [string]$Version = "0.1.0"
)

$ErrorActionPreference = "Stop"

$scriptDir = $PSScriptRoot

$projectFile = Join-Path $scriptDir "omni-multitool.csproj"
$publishDir = Join-Path $scriptDir "publish\win-x64"
$installerFile = Join-Path $scriptDir "installer\omni-multitool.iss"
$installerOutputDir = Join-Path $scriptDir "installer\Output"
$distDir = Join-Path $scriptDir "dist"


Push-Location $scriptDir

try {

    Write-Host ""
    Write-Host "========================================"
    Write-Host " Cleaning previous build outputs"
    Write-Host "========================================"

    Remove-Item $publishDir -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item $installerOutputDir -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item $distDir -Recurse -Force -ErrorAction SilentlyContinue


    Write-Host ""
    Write-Host "========================================"
    Write-Host " Publishing Omni Multitool $Version"
    Write-Host "========================================"

    dotnet publish $projectFile `
        -c Release `
        -r win-x64 `
        --self-contained true `
        -p:PublishSingleFile=true `
        -p:IncludeNativeLibrariesForSelfExtract=true `
        -p:EnableCompressionInSingleFile=false `
        -p:PublishTrimmed=false `
        -o $publishDir

    if ($LASTEXITCODE -ne 0) {
        throw "dotnet publish failed."
    }


    Write-Host ""
    Write-Host "========================================"
    Write-Host " Verifying publish output"
    Write-Host "========================================"

    if (-not (Test-Path $publishDir)) {
        throw "Publish directory was not created: $publishDir"
    }

    $publishedExe = Join-Path $publishDir "omni-multitool.exe"

    if (-not (Test-Path $publishedExe)) {
        throw "Published executable was not found: $publishedExe"
    }

    Write-Host "Publish successful."
    Write-Host "Output: $publishDir"


    Write-Host ""
    Write-Host "========================================"
    Write-Host " Locating Inno Setup"
    Write-Host "========================================"

    $innoExe = $null

    $isccCommand = Get-Command iscc.exe -ErrorAction SilentlyContinue

    if ($isccCommand) {
        $innoExe = $isccCommand.Source
    }
    else {
        $commonInnoPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"

        if (Test-Path $commonInnoPath) {
            $innoExe = $commonInnoPath
        }
        else {
            throw "Inno Setup (ISCC.exe) could not be found."
        }
    }

    Write-Host "Inno Setup: $innoExe"


    Write-Host ""
    Write-Host "========================================"
    Write-Host " Compiling installer"
    Write-Host "========================================"

    if (-not (Test-Path $installerFile)) {
        throw "Inno Setup script was not found: $installerFile"
    }

    & $innoExe "/DAppVersion=$Version" $installerFile

    if ($LASTEXITCODE -ne 0) {
        throw "Inno Setup compilation failed."
    }


    Write-Host ""
    Write-Host "========================================"
    Write-Host " Locating installer"
    Write-Host "========================================"

    $installerExe = Join-Path `
        $installerOutputDir `
        "Omni-Multitool-$Version-Setup.exe"

    if (-not (Test-Path $installerExe)) {

        Write-Host "Expected installer filename was not found."
        Write-Host "Looking for any generated installer..."

        $installerCandidates = Get-ChildItem `
            -Path $installerOutputDir `
            -Filter "*.exe" `
            -File

        if ($installerCandidates.Count -eq 0) {
            throw "No installer executable was generated."
        }

        $installerExe = $installerCandidates[0].FullName
    }


    Write-Host ""
    Write-Host "========================================"
    Write-Host " Preparing distribution"
    Write-Host "========================================"

    New-Item `
        -ItemType Directory `
        -Path $distDir `
        -Force | Out-Null

    $distExe = Join-Path `
        $distDir `
        (Split-Path $installerExe -Leaf)

    Copy-Item `
        $installerExe `
        $distExe `
        -Force


    Write-Host ""
    Write-Host "========================================"
    Write-Host " BUILD COMPLETE"
    Write-Host "========================================"
    Write-Host ""
    Write-Host "Installer:"
    Write-Host $distExe
    Write-Host ""

    Start-Process explorer.exe "/select,`"$distExe`""
}
finally {
    Pop-Location
}