#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used for VS debugging on Docker
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ENV ASPNETCORE_URLS=https://+:5001;http://+:5000
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/CrudTest.Presentation.Server/CrudTest.Presentation.Server.csproj", "src/CrudTest.Presentation.Server/"]
COPY ["src/CrudTest.Application/CrudTest.Application.csproj", "src/CrudTest.Application/"]
COPY ["src/CrudTest.Domain/CrudTest.Domain.csproj", "src/CrudTest.Domain/"]
COPY ["src/CrudTest.Infrastructure/CrudTest.Infrastructure.csproj", "src/CrudTest.Infrastructure/"]
RUN dotnet restore "src/CrudTest.Presentation.Server/CrudTest.Presentation.Server.csproj"
COPY . .
WORKDIR "/src/src/CrudTest.Presentation.Server"
RUN dotnet build "CrudTest.Presentation.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CrudTest.Presentation.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CrudTest.Presentation.Server.dll"]