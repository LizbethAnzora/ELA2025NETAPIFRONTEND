FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5031

ENV ASPNETCORE_URLS=http://+:5031

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Reclutamiento.WebApp/Reclutamiento.WebApp.csproj", "Reclutamiento.WebApp/"]
RUN dotnet restore "Reclutamiento.WebApp/Reclutamiento.WebApp.csproj"
COPY . .
WORKDIR "/src/Reclutamiento.WebApp"
RUN dotnet build "Reclutamiento.WebApp.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Reclutamiento.WebApp.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Reclutamiento.WebApp.dll"]
