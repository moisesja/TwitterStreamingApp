#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Console/TwitterStreamingConsole.csproj", "Console/"]
COPY ["Lib/TwitterStreamingLib.csproj", "Lib/"]
RUN dotnet restore "Console/TwitterStreamingConsole.csproj"
COPY . .
WORKDIR "/src/Console"
RUN dotnet build "TwitterStreamingConsole.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TwitterStreamingConsole.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TwitterStreamingConsole.dll"]
