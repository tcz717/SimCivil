$name = "SimCivil.Gate.exe"
if ((Get-Process).Where({ $_.Name -Match $name }, 'First').Count -gt 0 -and $env:APPLICATION_PATH -Match "net461") {
	Write-Output (Stop-Process -Name $name)
} else {
	Write-Output $name + " not found"
}