# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["MovieTracker.API/MovieTracker.API.csproj", "MovieTracker.API/"]
COPY ["MovieTracker.Core/MovieTracker.Core.csproj", "MovieTracker.Core/"]
COPY ["MovieTracker.Infrastructure/MovieTracker.Infrastructure.csproj", "MovieTracker.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "MovieTracker.API/MovieTracker.API.csproj"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/MovieTracker.API"
RUN dotnet build "MovieTracker.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "MovieTracker.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "MovieTracker.API.dll"]
