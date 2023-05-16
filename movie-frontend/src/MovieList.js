import { useContext, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { AuthContext } from "./AuthContext";
import axios from "axios";
import styles from "./MovieList.module.scss";
import { ClipLoader } from "react-spinners";
import { CSSTransition, TransitionGroup } from "react-transition-group";
import { useNavigate } from "react-router-dom";

function MovieList() {
  const { authToken } = useContext(AuthContext);
  const [movies, setMovies] = useState([]);
  const [showForm, setShowForm] = useState(false); // New state for showing or hiding the form
  const [newMovie, setNewMovie] = useState({
    name: "",
    description: "",
    imageUrl: "",
  });
  const navigate = useNavigate();

  useEffect(() => {
    if (!authToken) {
      navigate("/");
    }
  }, [authToken, navigate]);

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        const response = await axios.get("https://localhost:7028/api/Movies", {
          headers: { Authorization: `Bearer ${authToken}` },
        });

        if (!response.data.$values || !Array.isArray(response.data.$values)) {
          throw new Error(
            `Data is not an array! Data: ${JSON.stringify(response.data)}`
          );
        }

        setMovies(response.data.$values);
      } catch (error) {
        console.error("There was a problem fetching the movies:", error);
      }
    };

    fetchMovies();
  }, [authToken]);

  const addMovie = async () => {
    try {
      const response = await axios.post(
        "https://localhost:7028/api/Movies",
        newMovie,
        {
          headers: { Authorization: `Bearer ${authToken}` },
        }
      );

      if (response.status === 200) {
        setNewMovie({ name: "", description: "", imageUrl: "" });

        // Fetch the updated list of movies after adding a new movie
        const updatedMoviesResponse = await axios.get(
          "https://localhost:7028/api/Movies",
          {
            headers: { Authorization: `Bearer ${authToken}` },
          }
        );

        if (
          updatedMoviesResponse.status === 200 &&
          updatedMoviesResponse.data &&
          updatedMoviesResponse.data.$values
        ) {
          setMovies(updatedMoviesResponse.data.$values);
        } else {
          throw new Error(
            `Invalid response while fetching updated movie list: ${JSON.stringify(
              updatedMoviesResponse.data
            )}`
          );
        }
      }
    } catch (error) {
      console.error("There was a problem adding the movie:", error);
    }
  };

  if (!movies.length) {
    return <ClipLoader color="#61dafb" />;
  }

  function getAverageRating(ratings) {
    if (!ratings || !ratings.$values) return "No ratings yet";
    const ratingsArray = ratings.$values;
    if (ratingsArray.length === 0) return "No ratings yet";
    const sum = ratingsArray.reduce((a, b) => a + b.score, 0);
    return (sum / ratingsArray.length).toFixed(1);
  }
  return (
    <div className={styles.container}>
      <button
        className={styles["submit-button"]}
        onClick={() => setShowForm(!showForm)}
      >
        Create Movie
      </button>
      {showForm && (
        <div>
          <h2>Add a new movie</h2>
          <form>
            <label>
              Name:
              <input
                type="text"
                className={styles["form-input"]}
                value={newMovie.name}
                onChange={(e) =>
                  setNewMovie({ ...newMovie, name: e.target.value })
                }
              />
            </label>
            <label>
              Description:
              <input
                type="text"
                className={styles["form-input"]}
                value={newMovie.description}
                onChange={(e) =>
                  setNewMovie({ ...newMovie, description: e.target.value })
                }
              />
            </label>
            <label>
              Image URL:
              <input
                type="text"
                className={styles["form-input"]}
                value={newMovie.imageUrl}
                onChange={(e) =>
                  setNewMovie({ ...newMovie, imageUrl: e.target.value })
                }
              />
            </label>
            <button
              type="button"
              className={styles["form-button"]}
              onClick={addMovie}
            >
              Add Movie
            </button>
          </form>
        </div>
      )}
      <TransitionGroup className={styles.container}>
        {movies.map((movie) => (
          <CSSTransition key={movie.id} timeout={500} classNames="fade">
            <div className={styles["movie-container"]}>
              {" "}
              <Link to={`/movie/${movie.id}`} className={styles["movie-link"]}>
                <img
                  src={movie.imageUrl}
                  alt={movie.name}
                  className={styles["movie-image"]}
                />
                <div className={styles["movie-details"]}>
                  {" "}
                  <h2 className={styles["movie-name"]}>{movie.name}</h2>
                  <p>Average Rating: {getAverageRating(movie.ratings)}</p>
                </div>
              </Link>
            </div>
          </CSSTransition>
        ))}
      </TransitionGroup>
    </div>
  );
}

export default MovieList;
