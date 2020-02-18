#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
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

RUN dotnet restore "Aragas.QServer.Core/Aragas.QServer.Core.csproj"

COPY ["MineLib/Libraries/MineLib.Core/MineLib.Core.csproj", "MineLib/Libraries/MineLib.Core/"]
COPY ["MineLib/Libraries/MineLib.Server.Core/MineLib.Server.Core.csproj", "MineLib/Libraries/MineLib.Server.Core/"]
COPY ["MineLib/Libraries/Protocol/MineLib.Protocol/MineLib.Protocol.csproj", "MineLib/Libraries/Protocol/MineLib.Protocol/"]
COPY ["MineLib/Libraries/Protocol/MineLib.Protocol.Netty/MineLib.Protocol.Netty.csproj", "MineLib/Libraries/Protocol/MineLib.Protocol.Netty/"]
COPY ["MineLib/Libraries/Protocol/MineLib.Protocol5/MineLib.Protocol.Netty5.csproj", "MineLib/Libraries/Protocol/MineLib.Protocol5/"]

COPY ["MineLib/Executables/MineLib.Server.Proxy/MineLib.Server.Proxy.csproj", "MineLib/Executables/MineLib.Server.Proxy/"]
RUN dotnet restore "MineLib/Executables/MineLib.Server.Proxy/MineLib.Server.Proxy.csproj"
COPY . .
WORKDIR "/src/MineLib/Executables/MineLib.Server.Proxy"
RUN dotnet build "MineLib.Server.Proxy.csproj" -c Release -o /app/build -p:SolutionDir='/src'

FROM build AS publish
RUN dotnet publish "MineLib.Server.Proxy.csproj" -c Release -o /app/publish -p:SolutionDir='/src'

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get install -y procps
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MineLib.Server.Proxy.dll"]