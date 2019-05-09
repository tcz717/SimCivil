if (Test-Path SimCivil.Gate.exe)
{
	.\SimCivil.Gate.exe													`
		Cluster:ClusterId="$CLUSTER_CLUSTERID"							`
		DynamoDBClustering:Service="$DYNAMODBCLUSTERING_SERVICE"		`
		DynamoDBClustering:AccessKey="$DYNAMODBCLUSTERING_ACCESSKEY"	`
		DynamoDBClustering:SecretKey="$DYNAMODBCLUSTERING_SECRETKEY"
}