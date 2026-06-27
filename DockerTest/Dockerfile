# 1. Dasturni ishga tushirish uchun asosiy .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# 2. Loyihani build qilish uchun .NET 8 SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Csproj faylini nusxalash va kutubxonalarni yuklab olish (restore)
COPY ["src/FirstDockerProj/FirstDockerProj.csproj", "src/FirstDockerProj/"]
RUN dotnet restore "./src/FirstDockerProj/FirstDockerProj.csproj"

# Qolgan barcha fayllarni nusxalash va build qilish
COPY . .
WORKDIR "/src/src/FirstDockerProj"
RUN dotnet build "./FirstDockerProj.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Dasturni publish qilish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FirstDockerProj.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Yakuniy image'ni yig'ish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FirstDockerProj.dll"]