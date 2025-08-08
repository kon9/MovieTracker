using AutoMapper;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Core.Services;

namespace MovieTracker.Infrastructure.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RatingService(
            IRatingRepository ratingRepository,
            IMovieRepository movieRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _movieRepository = movieRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<RatingDto?> GetByIdAsync(int id)
        {
            var rating = await _ratingRepository.GetByIdAsync(id);
            return _mapper.Map<RatingDto>(rating);
        }

        public async Task<IEnumerable<RatingDto>> GetAllAsync()
        {
            var ratings = await _ratingRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<RatingDto?> GetByUserAndMovieAsync(int userId, int movieId)
        {
            var rating = await _ratingRepository.GetByUserAndMovieAsync(userId, movieId);
            return _mapper.Map<RatingDto>(rating);
        }

        public async Task<IEnumerable<RatingDto>> GetByMovieIdAsync(int movieId)
        {
            var ratings = await _ratingRepository.GetByMovieIdAsync(movieId);
            return _mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<IEnumerable<RatingDto>> GetByUserIdAsync(int userId)
        {
            var ratings = await _ratingRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<double> GetAverageRatingAsync(int movieId)
        {
            return await _ratingRepository.GetAverageRatingAsync(movieId);
        }

        public async Task<int> GetTotalRatingsAsync(int movieId)
        {
            return await _ratingRepository.GetTotalRatingsAsync(movieId);
        }

        public async Task<RatingDto> CreateAsync(int userId, CreateRatingDto createRatingDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var movie = await _movieRepository.GetByIdAsync(createRatingDto.MovieId);
            if (movie == null)
                throw new InvalidOperationException("Movie not found");

            if (await _ratingRepository.ExistsByUserAndMovieAsync(userId, createRatingDto.MovieId))
                throw new InvalidOperationException("You have already rated this movie");

            if (createRatingDto.Score < 1 || createRatingDto.Score > 10)
                throw new InvalidOperationException("Rating must be between 1 and 10");

            var rating = new Rating
            {
                UserId = userId,
                MovieId = createRatingDto.MovieId,
                Score = createRatingDto.Score,
                Review = createRatingDto.Review,
                CreatedAt = DateTime.UtcNow
            };

            var createdRating = await _ratingRepository.CreateAsync(rating);
            return _mapper.Map<RatingDto>(createdRating);
        }

        public async Task<RatingDto> UpdateAsync(int userId, int movieId, UpdateRatingDto updateRatingDto)
        {
            var rating = await _ratingRepository.GetByUserAndMovieAsync(userId, movieId);
            if (rating == null)
                throw new InvalidOperationException("Rating not found");

            if (updateRatingDto.Score < 1 || updateRatingDto.Score > 10)
                throw new InvalidOperationException("Rating must be between 1 and 10");

            rating.Score = updateRatingDto.Score;
            rating.Review = updateRatingDto.Review;
            rating.UpdatedAt = DateTime.UtcNow;

            var updatedRating = await _ratingRepository.UpdateAsync(rating);
            return _mapper.Map<RatingDto>(updatedRating);
        }

        public async Task DeleteAsync(int userId, int movieId)
        {
            var rating = await _ratingRepository.GetByUserAndMovieAsync(userId, movieId);
            if (rating == null)
                throw new InvalidOperationException("Rating not found");

            await _ratingRepository.DeleteAsync(rating.Id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _ratingRepository.ExistsAsync(id);
        }

        public async Task<bool> ExistsByUserAndMovieAsync(int userId, int movieId)
        {
            return await _ratingRepository.ExistsByUserAndMovieAsync(userId, movieId);
        }
    }
} 