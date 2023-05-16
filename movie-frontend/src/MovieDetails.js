import { useContext, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { AuthContext } from "./AuthContext";
import styles from "./MovieDetails.module.scss";
import { ClipLoader } from "react-spinners";
import { CSSTransition, TransitionGroup } from "react-transition-group";

function MovieDetails() {
  const { authToken } = useContext(AuthContext);
  const { id } = useParams();
  const [movie, setMovie] = useState(null);
  const [comments, setComments] = useState([]);
  const [comment, setComment] = useState("");
  const [averageRating, setAverageRating] = useState(null);

  const fetchMovieAndRatings = async () => {
    // Fetch movie data
    const responseMovie = await fetch(
      `https://localhost:7028/api/Movies/${id}`,
      {
        headers: { Authorization: `Bearer ${authToken}` },
      }
    );
    const dataMovie = await responseMovie.json();
    setMovie(dataMovie);

    // Fetch ratings data
    const responseRatings = await fetch(
      `https://localhost:7028/api/Ratings/${id}`,
      {
        headers: { Authorization: `Bearer ${authToken}` },
      }
    );
    const dataRatings = await responseRatings.json();

    // Calculate average rating
    if (dataRatings.$values && dataRatings.$values.length) {
      const sum = dataRatings.$values.reduce((a, b) => a + b.score, 0);
      const avg = sum / dataRatings.$values.length;
      setAverageRating(avg.toFixed(2));
    } else {
      setAverageRating("No ratings yet");
    }
  };

  useEffect(() => {
    fetchMovieAndRatings();
  }, [authToken, id]);

  const fetchComments = async () => {
    const response = await fetch(`https://localhost:7028/api/Comments/${id}`, {
      headers: { Authorization: `Bearer ${authToken}` },
    });

    if (!response.ok) {
      console.error("Error:", response.statusText);
      return;
    }

    const text = await response.text();

    if (!text) {
      console.log(`No comments found for movie id ${id}`);
      setComments([]);
      return;
    }

    const data = JSON.parse(text);
    console.log(data);
    setComments(data.$values); // Here we are setting the $values array to the comments state
  };

  useEffect(() => {
    fetchComments();
  }, [authToken, id]);

  const rateMovie = async (rate) => {
    await fetch("https://localhost:7028/api/Ratings", {
      method: "POST",
      body: JSON.stringify({ MovieId: id, Score: rate.toString() }), // Convert rate to string
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${authToken}`,
      },
    });
    fetchMovieAndRatings();
  };

  if (!movie) {
    return <ClipLoader color="#61dafb" />;
  }
  const addComment = async () => {
    const newComment = { MovieId: id, Content: comment };
    const response = await fetch("https://localhost:7028/api/Comments", {
      method: "POST",
      body: JSON.stringify(newComment),
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${authToken}`,
      },
    });

    if (response.ok) {
      setComment("");
      fetchComments(); // Fetch comments again to refresh the list
    } else {
      console.error("Error adding comment:", response.statusText);
    }
  };

  return (
    movie && (
      <TransitionGroup className={styles.container}>
        <CSSTransition key={movie.id} timeout={500} classNames="slide">
          <div className={styles.container}>
            <h1>{movie.name}</h1>
            <img
              className={styles.movieImage}
              src={movie.imageUrl}
              alt={movie.name}
            />
            <p className={styles.movieDescription}>{movie.description}</p>
            <p>Average Rating: {averageRating}</p>

            <div>
              {[...Array(11).keys()].map((num) => (
                <button
                  key={num}
                  className={styles.rateButton}
                  onClick={() => rateMovie(num)}
                >
                  {num}
                </button>
              ))}
            </div>
            <input
              className={styles.inputField}
              value={comment}
              onChange={(e) => setComment(e.target.value)}
              placeholder="Comment"
            />
            <button className={styles.submitButton} onClick={addComment}>
              Add Comment
            </button>
            <div className={styles.comments}>
              <h2>Comments</h2>
              {comments.map((comment, index) => (
                <p key={index}>{comment.content}</p>
              ))}
            </div>
          </div>
        </CSSTransition>
      </TransitionGroup>
    )
  );
}

export default MovieDetails;
