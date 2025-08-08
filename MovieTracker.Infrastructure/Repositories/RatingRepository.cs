using Dapper;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Infrastructure.Data;
using System.Data;

namespace MovieTracker.Infrastructure.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public RatingRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Rating?> GetByIdAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Rating>(
                "SELECT * FROM ratings WHERE id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Rating>> GetAllAsync()
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<Rating>("SELECT * FROM ratings ORDER BY created_at");
        }

        public async Task<IEnumerable<Rating>> GetByMovieIdAsync(int movieId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<Rating>(
                "SELECT * FROM ratings WHERE movie_id = @MovieId ORDER BY created_at", 
                new { MovieId = movieId });
        }

        public async Task<IEnumerable<Rating>> GetByUserIdAsync(int userId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<Rating>(
                "SELECT * FROM ratings WHERE user_id = @UserId ORDER BY created_at", 
                new { UserId = userId });
        }

        public async Task<Rating?> GetByUserAndMovieAsync(int userId, int movieId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Rating>(
                "SELECT * FROM ratings WHERE user_id = @UserId AND movie_id = @MovieId", 
                new { UserId = userId, MovieId = movieId });
        }

        public async Task<double> GetAverageRatingAsync(int movieId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var result = await connection.QueryFirstOrDefaultAsync<double?>(
                "SELECT AVG(score) FROM ratings WHERE movie_id = @MovieId", 
                new { MovieId = movieId });
            return result ?? 0.0;
        }

        public async Task<int> GetTotalRatingsAsync(int movieId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM ratings WHERE movie_id = @MovieId", 
                new { MovieId = movieId });
        }

        public async Task<Rating> CreateAsync(Rating rating)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                INSERT INTO ratings (user_id, movie_id, score, review, created_at)
                VALUES (@UserId, @MovieId, @Score, @Review, @CreatedAt)
                RETURNING *";
            
            return await connection.QueryFirstAsync<Rating>(sql, rating);
        }

        public async Task<Rating> UpdateAsync(Rating rating)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                UPDATE ratings 
                SET score = @Score, review = @Review, updated_at = @UpdatedAt
                WHERE id = @Id
                RETURNING *";
            
            return await connection.QueryFirstAsync<Rating>(sql, rating);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            await connection.ExecuteAsync("DELETE FROM ratings WHERE id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM ratings WHERE id = @Id", new { Id = id });
            return count > 0;
        }

        public async Task<bool> ExistsByUserAndMovieAsync(int userId, int movieId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM ratings WHERE user_id = @UserId AND movie_id = @MovieId", 
                new { UserId = userId, MovieId = movieId });
            return count > 0;
        }
    }
} 