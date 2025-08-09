namespace MovieTracker.Infrastructure.Migrations
{
    public class Migration002_SampleData : Migration
    {
        public override int Version => 2;
        public override string Description => "Insert sample data";

        public override string UpScript => @"
-- Insert sample users
INSERT INTO users (username, email, password_hash) VALUES
('admin', 'admin@example.com', 'admin123'),
('user1', 'user1@example.com', 'user123'),
('user2', 'user2@example.com', 'user123');

-- Insert sample movies
INSERT INTO movies (title, description, director, genre, release_year, rating, runtime) VALUES
('The Shawshank Redemption', 'Two imprisoned men bond over a number of years...', 'Frank Darabont', 'Drama', 1994, 9.3, 142),
('The Godfather', 'The aging patriarch of an organized crime dynasty...', 'Francis Ford Coppola', 'Crime', 1972, 9.2, 175),
('Pulp Fiction', 'The lives of two mob hitmen, a boxer, a gangster and his wife...', 'Quentin Tarantino', 'Crime', 1994, 8.9, 154);

-- Insert sample queues
INSERT INTO queues (name, description, is_public, owner_id) VALUES
('My Watchlist', 'Movies I want to watch', FALSE, 1),
('Classic Movies', 'Must-watch classic films', TRUE, 1),
('Action Movies', 'High-octane action films', TRUE, 2);

-- Insert queue members
INSERT INTO queue_members (queue_id, user_id, role) VALUES
(1, 1, 'Owner'),
(2, 1, 'Owner'),
(2, 2, 'Member'),
(3, 2, 'Owner'),
(3, 1, 'Member');

-- Insert queue items
INSERT INTO queue_items (queue_id, title, description, type, position, status, added_by_id) VALUES
(1, 'The Shawshank Redemption', 'Classic prison drama', 'movie', 1, 'Pending', 1),
(1, 'The Godfather', 'Mafia classic', 'movie', 2, 'Completed', 1),
(2, 'Pulp Fiction', 'Quentin Tarantino masterpiece', 'movie', 1, 'Pending', 1),
(3, 'Die Hard', 'Action classic', 'movie', 1, 'InProgress', 2);
";

        public override string DownScript => @"
-- Remove sample data in reverse order
DELETE FROM queue_items;
DELETE FROM queue_members;
DELETE FROM queues;
DELETE FROM movies;
DELETE FROM users;
";
    }
}

