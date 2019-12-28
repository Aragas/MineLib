#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MineLib/Executables/MineLib.Server.Proxy/MineLib.Server.Proxy.csproj", "MineLib/Executables/MineLib.Server.Proxy/"]
COPY ["MineLib/Libraries/MineLib.Server.Core/MineLib.Server.Core.csproj", "MineLib/Libraries/MineLib.Server.Core/"]
COPY ["MineLib/Libraries/MineLib.Core/MineLib.Core.csproj", "MineLib/Libraries/MineLib.Core/"]
COPY ["Third Party/fNbt/fNbt/fNbt.Standard.csproj", "Third Party/fNbt/fNbt/"]
COPY ["Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/Aragas.Network-2.csproj", "Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/"]
COPY ["Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/Aragas.TupleEventSystem.csproj", "Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/"]
COPY ["Aragas.QServer.Core/Aragas.QServer.Core.csproj", "Aragas.QServer.Core/"]
COPY ["Aragas.QServer.MBus/Aragas.QServer.MBus.csproj", "Aragas.QServer.MBus/"]
COPY ["MineLib/Libraries/Protocol/MineLib.Protocol/MineLib.Protocol.csproj", "MineLib/Libraries/Protocol/MineLib.Protocol/"]
COPY ["MineLib/Libraries/Protocol/MineLib.Protocol5/MineLib.Protocol5.csproj", "MineLib/Libraries/Protocol/MineLib.Protocol5/"]
RUN dotnet restore "MineLib/Executables/MineLib.Server.Proxy/MineLib.Server.Proxy.csproj"
COPY . .
WORKDIR "/src/MineLib/Executables/MineLib.Server.Proxy"
RUN dotnet build "MineLib.Server.Proxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MineLib.Server.Proxy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MineLib.Server.Proxy.dll"]