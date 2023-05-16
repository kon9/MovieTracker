import { useContext, useState } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "./AuthContext";
import styles from "./LoginPage.module.scss";

function LoginPage() {
  const { setAuthToken } = useContext(AuthContext);
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const login = async () => {
    const response = await fetch("https://localhost:7028/api/Auth/Login", {
      method: "POST",
      body: JSON.stringify({ UserName: username, Password: password }),
      headers: { "Content-Type": "application/json" },
    });
    const data = await response.json();
    setAuthToken(data.token);
    navigate("/movies");
  };
  return (
    <div className={styles.container}>
      <input
        className={styles["input-field"]}
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        placeholder="Username"
      />
      <input
        className={styles["input-field"]}
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        placeholder="Password"
      />
      <button className={styles["submit-button"]} onClick={login}>
        Login
      </button>
    </div>
  );
}
export default LoginPage;
