$name = "SimCivil.Gate.exe"
if ((Get-Process).Where({ $_.Name -Match $name }, 'First').Count -gt 0 ) {
	Stop-Process -Name $name
}