# 🎬 MovieTracker - Generic Queue Management System

A modern ASP.NET Core Web API for managing generic queues (movies, books, games, etc.) with collaborative features, user management, and comprehensive API endpoints.

## ✨ Features

### 🎯 **Generic Queue System**
- **Flexible Content Types**: Support for movies, books, games, TV shows, and any content type
- **Collaborative Queues**: Multiple users can join and contribute to queues
- **Queue Management**: Create, update, delete, and organize items in queues
- **Status Tracking**: Track item status (Pending, InProgress, Completed, Skipped)
- **Member Roles**: Owner, Admin, and Member roles with different permissions

### 👥 **User Management**
- User registration and authentication
- Profile management
- Password hashing for security
- User activity tracking

### 📚 **Content Management**
- Movie database with detailed information
- Comments and ratings system
- Search and filtering capabilities
- External service integration support (IMDB, etc.)

### 🔧 **Technical Features**
- **PostgreSQL** with **Dapper** for high-performance data access
- **Clean Architecture** with proper separation of concerns
- **Repository Pattern** for data access abstraction
- **Service Layer** for business logic
- **AutoMapper** for object mapping
- **Comprehensive API** with Swagger documentation
- **Unit Testing** with xUnit and Moq

## 🏗️ Architecture

```
MovieTrackerProj/
├── MovieTracker.API/           # Web API controllers
├── MovieTracker.Core/          # Domain entities, interfaces, DTOs
├── MovieTracker.Infrastructure/ # Data access, repositories, services
└── MovieTracker.Tests/         # Unit tests
```

### **Technology Stack**
- **.NET 8** with ASP.NET Core Web API
- **PostgreSQL** database
- **Dapper** micro-ORM for data access
- **AutoMapper** for object mapping
- **xUnit** and **Moq** for testing
- **Swagger/OpenAPI** for API documentation

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- PostgreSQL database
- Your favorite IDE (Visual Studio, VS Code, etc.)

### Database Setup

1. **Install PostgreSQL** (if not already installed)
   - Download from: https://www.postgresql.org/download/
   - Or use Docker: `docker run --name postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres`

2. **Create Database**
   ```sql
   CREATE DATABASE movietracker;
   ```

3. **Run Schema Script**
   ```bash
   psql -h localhost -U postgres -d movietracker -f database_schema.sql
   ```

### Application Setup

1. **Clone/Download the project**
   ```bash
   git clone <repository-url>
   cd MovieTrackerProj
   ```

2. **Update Connection String**
   - Edit `MovieTracker.API/appsettings.json`
   - Update the PostgreSQL connection string if needed:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=movietracker;Username=postgres;Password=postgres;Port=5432"
   }
   ```

3. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

4. **Run the Application**
   ```bash
   cd MovieTracker.API
   dotnet run
   ```

5. **Access the API**
   - API: http://localhost:5000
   - Swagger UI: http://localhost:5000/swagger

## 📋 API Endpoints

### Users
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user

### Movies
- `GET /api/movies` - Get all movies
- `GET /api/movies/{id}` - Get movie by ID
- `POST /api/movies` - Create new movie
- `PUT /api/movies/{id}` - Update movie
- `DELETE /api/movies/{id}` - Delete movie

### Queues (Generic)
- `GET /api/queues` - Get all queues
- `GET /api/queues/public` - Get public queues
- `GET /api/queues/my` - Get user's owned queues
- `GET /api/queues/member` - Get queues where user is member
- `GET /api/queues/{id}` - Get queue by ID
- `POST /api/queues` - Create new queue
- `PUT /api/queues/{id}` - Update queue
- `DELETE /api/queues/{id}` - Delete queue

### Queue Items
- `POST /api/queues/{id}/items` - Add item to queue
- `PUT /api/queues/{id}/items/{itemId}` - Update queue item
- `DELETE /api/queues/{id}/items/{itemId}` - Remove item from queue

### Queue Members
- `POST /api/queues/{id}/members` - Add member to queue
- `DELETE /api/queues/{id}/members/{memberId}` - Remove member from queue
- `PUT /api/queues/{id}/members/{memberId}/role` - Update member role

### Comments
- `GET /api/comments` - Get all comments
- `GET /api/comments/{id}` - Get comment by ID
- `POST /api/comments` - Create new comment
- `PUT /api/comments/{id}` - Update comment
- `DELETE /api/comments/{id}` - Delete comment

### Ratings
- `GET /api/ratings` - Get all ratings
- `GET /api/ratings/{id}` - Get rating by ID
- `POST /api/ratings` - Create new rating
- `PUT /api/ratings/{id}` - Update rating
- `DELETE /api/ratings/{id}` - Delete rating

## 🧪 Testing

Run the unit tests:
```bash
cd MovieTracker.Tests
dotnet test
```

## 📊 Data Models

### Core Entities

**User**
- `Id`, `Username`, `Email`, `PasswordHash`
- `CreatedAt`, `LastLoginAt`
- Navigation to Queues, Comments, Ratings

**Queue** (Generic)
- `Id`, `Name`, `Description`, `IsPublic`
- `OwnerId`, `CreatedAt`, `UpdatedAt`
- Navigation to Owner, Items, Members

**QueueItem** (Generic)
- `Id`, `QueueId`, `Title`, `Description`
- `Type`, `ExternalId`, `ImageUrl`
- `Position`, `Status`, `AddedAt`, `CompletedAt`
- Navigation to Queue, AddedBy

**QueueMember**
- `Id`, `QueueId`, `UserId`, `Role`
- `JoinedAt`
- Navigation to Queue, User

**Movie**
- `Id`, `Title`, `Description`, `Director`
- `Genre`, `ReleaseYear`, `Rating`, `Runtime`
- `PosterUrl`, `TrailerUrl`
- `CreatedAt`, `UpdatedAt`

**Comment**
- `Id`, `UserId`, `MovieId`, `Content`
- `CreatedAt`, `UpdatedAt`
- Navigation to User, Movie

**Rating**
- `Id`, `UserId`, `MovieId`, `Score`
- `Review`, `CreatedAt`, `UpdatedAt`
- Navigation to User, Movie

## 🔄 Generic Queue System

The queue system is designed to be **completely generic** and can handle any type of content:

### Content Types
- **Movies**: Films, documentaries, short films
- **Books**: Novels, non-fiction, comics, manga
- **Games**: Video games, board games, card games
- **TV Shows**: Series, episodes, seasons
- **Music**: Albums, songs, playlists
- **Podcasts**: Episodes, series
- **Any Custom Type**: Define your own content types

### Queue Features
- **Flexible Item Structure**: Each item can have custom properties
- **Status Tracking**: Track progress through items
- **Collaborative**: Multiple users can contribute
- **Public/Private**: Control visibility of queues
- **Role-based Access**: Different permission levels
- **Position Management**: Reorder items as needed

## 🚀 Future Enhancements

### Authentication & Authorization
- JWT token-based authentication
- Role-based authorization
- OAuth integration (Google, Facebook, etc.)

### Advanced Features
- Real-time notifications (SignalR)
- File uploads for images and media
- Email notifications
- Mobile app support
- Advanced search with Elasticsearch
- Caching with Redis
- Background job processing (Hangfire)

### External Integrations
- IMDB API for movie data
- Goodreads API for book data
- Steam API for game data
- Spotify API for music data

### Performance & Scalability
- Database connection pooling
- Query optimization
- Caching strategies
- Load balancing
- Microservices architecture

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Submit a pull request

## 📝 License

This project is licensed under the MIT License.

## 🆘 Support

If you encounter any issues or have questions:
1. Check the API documentation at `/swagger`
2. Review the test cases for usage examples
3. Create an issue in the repository

---

**Built with ❤️ using ASP.NET Core, PostgreSQL, and Dapper** 