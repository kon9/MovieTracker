import { useState } from "react";
import { useNavigate } from "react-router-dom";
import styles from "./RegistrationPage.module.scss";

function RegistrationPage() {
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const register = async () => {
    const response = await fetch("https://localhost:7028/api/Auth/Register", {
      method: "POST",
      body: JSON.stringify({
        UserName: username,
        Password: password,
      }),
      headers: { "Content-Type": "application/json" },
    });
    if (response.ok) {
      navigate("/login");
    }
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
      <button className={styles["submit-button"]} onClick={register}>
        Register
      </button>
    </div>
  );
}

export default RegistrationPage;
