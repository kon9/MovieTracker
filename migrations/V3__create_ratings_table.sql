-- Flyway migration V3 : create ratings table
CREATE TABLE IF NOT EXISTS public.ratings (
    id          SERIAL PRIMARY KEY,
    score       INT  NOT NULL,
    movie_id    INT  NOT NULL,
    app_user_id TEXT NOT NULL,
    CONSTRAINT fk_ratings_movies
        FOREIGN KEY (movie_id)
        REFERENCES public.movies(id)
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS ix_ratings_movie_id    ON public.ratings(movie_id);
CREATE INDEX IF NOT EXISTS ix_ratings_app_user_id ON public.ratings(app_user_id);
