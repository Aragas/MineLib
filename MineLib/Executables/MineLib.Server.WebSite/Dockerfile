#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
RUN apk add --no-cache procps
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["MineLib/Executables/MineLib.Server.WebSite/MineLib.Server.WebSite.csproj", "MineLib/Executables/MineLib.Server.WebSite/"]
COPY ["Aragas.QServer.Metrics/Aragas.QServer.Metrics.csproj", "Aragas.QServer.Metrics/"]
COPY ["Aragas.QServer.Metrics.PostgreSQL/Aragas.QServer.Metrics.PostgreSQL.csproj", "Aragas.QServer.Metrics.PostgreSQL/"]
COPY ["Aragas.QServer.Logging/Aragas.QServer.Logging.csproj", "Aragas.QServer.Logging/"]
COPY ["Aragas.QServer.NetworkBus/Aragas.QServer.NetworkBus.csproj", "Aragas.QServer.NetworkBus/"]
RUN dotnet restore "MineLib/Executables/MineLib.Server.WebSite/MineLib.Server.WebSite.csproj"
COPY . .
WORKDIR "/src/MineLib/Executables/MineLib.Server.WebSite"
RUN dotnet build "MineLib.Server.WebSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MineLib.Server.WebSite.csproj" -c Release -o /app/publish

FROM base AS final
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
RUN apk add --no-cache icu-libs

ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MineLib.Server.WebSite.dll"]