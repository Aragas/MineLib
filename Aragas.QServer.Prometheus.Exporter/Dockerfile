#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
RUN apk add --no-cache procps
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["Aragas.QServer.Prometheus.Exporter/Aragas.QServer.Prometheus.Exporter.csproj", "Aragas.QServer.Prometheus.Exporter/"]
COPY ["Aragas.QServer.Core/Aragas.QServer.Core.csproj", "Aragas.QServer.Core/"]
COPY ["Aragas.QServer.Metrics/Aragas.QServer.Metrics.csproj", "Aragas.QServer.Metrics/"]
COPY ["Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/Aragas.Network-2.csproj", "Third Party/Aragas.Network-2/src/Aragas.Network-2.Standard/"]
COPY ["Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/Aragas.TupleEventSystem.csproj", "Third Party/Aragas.TupleEventSystem/src/Aragas.TupleEventSystem/"]
RUN dotnet restore "Aragas.QServer.Prometheus.Exporter/Aragas.QServer.Prometheus.Exporter.csproj"
COPY . .
WORKDIR "/src/Aragas.QServer.Prometheus.Exporter"
RUN dotnet build "Aragas.QServer.Prometheus.Exporter.csproj" -c Release -o /app/build -p:SolutionDir='/src'

FROM build AS publish
RUN dotnet publish "Aragas.QServer.Prometheus.Exporter.csproj" -c Release -o /app/publish -p:SolutionDir='/src'

FROM base AS final
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
RUN apk add --no-cache icu-libs

ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aragas.QServer.Prometheus.Exporter.dll"]