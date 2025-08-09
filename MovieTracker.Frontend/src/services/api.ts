import axios from 'axios';
import { User, Movie, Queue, CreateUserDto, LoginDto, CreateMovieDto, CreateQueueDto } from '../types';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Users API
export const userService = {
  register: async (userData: CreateUserDto): Promise<User> => {
    const response = await api.post('/users/register', userData);
    return response.data;
  },

  login: async (loginData: LoginDto): Promise<string> => {
    const response = await api.post('/users/login', loginData);
    return response.data;
  },

  getProfile: async (userId: number): Promise<User> => {
    const response = await api.get(`/users/${userId}`);
    return response.data;
  },

  getAll: async (): Promise<User[]> => {
    const response = await api.get('/users');
    return response.data;
  },
};

// Movies API
export const movieService = {
  getAll: async (): Promise<Movie[]> => {
    const response = await api.get('/movies');
    return response.data;
  },

  getById: async (id: number): Promise<Movie> => {
    const response = await api.get(`/movies/${id}`);
    return response.data;
  },

  create: async (movieData: CreateMovieDto): Promise<Movie> => {
    const response = await api.post('/movies', movieData);
    return response.data;
  },

  update: async (id: number, movieData: Partial<CreateMovieDto>): Promise<Movie> => {
    const response = await api.put(`/movies/${id}`, movieData);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/movies/${id}`);
  },

  search: async (query: string): Promise<Movie[]> => {
    const response = await api.get(`/movies/search?query=${encodeURIComponent(query)}`);
    return response.data;
  },
};

// Queues API
export const queueService = {
  getAll: async (): Promise<Queue[]> => {
    const response = await api.get('/queues');
    return response.data;
  },

  getById: async (id: number): Promise<Queue> => {
    const response = await api.get(`/queues/${id}`);
    return response.data;
  },

  getByUserId: async (userId: number): Promise<Queue[]> => {
    const response = await api.get(`/queues/user/${userId}`);
    return response.data;
  },

  getPublic: async (): Promise<Queue[]> => {
    const response = await api.get('/queues/public');
    return response.data;
  },

  create: async (ownerId: number, queueData: CreateQueueDto): Promise<Queue> => {
    const response = await api.post(`/queues/${ownerId}`, queueData);
    return response.data;
  },

  update: async (id: number, queueData: Partial<CreateQueueDto>): Promise<Queue> => {
    const response = await api.put(`/queues/${id}`, queueData);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/queues/${id}`);
  },
};

export default api;
