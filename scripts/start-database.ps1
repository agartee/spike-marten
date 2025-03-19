$dockerImage = "postgres"
$dockerContainer = "postgres"

docker pull $dockerImage

docker run -d `
  --name $dockerContainer `
  -e POSTGRES_USER=admin `
  -e POSTGRES_PASSWORD=secret `
  -e POSTGRES_DB=mydatabase `
  -p 5432:5432 `
  $dockerImage
