[CmdletBinding()]
param()

$scriptRoot = $PSScriptRoot

$writeInfoSplat = @{
    "InformationAction" = "Continue";
}

$bootstrapCssPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap\dist\css\bootstrap.min.css"
$bootstrapCssMapPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap\dist\css\bootstrap.min.css.map"
$bootstrapOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\css\bootstrap\"

$bootstrapIconsCssPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap-icons\font\bootstrap-icons.css"
$bootstrapIconsFontDirPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap-icons\font\fonts\"
$bootstrapIconsOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\css\bootstrap-icons\"

$highlightJsPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\@highlightjs\cdn-assets\highlight.min.js"
$highlightJsCssPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\@highlightjs\cdn-assets\styles\github.min.css"
$highlightJsOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\highlight.js\"
$highlightJsCssOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\highlight.js\styles\"

if (!(Test-Path -Path $bootstrapOutPath)) {
    $null = New-Item -Path $bootstrapOutPath -ItemType "Directory"
}

foreach ($fileItem in (Get-ChildItem -Path $bootstrapOutPath)) {
    Write-Information @writeInfoSplat -MessageData "`t| Removing '$($fileItem.Name)'"
    Remove-Item -Path $fileItem.FullName -Force
}

Write-Information @writeInfoSplat -MessageData "`t| bootstrap.min.css-> $($bootstrapOutPath)"
Copy-Item -Path $bootstrapCssPath -Destination $bootstrapOutPath -ErrorAction "Stop"

Write-Information @writeInfoSplat -MessageData "`t| bootstrap.min.css.map-> $($bootstrapOutPath)"
Copy-Item -Path $bootstrapCssMapPath -Destination $bootstrapOutPath -ErrorAction "Stop"

if (!(Test-Path -Path $bootstrapIconsOutPath)) {
    $null = New-Item -Path $bootstrapIconsOutPath -ItemType "Directory"
}

foreach ($fileItem in (Get-ChildItem -Path $bootstrapIconsOutPath)) {
    Write-Information @writeInfoSplat -MessageData "`t| Removing '$($fileItem.Name)'"
    Remove-Item -Path $fileItem.FullName -Force -Recurse
}

Write-Information @writeInfoSplat -MessageData "`t| bootstrap-icons.css-> $($bootstrapIconsOutPath)"
Copy-Item -Path $bootstrapIconsCssPath -Destination $bootstrapIconsOutPath -ErrorAction "Stop"

Write-Information @writeInfoSplat -MessageData "`t| fonts\-> $($bootstrapIconsOutPath)"
Copy-Item -Path $bootstrapIconsFontDirPath -Destination $bootstrapIconsOutPath -Recurse -ErrorAction "Stop"

if (Test-Path -Path $highlightJsOutPath) {
    Remove-Item -Path $highlightJsOutPath -Recurse -Force
}

$null = New-Item -Path $highlightJsOutPath -ItemType "Directory"
$null = New-Item -Path $highlightJsCssOutPath -ItemType "Directory"

Write-Information @writeInfoSplat -MessageData "`t| highlight.min.js-> $($highlightJsOutPath)"
Copy-Item -Path $highlightJsPath -Destination $highlightJsOutPath -ErrorAction "Stop"

Write-Information @writeInfoSplat -MessageData "`t| styles\github.min.css-> $($highlightJsCssOutPath)"
Copy-Item -Path $highlightJsCssPath -Destination $highlightJsCssOutPath -Recurse -ErrorAction "Stop"