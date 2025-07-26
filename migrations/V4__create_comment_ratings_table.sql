-- Flyway migration V4 : create comment_ratings table
CREATE TABLE IF NOT EXISTS public.comment_ratings (
    id          SERIAL PRIMARY KEY,
    comment_id  INT  NOT NULL,
    app_user_id TEXT NOT NULL,
    is_upvote   BOOLEAN NOT NULL,
    CONSTRAINT fk_comment_ratings_comment
        FOREIGN KEY (comment_id)
        REFERENCES public.comments(id)
        ON DELETE NO ACTION
);

CREATE INDEX IF NOT EXISTS ix_comment_ratings_comment_id ON public.comment_ratings(comment_id);
CREATE INDEX IF NOT EXISTS ix_comment_ratings_app_user_id ON public.comment_ratings(app_user_id);
