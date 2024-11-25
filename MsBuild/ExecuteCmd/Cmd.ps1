param(
  [Parameter(Mandatory)]
  [string] $Name
)

$ErrorActionPreference = 'Stop'

Write-Output "Hello, $Name!"
