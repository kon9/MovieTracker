using AutoMapper;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Core.Services;

namespace MovieTracker.Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CommentService(
            ICommentRepository commentRepository,
            IMovieRepository movieRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _movieRepository = movieRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<CommentDto?> GetByIdAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<IEnumerable<CommentDto>> GetByMovieIdAsync(int movieId)
        {
            var comments = await _commentRepository.GetByMovieIdAsync(movieId);
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<IEnumerable<CommentDto>> GetAllAsync()
        {
            var comments = await _commentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<IEnumerable<CommentDto>> GetByUserIdAsync(int userId)
        {
            var comments = await _commentRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<CommentDto> CreateAsync(int userId, CreateCommentDto createCommentDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var movie = await _movieRepository.GetByIdAsync(createCommentDto.MovieId);
            if (movie == null)
                throw new InvalidOperationException("Movie not found");

            var comment = new Comment
            {
                UserId = userId,
                MovieId = createCommentDto.MovieId,
                Content = createCommentDto.Content,
                CreatedAt = DateTime.UtcNow
            };

            var createdComment = await _commentRepository.CreateAsync(comment);
            return _mapper.Map<CommentDto>(createdComment);
        }

        public async Task<CommentDto> UpdateAsync(int id, int userId, UpdateCommentDto updateCommentDto)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
                throw new InvalidOperationException("Comment not found");

            if (!await _commentRepository.IsOwnerAsync(id, userId))
                throw new InvalidOperationException("You can only update your own comments");

            comment.Content = updateCommentDto.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            var updatedComment = await _commentRepository.UpdateAsync(comment);
            return _mapper.Map<CommentDto>(updatedComment);
        }

        public async Task DeleteAsync(int id, int userId)
        {
            if (!await _commentRepository.ExistsAsync(id))
                throw new InvalidOperationException("Comment not found");

            if (!await _commentRepository.IsOwnerAsync(id, userId))
                throw new InvalidOperationException("You can only delete your own comments");

            await _commentRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _commentRepository.ExistsAsync(id);
        }

        public async Task<bool> IsOwnerAsync(int commentId, int userId)
        {
            return await _commentRepository.IsOwnerAsync(commentId, userId);
        }
    }
} 