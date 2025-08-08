# API Test Guide

## Prerequisites
1. Make sure PostgreSQL is running
2. Create the database and run the schema script
3. Start the API: `dotnet run` in the MovieTracker.API directory

## Database Setup
```sql
-- Create database
CREATE DATABASE movietracker;

-- Run the schema script
psql -h localhost -U postgres -d movietracker -f database_schema.sql
```

## Test API Endpoints

### 1. Test Swagger UI
Open browser and go to: http://localhost:5000/swagger

### 2. Test Users Endpoint
```bash
# Get all users
curl -X GET "http://localhost:5000/api/users"

# Create a user
curl -X POST "http://localhost:5000/api/users" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "password123"
  }'
```

### 3. Test Movies Endpoint
```bash
# Get all movies
curl -X GET "http://localhost:5000/api/movies"

# Create a movie
curl -X POST "http://localhost:5000/api/movies" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test Movie",
    "description": "A test movie",
    "director": "Test Director",
    "genre": "Action",
    "releaseYear": 2024
  }'
```

### 4. Test Queues Endpoint
```bash
# Get all queues
curl -X GET "http://localhost:5000/api/queues"

# Create a queue
curl -X POST "http://localhost:5000/api/queues" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "My Watchlist",
    "description": "Movies I want to watch",
    "isPublic": false
  }'

# Add item to queue
curl -X POST "http://localhost:5000/api/queues/1/items" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "The Shawshank Redemption",
    "description": "Classic prison drama",
    "type": "movie",
    "externalId": "tt0111161"
  }'
```

### 5. Test Comments Endpoint
```bash
# Get all comments
curl -X GET "http://localhost:5000/api/comments"

# Create a comment
curl -X POST "http://localhost:5000/api/comments" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "movieId": 1,
    "content": "Great movie!"
  }'
```

### 6. Test Ratings Endpoint
```bash
# Get all ratings
curl -X GET "http://localhost:5000/api/ratings"

# Create a rating
curl -X POST "http://localhost:5000/api/ratings" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "movieId": 1,
    "score": 9,
    "review": "Excellent film!"
  }'
```

## Expected Results
- All endpoints should return JSON responses
- Swagger UI should be accessible and show all endpoints
- Database operations should work correctly
- No compilation errors should occur

## Troubleshooting
1. If you get connection errors, make sure PostgreSQL is running
2. If you get compilation errors, run `dotnet restore` and `dotnet build`
3. Check the connection string in `appsettings.json`
4. Make sure the database schema has been created 