$App = "SimCivil.Gate.exe"
if ($env:APPLICATION_PATH -Match "net461")
{
	Set-Location $env:APPLICATION_PATH
	if ( Test-Path $App )
	{
		Start-Sleep -s 10
		Write-Output "Start App "$App
		Start-Process -FilePath $App -ArgumentList								`
			"Cluster:ClusterId=`"$env:CLUSTER_CLUSTERID`"",							`
			"DynamoDBClustering:Service=`"$env:DYNAMODBCLUSTERING_SERVICE`"",		`
			"DynamoDBClustering:AccessKey=`"$env:DYNAMODBCLUSTERING_ACCESSKEY`"",	`
			"DynamoDBClustering:SecretKey=`"$env:DYNAMODBCLUSTERING_SECRETKEY`""
	} else {
		Write-Error $App" not found"
		Write-Output (Get-Location)
		Write-Output (Get-Variable)
		Write-Output (Get-ChildItem Env:)
	}
}