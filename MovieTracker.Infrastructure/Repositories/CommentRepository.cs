using Dapper;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Infrastructure.Data;
using System.Data;

namespace MovieTracker.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public CommentRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Comment>(
                "SELECT * FROM comments WHERE id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<Comment>("SELECT * FROM comments ORDER BY created_at");
        }

        public async Task<IEnumerable<Comment>> GetByMovieIdAsync(int movieId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<Comment>(
                "SELECT * FROM comments WHERE movie_id = @MovieId ORDER BY created_at", 
                new { MovieId = movieId });
        }

        public async Task<IEnumerable<Comment>> GetByUserIdAsync(int userId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<Comment>(
                "SELECT * FROM comments WHERE user_id = @UserId ORDER BY created_at", 
                new { UserId = userId });
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                INSERT INTO comments (user_id, movie_id, content, created_at)
                VALUES (@UserId, @MovieId, @Content, @CreatedAt)
                RETURNING *";
            
            return await connection.QueryFirstAsync<Comment>(sql, comment);
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                UPDATE comments 
                SET content = @Content, updated_at = @UpdatedAt
                WHERE id = @Id
                RETURNING *";
            
            return await connection.QueryFirstAsync<Comment>(sql, comment);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            await connection.ExecuteAsync("DELETE FROM comments WHERE id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM comments WHERE id = @Id", new { Id = id });
            return count > 0;
        }

        public async Task<bool> IsOwnerAsync(int commentId, int userId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM comments WHERE id = @CommentId AND user_id = @UserId", 
                new { CommentId = commentId, UserId = userId });
            return count > 0;
        }
    }
} 