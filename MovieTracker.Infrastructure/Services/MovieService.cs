using AutoMapper;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Core.Services;

namespace MovieTracker.Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public MovieService(IMovieRepository movieRepository, IRatingRepository ratingRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<MovieDto?> GetByIdAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
                return null;

            var movieDto = _mapper.Map<MovieDto>(movie);
            movieDto.AverageRating = await _ratingRepository.GetAverageRatingAsync(id);
            movieDto.TotalRatings = await _ratingRepository.GetTotalRatingsAsync(id);
            return movieDto;
        }

        public async Task<IEnumerable<MovieDto>> GetAllAsync()
        {
            var movies = await _movieRepository.GetAllAsync();
            var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movies);

            foreach (var movieDto in movieDtos)
            {
                movieDto.AverageRating = await _ratingRepository.GetAverageRatingAsync(movieDto.Id);
                movieDto.TotalRatings = await _ratingRepository.GetTotalRatingsAsync(movieDto.Id);
            }

            return movieDtos;
        }

        public async Task<IEnumerable<MovieDto>> SearchAsync(string searchTerm)
        {
            var movies = await _movieRepository.SearchAsync(searchTerm);
            var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movies);

            foreach (var movieDto in movieDtos)
            {
                movieDto.AverageRating = await _ratingRepository.GetAverageRatingAsync(movieDto.Id);
                movieDto.TotalRatings = await _ratingRepository.GetTotalRatingsAsync(movieDto.Id);
            }

            return movieDtos;
        }

        public async Task<IEnumerable<MovieDto>> GetByGenreAsync(string genre)
        {
            var movies = await _movieRepository.GetByGenreAsync(genre);
            var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movies);

            foreach (var movieDto in movieDtos)
            {
                movieDto.AverageRating = await _ratingRepository.GetAverageRatingAsync(movieDto.Id);
                movieDto.TotalRatings = await _ratingRepository.GetTotalRatingsAsync(movieDto.Id);
            }

            return movieDtos;
        }

        public async Task<IEnumerable<MovieDto>> GetByYearAsync(int year)
        {
            var movies = await _movieRepository.GetByYearAsync(year);
            var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movies);

            foreach (var movieDto in movieDtos)
            {
                movieDto.AverageRating = await _ratingRepository.GetAverageRatingAsync(movieDto.Id);
                movieDto.TotalRatings = await _ratingRepository.GetTotalRatingsAsync(movieDto.Id);
            }

            return movieDtos;
        }

        public async Task<MovieDto> CreateAsync(CreateMovieDto createMovieDto)
        {
            if (await _movieRepository.ExistsByTitleAsync(createMovieDto.Title))
                throw new InvalidOperationException("Movie with this title already exists");

            var movie = _mapper.Map<Movie>(createMovieDto);
            movie.CreatedAt = DateTime.UtcNow;

            var createdMovie = await _movieRepository.CreateAsync(movie);
            return _mapper.Map<MovieDto>(createdMovie);
        }

        public async Task<MovieDto> UpdateAsync(int id, UpdateMovieDto updateMovieDto)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
                throw new InvalidOperationException("Movie not found");

            if (!string.IsNullOrEmpty(updateMovieDto.Title) && updateMovieDto.Title != movie.Title)
            {
                if (await _movieRepository.ExistsByTitleAsync(updateMovieDto.Title))
                    throw new InvalidOperationException("Movie with this title already exists");
                movie.Title = updateMovieDto.Title;
            }

            if (updateMovieDto.Description != null)
                movie.Description = updateMovieDto.Description;
            if (updateMovieDto.Director != null)
                movie.Director = updateMovieDto.Director;
            if (updateMovieDto.Genre != null)
                movie.Genre = updateMovieDto.Genre;
            if (updateMovieDto.ReleaseYear.HasValue)
                movie.ReleaseYear = updateMovieDto.ReleaseYear;
            if (updateMovieDto.Rating != null)
                movie.Rating = updateMovieDto.Rating;
            if (updateMovieDto.Runtime.HasValue)
                movie.Runtime = updateMovieDto.Runtime;
            if (updateMovieDto.PosterUrl != null)
                movie.PosterUrl = updateMovieDto.PosterUrl;
            if (updateMovieDto.TrailerUrl != null)
                movie.TrailerUrl = updateMovieDto.TrailerUrl;

            movie.UpdatedAt = DateTime.UtcNow;

            var updatedMovie = await _movieRepository.UpdateAsync(movie);
            return _mapper.Map<MovieDto>(updatedMovie);
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _movieRepository.ExistsAsync(id))
                throw new InvalidOperationException("Movie not found");

            await _movieRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _movieRepository.ExistsAsync(id);
        }
    }
} 