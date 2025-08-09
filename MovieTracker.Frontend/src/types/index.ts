export interface User {
  id: number;
  username: string;
  email: string;
  createdAt: string;
  lastLoginAt?: string;
}

export interface Movie {
  id: number;
  title: string;
  description?: string;
  director?: string;
  genre?: string;
  releaseYear?: number;
  rating?: string;
  runtime?: number;
  posterUrl?: string;
  trailerUrl?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface Queue {
  id: number;
  name: string;
  description?: string;
  isPublic: boolean;
  ownerId: number;
  ownerUsername?: string;
  totalItems: number;
  completedItems: number;
  createdAt: string;
  updatedAt?: string;
  items?: QueueItem[];
  members?: QueueMember[];
}

export interface QueueItem {
  id: number;
  queueId: number;
  title: string;
  description?: string;
  type?: string;
  externalId?: string;
  imageUrl?: string;
  position: number;
  status: string;
  addedAt: string;
  completedAt?: string;
  addedById?: number;
  addedByUsername?: string;
}

export interface QueueMember {
  id: number;
  queueId: number;
  userId: number;
  username?: string;
  role: string;
  joinedAt: string;
}

export interface Comment {
  id: number;
  userId: number;
  username?: string;
  movieId: number;
  content: string;
  createdAt: string;
  updatedAt?: string;
}

export interface Rating {
  id: number;
  userId: number;
  username?: string;
  movieId: number;
  score: number;
  review?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateUserDto {
  username: string;
  email: string;
  password: string;
}

export interface LoginDto {
  username: string;
  password: string;
}

export interface CreateMovieDto {
  title: string;
  description?: string;
  director?: string;
  genre?: string;
  releaseYear?: number;
  rating?: string;
  runtime?: number;
  posterUrl?: string;
  trailerUrl?: string;
}

export interface CreateQueueDto {
  name: string;
  description?: string;
  isPublic: boolean;
}
