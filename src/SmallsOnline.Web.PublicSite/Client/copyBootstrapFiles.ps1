﻿[CmdletBinding()]
param()

$scriptRoot = $PSScriptRoot

$writeInfoSplat = @{
    "InformationAction" = "Continue";
}

$bootstrapCssPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap\dist\css\bootstrap.css"
$bootstrapCssMapPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap\dist\css\bootstrap.css.map"
$bootstrapOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\css\bootstrap\"

$bootstrapIconsCssPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap-icons\font\bootstrap-icons.css"
$bootstrapIconsFontDirPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap-icons\font\fonts\"
$bootstrapIconsOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\css\bootstrap-icons\"

if (!(Test-Path -Path $bootstrapOutPath)) {
    $null = New-Item -Path $bootstrapOutPath -ItemType "Directory"
}

foreach ($fileItem in (Get-ChildItem -Path $bootstrapOutPath)) {
    Write-Information @writeInfoSplat -MessageData "`t| Removing '$($fileItem.Name)'"
    Remove-Item -Path $fileItem.FullName -Force
}

#Write-Information @writeInfoSplat -MessageData "`t| bootstrap.css-> $($bootstrapOutPath)"
#Copy-Item -Path $bootstrapCssPath -Destination $bootstrapOutPath -ErrorAction "Stop"

Start-Process -FilePath "npm" -ArgumentList @("run", "trimcss-bootstrap") -Wait -NoNewWindow

Write-Information @writeInfoSplat -MessageData "`t| bootstrap.css.map-> $($bootstrapOutPath)"
Copy-Item -Path $bootstrapCssMapPath -Destination $bootstrapOutPath -ErrorAction "Stop"

if (!(Test-Path -Path $bootstrapIconsOutPath)) {
    $null = New-Item -Path $bootstrapIconsOutPath -ItemType "Directory"
}

foreach ($fileItem in (Get-ChildItem -Path $bootstrapIconsOutPath)) {
    Write-Information @writeInfoSplat -MessageData "`t| Removing '$($fileItem.Name)'"
    Remove-Item -Path $fileItem.FullName -Force -Recurse
}

#Write-Information @writeInfoSplat -MessageData "`t| bootstrap-icons.css-> $($bootstrapIconsOutPath)"
#Copy-Item -Path $bootstrapIconsCssPath -Destination $bootstrapIconsOutPath -ErrorAction "Stop"

Start-Process -FilePath "npm" -ArgumentList @("run", "trimcss-bootstrapicons") -Wait -NoNewWindow

Write-Information @writeInfoSplat -MessageData "`t| fonts\-> $($bootstrapIconsOutPath)"
Copy-Item -Path $bootstrapIconsFontDirPath -Destination $bootstrapIconsOutPath -Recurse -ErrorAction "Stop"