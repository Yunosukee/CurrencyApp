FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CurrencyBackend.csproj", "./"]
RUN dotnet restore "./CurrencyBackend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CurrencyBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurrencyBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyBackend.dll"]