$buildOutputPath = Join-Path -Path $PSScriptRoot -ChildPath "build/"

if (Test-Path -Path $buildOutputPath) {
    Remove-Item -Path $buildOutputPath -Recurse -Force -ErrorAction "SilentlyContinue"
}
$null = New-Item -Path $buildOutputPath -ItemType "Directory" -ErrorAction "SilentlyContinue"