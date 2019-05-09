if (Test-Path SimCivil.Orleans.Server.exe)
{
	.\SimCivil.Orleans.Server.exe										`
		Cluster:ClusterId="$CLUSTER_CLUSTERID"							`
		DynamoDBClustering:Service="$DYNAMODBCLUSTERING_SERVICE"		`
		DynamoDBClustering:AccessKey="$DYNAMODBCLUSTERING_ACCESSKEY"	`
		DynamoDBClustering:SecretKey="$DYNAMODBCLUSTERING_SECRETKEY"
}