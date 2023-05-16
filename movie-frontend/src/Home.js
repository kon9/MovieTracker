import { Link } from "react-router-dom";
import styles from "./Home.module.scss"; // import the new styles

function Home() {
  return (
    <div className={styles.homeContainer}>
      <h1>Welcome</h1>
      <Link to="/login">
        <button className={styles.homeButton}>Login</button>
      </Link>
      <Link to="/register">
        <button className={styles.homeButton}>Register</button>
      </Link>
    </div>
  );
}
export default Home;
