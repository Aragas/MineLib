#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PokeD/Executables/Bus/PokeD.Server.WorldBus/PokeD.Server.WorldBus.csproj", "PokeD/Executables/Bus/PokeD.Server.WorldBus/"]
COPY ["Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/Aragas.TupleEventSystem.csproj", "Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/"]
COPY ["Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/Aragas.Network-2.csproj", "Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/"]
COPY ["PokeD/Libraries/PokeD.Server.Core/PokeD.Server.Core.csproj", "PokeD/Libraries/PokeD.Server.Core/"]
COPY ["C:/Users/Aragas/source/repos/PokeD.Server.NetCore/PokeD.Core/PokeD.Core/PokeD.Core.csproj", "C:/Users/Aragas/source/repos/PokeD.Server.NetCore/PokeD.Core/PokeD.Core/"]
COPY ["C:/Users/Aragas/source/repos/PokeD.Server.NetCore/PokeD.BattleEngine/PokeD.BattleEngine.csproj", "C:/Users/Aragas/source/repos/PokeD.Server.NetCore/PokeD.BattleEngine/"]
COPY ["Aragas.QServer.Core/Aragas.QServer.Core.csproj", "Aragas.QServer.Core/"]
RUN dotnet restore "PokeD/Executables/Bus/PokeD.Server.WorldBus/PokeD.Server.WorldBus.csproj"
COPY . .
WORKDIR "/src/PokeD/Executables/Bus/PokeD.Server.WorldBus"
RUN dotnet build "PokeD.Server.WorldBus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PokeD.Server.WorldBus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokeD.Server.WorldBus.dll"]