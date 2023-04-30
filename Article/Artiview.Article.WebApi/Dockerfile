#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Article/Artiview.Article.WebApi/Artiview.Article.WebApi.csproj", "Article/Artiview.Article.WebApi/"]
COPY ["Article/Artiview.Article.Application/Artiview.Article.Application.csproj", "Article/Artiview.Article.Application/"]
COPY ["Article/Artiview.Article.Domain/Artiview.Article.Domain.csproj", "Article/Artiview.Article.Domain/"]
COPY ["Article/Artiview.Article.Infrastructure/Artiview.Article.Infrastructure.csproj", "Article/Artiview.Article.Infrastructure/"]
RUN dotnet restore "Article/Artiview.Article.WebApi/Artiview.Article.WebApi.csproj"
COPY . .
WORKDIR "/src/Article/Artiview.Article.WebApi"
RUN dotnet build "Artiview.Article.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Artiview.Article.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Artiview.Article.WebApi.dll"]