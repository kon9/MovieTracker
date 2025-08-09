import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { Film, Users, Star, TrendingUp } from 'lucide-react';
import { Movie, Queue } from '../types';
import { movieService, queueService } from '../services/api';

const Home = () => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [queues, setQueues] = useState<Queue[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [moviesData, queuesData] = await Promise.all([
          movieService.getAll(),
          queueService.getPublic(),
        ]);
        setMovies(moviesData.slice(0, 6)); // Show only first 6 movies
        setQueues(queuesData.slice(0, 3)); // Show only first 3 public queues
      } catch (error) {
        console.error('Error fetching data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
      </div>
    );
  }

  return (
    <div className="space-y-12">
      {/* Hero Section */}
      <section className="text-center py-12">
        <div className="max-w-4xl mx-auto">
          <h1 className="text-4xl md:text-6xl font-bold text-gray-900 dark:text-white mb-6">
            Track Your Movie Journey
          </h1>
          <p className="text-xl text-gray-600 dark:text-gray-300 mb-8">
            Organize your watchlists, discover new films, and share your movie experiences with friends.
          </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <Link
              to="/movies"
              className="bg-primary-600 hover:bg-primary-700 text-white px-8 py-3 rounded-lg font-semibold transition-colors"
            >
              Browse Movies
            </Link>
            <Link
              to="/queues"
              className="border border-primary-600 text-primary-600 hover:bg-primary-50 dark:hover:bg-primary-900 px-8 py-3 rounded-lg font-semibold transition-colors"
            >
              Explore Queues
            </Link>
          </div>
        </div>
      </section>

      {/* Stats Section */}
      <section className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-md text-center">
          <Film className="h-12 w-12 text-primary-600 mx-auto mb-4" />
          <h3 className="text-2xl font-bold text-gray-900 dark:text-white">{movies.length}</h3>
          <p className="text-gray-600 dark:text-gray-300">Movies Available</p>
        </div>
        <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-md text-center">
          <Users className="h-12 w-12 text-primary-600 mx-auto mb-4" />
          <h3 className="text-2xl font-bold text-gray-900 dark:text-white">{queues.length}</h3>
          <p className="text-gray-600 dark:text-gray-300">Public Queues</p>
        </div>
        <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-md text-center">
          <Star className="h-12 w-12 text-primary-600 mx-auto mb-4" />
          <h3 className="text-2xl font-bold text-gray-900 dark:text-white">4.8</h3>
          <p className="text-gray-600 dark:text-gray-300">Average Rating</p>
        </div>
      </section>

      {/* Featured Movies */}
      <section>
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-3xl font-bold text-gray-900 dark:text-white">Featured Movies</h2>
          <Link
            to="/movies"
            className="text-primary-600 hover:text-primary-700 font-semibold flex items-center gap-2"
          >
            View All <TrendingUp className="h-4 w-4" />
          </Link>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {movies.map((movie) => (
            <div key={movie.id} className="bg-white dark:bg-gray-800 rounded-lg shadow-md overflow-hidden">
              {movie.posterUrl ? (
                <img
                  src={movie.posterUrl}
                  alt={movie.title}
                  className="w-full h-48 object-cover"
                />
              ) : (
                <div className="w-full h-48 bg-gray-200 dark:bg-gray-700 flex items-center justify-center">
                  <Film className="h-16 w-16 text-gray-400" />
                </div>
              )}
              <div className="p-4">
                <h3 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
                  {movie.title}
                </h3>
                <p className="text-gray-600 dark:text-gray-300 text-sm mb-2">
                  {movie.director} • {movie.releaseYear}
                </p>
                <p className="text-gray-700 dark:text-gray-300 text-sm line-clamp-3">
                  {movie.description}
                </p>
                <div className="mt-4 flex items-center justify-between">
                  <span className="bg-primary-100 text-primary-800 text-xs px-2 py-1 rounded">
                    {movie.genre}
                  </span>
                  {movie.rating && (
                    <div className="flex items-center gap-1">
                      <Star className="h-4 w-4 text-yellow-400 fill-current" />
                      <span className="text-sm text-gray-600 dark:text-gray-300">
                        {movie.rating}
                      </span>
                    </div>
                  )}
                </div>
              </div>
            </div>
          ))}
        </div>
      </section>

      {/* Popular Queues */}
      <section>
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-3xl font-bold text-gray-900 dark:text-white">Popular Queues</h2>
          <Link
            to="/queues"
            className="text-primary-600 hover:text-primary-700 font-semibold flex items-center gap-2"
          >
            View All <TrendingUp className="h-4 w-4" />
          </Link>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          {queues.map((queue) => (
            <div key={queue.id} className="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6">
              <h3 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
                {queue.name}
              </h3>
              <p className="text-gray-600 dark:text-gray-300 text-sm mb-4">
                {queue.description}
              </p>
              <div className="flex items-center justify-between text-sm text-gray-500 dark:text-gray-400">
                <span>by {queue.ownerUsername}</span>
                <span>{queue.totalItems} items</span>
              </div>
              <div className="mt-4">
                <div className="bg-gray-200 dark:bg-gray-700 rounded-full h-2">
                  <div
                    className="bg-primary-600 h-2 rounded-full"
                    style={{
                      width: `${queue.totalItems > 0 ? (queue.completedItems / queue.totalItems) * 100 : 0}%`,
                    }}
                  ></div>
                </div>
                <p className="text-xs text-gray-500 dark:text-gray-400 mt-1">
                  {queue.completedItems} of {queue.totalItems} completed
                </p>
              </div>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
};

export default Home;
