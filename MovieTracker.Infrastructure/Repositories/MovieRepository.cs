using Dapper;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Infrastructure.Data;
using System.Data;

namespace MovieTracker.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public MovieRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Movie>(
                "SELECT * FROM movies WHERE id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<Movie>("SELECT * FROM movies ORDER BY created_at");
        }

        public async Task<IEnumerable<Movie>> SearchAsync(string searchTerm)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                SELECT * FROM movies 
                WHERE title ILIKE @SearchTerm OR 
                      director ILIKE @SearchTerm OR 
                      description ILIKE @SearchTerm
                ORDER BY created_at";
            
            return await connection.QueryAsync<Movie>(sql, new { SearchTerm = $"%{searchTerm}%" });
        }

        public async Task<IEnumerable<Movie>> GetByGenreAsync(string genre)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<Movie>(
                "SELECT * FROM movies WHERE genre = @Genre ORDER BY created_at", 
                new { Genre = genre });
        }

        public async Task<IEnumerable<Movie>> GetByYearAsync(int year)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<Movie>(
                "SELECT * FROM movies WHERE release_year = @Year ORDER BY created_at", 
                new { Year = year });
        }

        public async Task<Movie> CreateAsync(Movie movie)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                INSERT INTO movies (title, description, director, genre, release_year, 
                                  rating, runtime, poster_url, trailer_url, created_at)
                VALUES (@Title, @Description, @Director, @Genre, @ReleaseYear, 
                       @Rating, @Runtime, @PosterUrl, @TrailerUrl, @CreatedAt)
                RETURNING *";
            
            return await connection.QueryFirstAsync<Movie>(sql, movie);
        }

        public async Task<Movie> UpdateAsync(Movie movie)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                UPDATE movies 
                SET title = @Title, description = @Description, director = @Director, 
                    genre = @Genre, release_year = @ReleaseYear, rating = @Rating, 
                    runtime = @Runtime, poster_url = @PosterUrl, trailer_url = @TrailerUrl, 
                    updated_at = @UpdatedAt
                WHERE id = @Id
                RETURNING *";
            
            return await connection.QueryFirstAsync<Movie>(sql, movie);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            await connection.ExecuteAsync("DELETE FROM movies WHERE id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM movies WHERE id = @Id", new { Id = id });
            return count > 0;
        }

        public async Task<bool> ExistsByTitleAsync(string title)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM movies WHERE title = @Title", new { Title = title });
            return count > 0;
        }
    }
} 