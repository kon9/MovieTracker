import { useEffect, useState } from 'react';
import { Search, Plus, Users, Lock, Globe, CheckCircle, Clock } from 'lucide-react';
import { Queue } from '../types';
import { queueService } from '../services/api';
import { useAuth } from '../contexts/AuthContext';

const Queues = () => {
  const [queues, setQueues] = useState<Queue[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState('');
  const [filteredQueues, setFilteredQueues] = useState<Queue[]>([]);
  const [filter, setFilter] = useState<'all' | 'public' | 'my'>('all');
  
  const { user, isAuthenticated } = useAuth();

  useEffect(() => {
    const fetchQueues = async () => {
      try {
        const data = await queueService.getAll();
        setQueues(data);
        setFilteredQueues(data);
      } catch (error) {
        console.error('Error fetching queues:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchQueues();
  }, []);

  useEffect(() => {
    let filtered = queues;

    // Apply filter
    if (filter === 'public') {
      filtered = queues.filter(queue => queue.isPublic);
    } else if (filter === 'my' && user) {
      filtered = queues.filter(queue => queue.ownerId === user.id);
    }

    // Apply search
    if (searchQuery) {
      filtered = filtered.filter((queue) =>
        queue.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
        queue.description?.toLowerCase().includes(searchQuery.toLowerCase()) ||
        queue.ownerUsername?.toLowerCase().includes(searchQuery.toLowerCase())
      );
    }

    setFilteredQueues(filtered);
  }, [searchQuery, queues, filter, user]);

  const getProgressPercentage = (queue: Queue) => {
    return queue.totalItems > 0 ? (queue.completedItems / queue.totalItems) * 100 : 0;
  };

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
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Queues</h1>
          <p className="text-gray-600 dark:text-gray-300">
            Manage your watchlists and discover public queues
          </p>
        </div>
        {isAuthenticated && (
          <button className="bg-primary-600 hover:bg-primary-700 text-white px-4 py-2 rounded-lg font-semibold transition-colors flex items-center gap-2">
            <Plus className="h-4 w-4" />
            Create Queue
          </button>
        )}
      </div>

      {/* Filters */}
      <div className="flex flex-col sm:flex-row gap-4">
        <div className="flex-1 relative">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 h-5 w-5" />
          <input
            type="text"
            placeholder="Search queues by name, description, or owner..."
            className="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>
        
        <div className="flex gap-2">
          <button
            onClick={() => setFilter('all')}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              filter === 'all'
                ? 'bg-primary-600 text-white'
                : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-gray-600'
            }`}
          >
            All
          </button>
          <button
            onClick={() => setFilter('public')}
            className={`px-4 py-2 rounded-lg font-medium transition-colors flex items-center gap-1 ${
              filter === 'public'
                ? 'bg-primary-600 text-white'
                : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-gray-600'
            }`}
          >
            <Globe className="h-4 w-4" />
            Public
          </button>
          {isAuthenticated && (
            <button
              onClick={() => setFilter('my')}
              className={`px-4 py-2 rounded-lg font-medium transition-colors flex items-center gap-1 ${
                filter === 'my'
                  ? 'bg-primary-600 text-white'
                  : 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-gray-600'
              }`}
            >
              <Users className="h-4 w-4" />
              My Queues
            </button>
          )}
        </div>
      </div>

      {/* Queues Grid */}
      {filteredQueues.length === 0 ? (
        <div className="text-center py-12">
          <Users className="h-16 w-16 text-gray-400 mx-auto mb-4" />
          <h3 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
            No queues found
          </h3>
          <p className="text-gray-600 dark:text-gray-300">
            {searchQuery ? 'Try adjusting your search terms.' : 'No queues available yet.'}
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredQueues.map((queue) => (
            <div
              key={queue.id}
              className="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow cursor-pointer"
            >
              <div className="flex items-start justify-between mb-4">
                <h3 className="text-xl font-semibold text-gray-900 dark:text-white line-clamp-2">
                  {queue.name}
                </h3>
                <div className="flex items-center gap-1 ml-2">
                  {queue.isPublic ? (
                    <Globe className="h-5 w-5 text-green-500" />
                  ) : (
                    <Lock className="h-5 w-5 text-gray-400" />
                  )}
                </div>
              </div>

              {queue.description && (
                <p className="text-gray-600 dark:text-gray-300 text-sm mb-4 line-clamp-3">
                  {queue.description}
                </p>
              )}

              <div className="space-y-3 mb-4">
                <div className="flex items-center justify-between text-sm text-gray-500 dark:text-gray-400">
                  <span>by {queue.ownerUsername}</span>
                  <span>{queue.totalItems} items</span>
                </div>

                <div>
                  <div className="flex items-center justify-between text-sm mb-1">
                    <span className="text-gray-600 dark:text-gray-300">Progress</span>
                    <span className="text-gray-600 dark:text-gray-300">
                      {queue.completedItems}/{queue.totalItems}
                    </span>
                  </div>
                  <div className="bg-gray-200 dark:bg-gray-700 rounded-full h-2">
                    <div
                      className="bg-primary-600 h-2 rounded-full transition-all duration-300"
                      style={{ width: `${getProgressPercentage(queue)}%` }}
                    ></div>
                  </div>
                </div>
              </div>

              <div className="flex items-center justify-between">
                <div className="flex items-center gap-4 text-sm text-gray-500 dark:text-gray-400">
                  <div className="flex items-center gap-1">
                    <CheckCircle className="h-4 w-4 text-green-500" />
                    <span>{queue.completedItems} done</span>
                  </div>
                  <div className="flex items-center gap-1">
                    <Clock className="h-4 w-4 text-orange-500" />
                    <span>{queue.totalItems - queue.completedItems} pending</span>
                  </div>
                </div>
              </div>

              <div className="mt-4 pt-4 border-t border-gray-200 dark:border-gray-700">
                <div className="flex gap-2">
                  <button className="flex-1 bg-primary-600 hover:bg-primary-700 text-white text-sm px-3 py-2 rounded transition-colors">
                    View Queue
                  </button>
                  {isAuthenticated && queue.ownerId === user?.id && (
                    <button className="flex-1 border border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700 text-sm px-3 py-2 rounded transition-colors">
                      Edit
                    </button>
                  )}
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Queues;
