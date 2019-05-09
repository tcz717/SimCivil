$App = "SimCivil.Orleans.Server.exe"
if ( Test-Path $App )
{
	Start-Process -FilePath $App -ArgumentList								`
		"Cluster:ClusterId=`"$CLUSTER_CLUSTERID`"",							`
		"DynamoDBClustering:Service=`"$DYNAMODBCLUSTERING_SERVICE`"",		`
		"DynamoDBClustering:AccessKey=`"$DYNAMODBCLUSTERING_ACCESSKEY`"",	`
		"DynamoDBClustering:SecretKey=`"$DYNAMODBCLUSTERING_SECRETKEY`""
}