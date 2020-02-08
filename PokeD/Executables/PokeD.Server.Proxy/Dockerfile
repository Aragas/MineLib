#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PokeD/Executables/PokeD.Server.Proxy/PokeD.Server.Proxy.csproj", "PokeD/Executables/PokeD.Server.Proxy/"]
COPY ["PokeD/Libraries/PokeD.Server.Core/PokeD.Server.Core.csproj", "PokeD/Libraries/PokeD.Server.Core/"]
COPY ["PokeD/Libraries/PokeD.Core/PokeD.Core/PokeD.Core.csproj", "PokeD/Libraries/PokeD.Core/PokeD.Core/"]
COPY ["Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/Aragas.Network-2.csproj", "Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/"]
COPY ["PokeD/Libraries/PokeD.BattleEngine/PokeD.BattleEngine.csproj", "PokeD/Libraries/PokeD.BattleEngine/"]
COPY ["Aragas.QServer.Core/Aragas.QServer.Core.csproj", "Aragas.QServer.Core/"]
COPY ["Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/Aragas.TupleEventSystem.csproj", "Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/"]
COPY ["Aragas.QServer.Metrics/Aragas.QServer.Metrics.csproj", "Aragas.QServer.Metrics/"]
COPY ["Aragas.QServer.Logging/Aragas.QServer.Logging.csproj", "Aragas.QServer.Logging/"]
COPY ["Aragas.QServer.NetworkBus/Aragas.QServer.NetworkBus.csproj", "Aragas.QServer.NetworkBus/"]
RUN dotnet restore "PokeD/Executables/PokeD.Server.Proxy/PokeD.Server.Proxy.csproj"
COPY . .
WORKDIR "/src/PokeD/Executables/PokeD.Server.Proxy"
RUN dotnet build "PokeD.Server.Proxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PokeD.Server.Proxy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokeD.Server.Proxy.dll"]