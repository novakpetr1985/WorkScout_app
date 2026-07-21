[CmdletBinding()]
param(
    [string] $Runtime = "win-x64"
)

$ErrorActionPreference = "Stop"
$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path
$projectPath = Join-Path $repositoryRoot "WorkScout_app.csproj"
[xml] $project = Get-Content -LiteralPath $projectPath
$version = ($project.Project.PropertyGroup | Where-Object Version | Select-Object -First 1).Version

if ([string]::IsNullOrWhiteSpace($version)) {
    throw "Version is missing in WorkScout_app.csproj."
}

$publishRoot = Join-Path $repositoryRoot "artifacts\publish"
$releaseRoot = Join-Path $repositoryRoot "artifacts\release"
$releaseName = "WorkScout-$version-$Runtime"
$publishDirectory = Join-Path $publishRoot $releaseName
$archivePath = Join-Path $releaseRoot "$releaseName.zip"

New-Item -ItemType Directory -Force -Path $publishDirectory, $releaseRoot | Out-Null

dotnet restore $projectPath -r $Runtime --configfile (Join-Path $repositoryRoot "NuGet.Config")
if ($LASTEXITCODE -ne 0) { throw "dotnet restore failed." }

dotnet publish $projectPath `
    --configuration Release `
    --runtime $Runtime `
    --self-contained false `
    --no-restore `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    --output $publishDirectory
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed." }

Get-ChildItem -LiteralPath $publishDirectory -Filter *.pdb | Remove-Item
Copy-Item -LiteralPath (Join-Path $repositoryRoot "README.md") -Destination $publishDirectory -Force
Copy-Item -LiteralPath (Join-Path $repositoryRoot "docs\CHANGELOG.md") -Destination $publishDirectory -Force
Compress-Archive -Path "$publishDirectory\*" -DestinationPath $archivePath -CompressionLevel Optimal -Force

$hash = (Get-FileHash -LiteralPath $archivePath -Algorithm SHA256).Hash
"$hash  $releaseName.zip" | Set-Content -LiteralPath (Join-Path $releaseRoot "SHA256.txt")

Write-Host "Published $releaseName"
Write-Host "Archive: $archivePath"
Write-Host "SHA256: $hash"
