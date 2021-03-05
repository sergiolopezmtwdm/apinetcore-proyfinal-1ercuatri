FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ApiDigitalGamesMx/ApiDigitalGamesMx.csproj", "ApiDigitalGamesMx/"]
RUN dotnet restore "ApiDigitalGamesMx/ApiDigitalGamesMx.csproj"
COPY . .
RUN dotnet build "ApiDigitalGamesMx.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiDigitalGamesMx.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiDigitalGamesMx.dll"]
