import { useEffect, useState } from 'react';
import { Search, Plus, Film, Star, Calendar, Clock } from 'lucide-react';
import { Movie } from '../types';
import { movieService } from '../services/api';

const Movies = () => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState('');
  const [filteredMovies, setFilteredMovies] = useState<Movie[]>([]);

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        const data = await movieService.getAll();
        setMovies(data);
        setFilteredMovies(data);
      } catch (error) {
        console.error('Error fetching movies:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchMovies();
  }, []);

  useEffect(() => {
    const filtered = movies.filter((movie) =>
      movie.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
      movie.director?.toLowerCase().includes(searchQuery.toLowerCase()) ||
      movie.genre?.toLowerCase().includes(searchQuery.toLowerCase())
    );
    setFilteredMovies(filtered);
  }, [searchQuery, movies]);

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Movies</h1>
          <p className="text-gray-600 dark:text-gray-300">
            Discover and explore our collection of {movies.length} movies
          </p>
        </div>
        <button className="bg-primary-600 hover:bg-primary-700 text-white px-4 py-2 rounded-lg font-semibold transition-colors flex items-center gap-2">
          <Plus className="h-4 w-4" />
          Add Movie
        </button>
      </div>

      {/* Search */}
      <div className="relative">
        <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 h-5 w-5" />
        <input
          type="text"
          placeholder="Search movies by title, director, or genre..."
          className="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
        />
      </div>

      {/* Movies Grid */}
      {filteredMovies.length === 0 ? (
        <div className="text-center py-12">
          <Film className="h-16 w-16 text-gray-400 mx-auto mb-4" />
          <h3 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
            No movies found
          </h3>
          <p className="text-gray-600 dark:text-gray-300">
            {searchQuery ? 'Try adjusting your search terms.' : 'No movies available yet.'}
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {filteredMovies.map((movie) => (
            <div
              key={movie.id}
              className="bg-white dark:bg-gray-800 rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow cursor-pointer"
            >
              {movie.posterUrl ? (
                <img
                  src={movie.posterUrl}
                  alt={movie.title}
                  className="w-full h-64 object-cover"
                />
              ) : (
                <div className="w-full h-64 bg-gray-200 dark:bg-gray-700 flex items-center justify-center">
                  <Film className="h-16 w-16 text-gray-400" />
                </div>
              )}
              
              <div className="p-4">
                <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-2 line-clamp-2">
                  {movie.title}
                </h3>
                
                <div className="space-y-2 mb-4">
                  {movie.director && (
                    <p className="text-sm text-gray-600 dark:text-gray-300 flex items-center gap-1">
                      <span className="font-medium">Director:</span> {movie.director}
                    </p>
                  )}
                  
                  <div className="flex items-center gap-4 text-sm text-gray-600 dark:text-gray-300">
                    {movie.releaseYear && (
                      <div className="flex items-center gap-1">
                        <Calendar className="h-3 w-3" />
                        {movie.releaseYear}
                      </div>
                    )}
                    {movie.runtime && (
                      <div className="flex items-center gap-1">
                        <Clock className="h-3 w-3" />
                        {movie.runtime}m
                      </div>
                    )}
                  </div>
                </div>

                <p className="text-gray-700 dark:text-gray-300 text-sm line-clamp-3 mb-4">
                  {movie.description}
                </p>

                <div className="flex items-center justify-between">
                  {movie.genre && (
                    <span className="bg-primary-100 text-primary-800 text-xs px-2 py-1 rounded-full">
                      {movie.genre}
                    </span>
                  )}
                  {movie.rating && (
                    <div className="flex items-center gap-1">
                      <Star className="h-4 w-4 text-yellow-400 fill-current" />
                      <span className="text-sm font-medium text-gray-900 dark:text-white">
                        {movie.rating}
                      </span>
                    </div>
                  )}
                </div>

                <div className="mt-4 pt-4 border-t border-gray-200 dark:border-gray-700">
                  <div className="flex gap-2">
                    <button className="flex-1 bg-primary-600 hover:bg-primary-700 text-white text-sm px-3 py-2 rounded transition-colors">
                      Add to Queue
                    </button>
                    <button className="flex-1 border border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700 text-sm px-3 py-2 rounded transition-colors">
                      Details
                    </button>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Movies;
