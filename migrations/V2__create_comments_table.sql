-- Flyway migration V2 : create comments table
CREATE TABLE IF NOT EXISTS public.comments (
    id               SERIAL PRIMARY KEY,
    content          TEXT        NOT NULL,
    rating           INT         NOT NULL,
    movie_id         INT         NOT NULL,
    app_user_id      TEXT        NOT NULL,
    created_at       TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    total_rating     INT         NOT NULL DEFAULT 0,
    parent_comment_id INT,
    CONSTRAINT fk_comments_movies
        FOREIGN KEY (movie_id)
        REFERENCES public.movies(id)
        ON DELETE CASCADE,
    CONSTRAINT fk_comments_parent
        FOREIGN KEY (parent_comment_id)
        REFERENCES public.comments(id)
);

-- Useful indexes
CREATE INDEX IF NOT EXISTS ix_comments_movie_id         ON public.comments(movie_id);
CREATE INDEX IF NOT EXISTS ix_comments_app_user_id      ON public.comments(app_user_id);
CREATE INDEX IF NOT EXISTS ix_comments_parent_comment_id ON public.comments(parent_comment_id);
