#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["WebApp.Api/WebApp.Api.csproj", "WebApp.Api/"]
COPY ["WebApp.Application/WebApp.Application.csproj", "WebApp.Application/"]
COPY ["WebApp.Infrastructure/WebApp.Infrastructure.csproj", "WebApp.Infrastructure/"]
COPY ["WebApp.Data/WebApp.Data.csproj", "WebApp.Data/"]
RUN dotnet restore "WebApp.Api/WebApp.Api.csproj"
COPY . .
WORKDIR "/src/WebApp.Api"
RUN dotnet build "WebApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp.Api.dll"]