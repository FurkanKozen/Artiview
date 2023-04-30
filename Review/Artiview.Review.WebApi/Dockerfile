#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Review/Artiview.Review.WebApi/Artiview.Review.WebApi.csproj", "Review/Artiview.Review.WebApi/"]
COPY ["Review/Artiview.Review.Infrastructure/Artiview.Review.Infrastructure.csproj", "Review/Artiview.Review.Infrastructure/"]
COPY ["Review/Artiview.Review.Application/Artiview.Review.Application.csproj", "Review/Artiview.Review.Application/"]
COPY ["Review/Artiview.Review.Domain/Artiview.Review.Domain.csproj", "Review/Artiview.Review.Domain/"]
RUN dotnet restore "Review/Artiview.Review.WebApi/Artiview.Review.WebApi.csproj"
COPY . .
WORKDIR "/src/Review/Artiview.Review.WebApi"
RUN dotnet build "Artiview.Review.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Artiview.Review.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Artiview.Review.WebApi.dll"]