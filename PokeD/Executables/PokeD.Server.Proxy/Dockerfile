#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PokeD/Executables/PokeD.Server.Proxy/PokeD.Server.Proxy.csproj", "PokeD/Executables/PokeD.Server.Proxy/"]
COPY ["C:/Users/Aragas/source/repos/PokeD.Server.NetCore/PokeD.Core/PokeD.Core/PokeD.Core.csproj", "C:/Users/Aragas/source/repos/PokeD.Server.NetCore/PokeD.Core/PokeD.Core/"]
COPY ["Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/Aragas.Network-2.csproj", "Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/"]
COPY ["C:/Users/Aragas/source/repos/PokeD.Server.NetCore/PokeD.BattleEngine/PokeD.BattleEngine.csproj", "C:/Users/Aragas/source/repos/PokeD.Server.NetCore/PokeD.BattleEngine/"]
COPY ["Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/Aragas.TupleEventSystem.csproj", "Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/"]
COPY ["PokeD/Libraries/PokeD.Server.Core/PokeD.Server.Core.csproj", "PokeD/Libraries/PokeD.Server.Core/"]
COPY ["Aragas.QServer.Core/Aragas.QServer.Core.csproj", "Aragas.QServer.Core/"]
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