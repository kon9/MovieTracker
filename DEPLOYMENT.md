# MovieTracker Deployment Guide

This guide provides instructions for deploying the MovieTracker application to production.

## Architecture

The application consists of three main components:
- **Frontend**: React application served by Nginx
- **API**: ASP.NET Core Web API
- **Database**: PostgreSQL database

## Prerequisites

- Docker and Docker Compose
- PostgreSQL (if running outside Docker)
- Node.js 18+ (for local development)
- .NET 9.0 SDK (for local development)

## Production Deployment with Docker

### 1. Clone the Repository

```bash
git clone <repository-url>
cd MovieTracker
```

### 2. Environment Configuration

Create environment-specific configuration files:

#### API Configuration
Update `MovieTracker.API/appsettings.Production.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Database=movietracker;Username=movietracker_user;Password=your_secure_password"
  }
}
```

#### Frontend Configuration
Create `MovieTracker.Frontend/.env.production`:
```env
VITE_API_BASE_URL=http://your-domain.com/api
```

### 3. Deploy with Docker Compose

```bash
# Build and start all services
docker-compose up -d --build

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

### 4. Database Migration

The application will automatically run database migrations on startup. The migration system will:
- Create the database if it doesn't exist
- Apply all pending migrations
- Insert sample data (only on first run)

### 5. Verify Deployment

- Frontend: http://localhost (or your domain)
- API: http://localhost:5000/health
- API Documentation: http://localhost:5000/swagger

## Manual Deployment

### Database Setup

1. Install PostgreSQL
2. Create database and user:
```sql
CREATE DATABASE movietracker;
CREATE USER movietracker_user WITH PASSWORD 'your_password';
GRANT ALL PRIVILEGES ON DATABASE movietracker TO movietracker_user;
```

### API Deployment

1. Build the application:
```bash
cd MovieTracker.API
dotnet publish -c Release -o publish
```

2. Configure connection string in `appsettings.Production.json`

3. Run the application:
```bash
cd publish
dotnet MovieTracker.API.dll
```

### Frontend Deployment

1. Install dependencies and build:
```bash
cd MovieTracker.Frontend
npm install
npm run build
```

2. Serve the `dist` folder using a web server (Nginx, Apache, etc.)

## Configuration

### Environment Variables

#### API
- `ASPNETCORE_ENVIRONMENT`: Set to "Production"
- `ConnectionStrings__DefaultConnection`: PostgreSQL connection string

#### Frontend
- `VITE_API_BASE_URL`: API base URL

### Database Configuration

Update the connection string in:
- `MovieTracker.API/appsettings.Production.json`
- Docker Compose environment variables

### CORS Configuration

Update the CORS policy in `Program.cs` to include your production domains:
```csharp
policy.WithOrigins("https://yourdomain.com", "https://www.yourdomain.com")
```

## Security Considerations

1. **Database Security**:
   - Use strong passwords
   - Restrict database access to application only
   - Enable SSL/TLS for database connections

2. **API Security**:
   - Use HTTPS in production
   - Implement proper authentication/authorization
   - Configure security headers

3. **Frontend Security**:
   - Serve over HTTPS
   - Configure CSP headers
   - Validate all user inputs

## Monitoring and Logging

### Health Checks

The API includes health check endpoints:
- `/health`: Basic health check
- Database connectivity is verified during startup

### Logging

Logs are configured in `appsettings.json`:
- Console logging for development
- File logging recommended for production
- Consider centralized logging (ELK stack, Azure Monitor, etc.)

### Monitoring

Consider implementing:
- Application Performance Monitoring (APM)
- Database monitoring
- Infrastructure monitoring
- Uptime monitoring

## Backup and Recovery

### Database Backup

```bash
# Create backup
docker exec movietracker-postgres pg_dump -U movietracker_user movietracker > backup.sql

# Restore backup
docker exec -i movietracker-postgres psql -U movietracker_user movietracker < backup.sql
```

### File System Backup

Backup the Docker volumes:
```bash
docker run --rm -v movietracker_postgres_data:/data -v $(pwd):/backup alpine tar czf /backup/postgres-data.tar.gz /data
```

## Scaling Considerations

1. **Database**: Consider read replicas for high-read workloads
2. **API**: Can be horizontally scaled behind a load balancer
3. **Frontend**: Use CDN for static assets
4. **Caching**: Implement Redis for API caching

## Troubleshooting

### Common Issues

1. **Database Connection Issues**:
   - Check connection string
   - Verify PostgreSQL is running
   - Check network connectivity

2. **Migration Failures**:
   - Check database permissions
   - Verify migration files
   - Check logs for specific errors

3. **Frontend API Connection**:
   - Verify CORS configuration
   - Check API base URL configuration
   - Ensure API is accessible from frontend

### Useful Commands

```bash
# View API logs
docker-compose logs api

# View database logs
docker-compose logs postgres

# Access database shell
docker exec -it movietracker-postgres psql -U movietracker_user movietracker

# Rebuild specific service
docker-compose up -d --build api
```

## Performance Optimization

1. **Database**:
   - Implement proper indexing
   - Use connection pooling
   - Optimize queries

2. **API**:
   - Implement caching
   - Use async/await patterns
   - Optimize AutoMapper configurations

3. **Frontend**:
   - Code splitting
   - Lazy loading
   - Image optimization
   - Bundle optimization

## Support

For issues and questions:
1. Check the logs first
2. Review this deployment guide
3. Check the application documentation
4. Contact the development team
