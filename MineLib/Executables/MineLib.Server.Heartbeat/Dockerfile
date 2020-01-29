#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MineLib/Executables/MineLib.Server.Heartbeat/MineLib.Server.Heartbeat.csproj", "MineLib/Executables/MineLib.Server.Heartbeat/"]
RUN dotnet restore "MineLib/Executables/MineLib.Server.Heartbeat/MineLib.Server.Heartbeat.csproj"
COPY . .
WORKDIR "/src/MineLib/Executables/MineLib.Server.Heartbeat"
RUN dotnet build "MineLib.Server.Heartbeat.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MineLib.Server.Heartbeat.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MineLib.Server.Heartbeat.dll"]