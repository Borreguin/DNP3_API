FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 7070
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["DNP3_API.csproj", ""]
RUN dotnet restore "./DNP3_API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DNP3_API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DNP3_API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DNP3_API.dll"]