docker network create artiview

docker run -d --name mssql -p 14330:1433 --network artiview -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD=FdvyCcY1877 -e MSSQL_PID=Express mcr.microsoft.com/mssql/server:2017-latest-ubuntu

docker build -t artiview/article -f Article\Artiview.Article.WebApi\Dockerfile .
docker build -t artiview/review -f Review\Artiview.Review.WebApi\Dockerfile .

docker run -d --name Artiview.Article.WebApi -p 50000:80 --network artiview -e ASPNETCORE_ENVIRONMENT=Development artiview/article
docker run -d --name Artiview.Review.WebApi -p 50001:80 --network artiview -e ASPNETCORE_ENVIRONMENT=Development artiview/review