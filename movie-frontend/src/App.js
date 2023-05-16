import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { AuthContextProvider } from "./AuthContext";
import LoginPage from "./LoginPage";
import RegistrationPage from "./RegistrationPage";
import MovieList from "./MovieList";
import MovieDetails from "./MovieDetails";
import Home from "./Home";

function App() {
  return (
    <AuthContextProvider>
      <Router>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegistrationPage />} />
          <Route path="/movies" element={<MovieList />} />
          <Route path="/movie/:id" element={<MovieDetails />} />
          <Route path="/" element={<Home />} />
          <Route path="*" element={<Home />} />
        </Routes>
      </Router>
    </AuthContextProvider>
  );
}

export default App;
