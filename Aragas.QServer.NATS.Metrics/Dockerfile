#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Aragas.QServer.NATS.Metrics/Aragas.QServer.NATS.Metrics.csproj", "Aragas.QServer.NATS.Metrics/"]
COPY ["Aragas.QServer.Core/Aragas.QServer.Core.csproj", "Aragas.QServer.Core/"]
COPY ["Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/Aragas.Network-2.csproj", "Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/"]
COPY ["Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/Aragas.TupleEventSystem.csproj", "Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/"]
RUN dotnet restore "Aragas.QServer.NATS.Metrics/Aragas.QServer.NATS.Metrics.csproj"
COPY . .
WORKDIR "/src/Aragas.QServer.NATS.Metrics"
RUN dotnet build "Aragas.QServer.NATS.Metrics.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Aragas.QServer.NATS.Metrics.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aragas.QServer.NATS.Metrics.dll"]