-- Flyway migration V1 : create movies table
CREATE TABLE IF NOT EXISTS public.movies (
    id              SERIAL PRIMARY KEY,
    name            TEXT            NOT NULL,
    description     TEXT            NOT NULL,
    image_url       TEXT            NOT NULL,
    average_rating  DOUBLE PRECISION NOT NULL DEFAULT 0
);
