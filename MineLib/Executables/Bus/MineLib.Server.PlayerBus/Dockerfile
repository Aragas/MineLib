#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine AS base
RUN apk add --no-cache procps
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["Aragas.QServer.Core/Aragas.QServer.Core.csproj", "Aragas.QServer.Core/"]
COPY ["MineLib/Executables/Bus/MineLib.Server.PlayerBus/MineLib.Server.PlayerBus.csproj", "MineLib/Executables/Bus/MineLib.Server.PlayerBus/"]
COPY ["MineLib/Libraries/Protocol/MineLib.Protocol/MineLib.Protocol.csproj", "MineLib/Libraries/Protocol/MineLib.Protocol/"]
COPY ["MineLib/Libraries/Protocol/MineLib.Protocol5/MineLib.Protocol5.csproj", "MineLib/Libraries/Protocol/MineLib.Protocol5/"]
COPY ["MineLib/Libraries/MineLib.Server.Core/MineLib.Server.Core.csproj", "MineLib/Libraries/MineLib.Server.Core/"]
COPY ["MineLib/Libraries/MineLib.Core/MineLib.Core.csproj", "MineLib/Libraries/MineLib.Core/"]
COPY ["Third Party/NiclasOlofsson-fNbt/fNbt/NiclasOlofsson-fNbt.csproj", "Third Party/NiclasOlofsson-fNbt/fNbt/"]
COPY ["Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/Aragas.Network-2.csproj", "Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/"]
COPY ["Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/Aragas.TupleEventSystem.csproj", "Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/"]
RUN dotnet restore "MineLib/Executables/Bus/MineLib.Server.PlayerBus/MineLib.Server.PlayerBus.csproj"
COPY . .
WORKDIR "/src/MineLib/Executables/Bus/MineLib.Server.PlayerBus"
RUN dotnet build "MineLib.Server.PlayerBus.csproj" -c Release -o /app/build -p:SolutionDir='/src'

FROM build AS publish
RUN dotnet publish "MineLib.Server.PlayerBus.csproj" -c Release -o /app/publish -p:SolutionDir='/src'

FROM base AS final

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
RUN apk add --no-cache icu-libs

ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MineLib.Server.PlayerBus.dll"]