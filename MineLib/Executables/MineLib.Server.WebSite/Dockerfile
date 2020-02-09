#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY ["Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/Aragas.TupleEventSystem.csproj", "Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/"]
COPY ["Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/Aragas.Network-2.csproj", "Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/"]
COPY ["Aragas.QServer.Hosting/Aragas.QServer.Hosting.csproj", "Aragas.QServer.Hosting/"]
COPY ["Aragas.QServer.Logging/Aragas.QServer.Logging.csproj", "Aragas.QServer.Logging/"]
COPY ["Aragas.QServer.Metrics/Aragas.QServer.Metrics.csproj", "Aragas.QServer.Metrics/"]
COPY ["Aragas.QServer.NetworkBus/Aragas.QServer.NetworkBus.csproj", "Aragas.QServer.NetworkBus/"]
COPY ["Aragas.QServer.Core/Aragas.QServer.Core.csproj", "Aragas.QServer.Core/"]
COPY ["Aragas.QServer.Metrics.PostgreSQL/Aragas.QServer.Metrics.PostgreSQL.csproj", "Aragas.QServer.Metrics.PostgreSQL/"]

COPY ["MineLib/Executables/MineLib.Server.WebSite/MineLib.Server.WebSite.csproj", "MineLib/Executables/MineLib.Server.WebSite/"]

RUN dotnet restore "MineLib/Executables/MineLib.Server.WebSite/MineLib.Server.WebSite.csproj"
COPY . .
WORKDIR "/src/MineLib/Executables/MineLib.Server.WebSite"
RUN dotnet build "MineLib.Server.WebSite.csproj" -c Release -o /app/build -p:SolutionDir='/src'

FROM build AS publish
RUN dotnet publish "MineLib.Server.WebSite.csproj" -c Release -o /app/publish -p:SolutionDir='/src'

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MineLib.Server.WebSite.dll"]