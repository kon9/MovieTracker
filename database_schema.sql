-- PostgreSQL Database Schema for MovieTracker
-- Run this script to create the database tables

-- Create database (run this separately if needed)
-- CREATE DATABASE movietracker;

-- Connect to the database and run the following:

-- Users table
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    last_login_at TIMESTAMP NULL
);

-- Movies table
CREATE TABLE movies (
    id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    director VARCHAR(255),
    genre VARCHAR(100),
    release_year INTEGER,
    rating DECIMAL(3,1),
    runtime INTEGER,
    poster_url VARCHAR(500),
    trailer_url VARCHAR(500),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL
);

-- Queues table (generic)
CREATE TABLE queues (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    is_public BOOLEAN NOT NULL DEFAULT FALSE,
    owner_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL
);

-- Queue items table (generic)
CREATE TABLE queue_items (
    id SERIAL PRIMARY KEY,
    queue_id INTEGER NOT NULL REFERENCES queues(id) ON DELETE CASCADE,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    type VARCHAR(100), -- "movie", "book", "game", etc.
    external_id VARCHAR(500), -- ID from external service (IMDB, etc.)
    image_url VARCHAR(500),
    position INTEGER NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'Pending', -- Pending, InProgress, Completed, Skipped
    added_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    completed_at TIMESTAMP NULL,
    added_by_id INTEGER REFERENCES users(id) ON DELETE SET NULL
);

-- Queue members table
CREATE TABLE queue_members (
    id SERIAL PRIMARY KEY,
    queue_id INTEGER NOT NULL REFERENCES queues(id) ON DELETE CASCADE,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    role VARCHAR(20) NOT NULL DEFAULT 'Member', -- Owner, Admin, Member
    joined_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(queue_id, user_id)
);

-- Comments table
CREATE TABLE comments (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    movie_id INTEGER NOT NULL REFERENCES movies(id) ON DELETE CASCADE,
    content TEXT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL
);

-- Ratings table
CREATE TABLE ratings (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    movie_id INTEGER NOT NULL REFERENCES movies(id) ON DELETE CASCADE,
    score INTEGER NOT NULL CHECK (score >= 1 AND score <= 10),
    review TEXT,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL,
    UNIQUE(user_id, movie_id)
);

-- Create indexes for better performance
CREATE INDEX idx_users_username ON users(username);
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_movies_title ON movies(title);
CREATE INDEX idx_movies_genre ON movies(genre);
CREATE INDEX idx_movies_release_year ON movies(release_year);
CREATE INDEX idx_queues_owner_id ON queues(owner_id);
CREATE INDEX idx_queues_is_public ON queues(is_public);
CREATE INDEX idx_queue_items_queue_id ON queue_items(queue_id);
CREATE INDEX idx_queue_items_position ON queue_items(position);
CREATE INDEX idx_queue_members_queue_id ON queue_members(queue_id);
CREATE INDEX idx_queue_members_user_id ON queue_members(user_id);
CREATE INDEX idx_comments_movie_id ON comments(movie_id);
CREATE INDEX idx_comments_user_id ON comments(user_id);
CREATE INDEX idx_ratings_movie_id ON ratings(movie_id);
CREATE INDEX idx_ratings_user_id ON ratings(user_id);

-- Insert sample data
INSERT INTO users (username, email, password_hash) VALUES
('admin', 'admin@example.com', 'admin123'),
('user1', 'user1@example.com', 'user123'),
('user2', 'user2@example.com', 'user123');

INSERT INTO movies (title, description, director, genre, release_year, rating, runtime) VALUES
('The Shawshank Redemption', 'Two imprisoned men bond over a number of years...', 'Frank Darabont', 'Drama', 1994, 9.3, 142),
('The Godfather', 'The aging patriarch of an organized crime dynasty...', 'Francis Ford Coppola', 'Crime', 1972, 9.2, 175),
('Pulp Fiction', 'The lives of two mob hitmen, a boxer, a gangster and his wife...', 'Quentin Tarantino', 'Crime', 1994, 8.9, 154);

INSERT INTO queues (name, description, is_public, owner_id) VALUES
('My Watchlist', 'Movies I want to watch', FALSE, 1),
('Classic Movies', 'Must-watch classic films', TRUE, 1),
('Action Movies', 'High-octane action films', TRUE, 2);

INSERT INTO queue_members (queue_id, user_id, role) VALUES
(1, 1, 'Owner'),
(2, 1, 'Owner'),
(2, 2, 'Member'),
(3, 2, 'Owner'),
(3, 1, 'Member');

INSERT INTO queue_items (queue_id, title, description, type, position, status, added_by_id) VALUES
(1, 'The Shawshank Redemption', 'Classic prison drama', 'movie', 1, 'Pending', 1),
(1, 'The Godfather', 'Mafia classic', 'movie', 2, 'Completed', 1),
(2, 'Pulp Fiction', 'Quentin Tarantino masterpiece', 'movie', 1, 'Pending', 1),
(3, 'Die Hard', 'Action classic', 'movie', 1, 'InProgress', 2); 