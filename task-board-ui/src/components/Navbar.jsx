import { Link } from "react-router-dom";
import { useContext } from "react";
import { AppContext } from "../context/AppContext";
import "../styles/navbar.css";

function Navbar() {
  const { darkMode, setDarkMode } = useContext(AppContext);

  return (
    <nav>
      <h2>Task Board</h2>

      <div>
        <Link to="/">Dashboard</Link>

        <Link to="/projects">Projects</Link>

        <button onClick={() => setDarkMode(!darkMode)}>
          {darkMode ? "Light" : "Dark"}
        </button>
      </div>
    </nav>
  );
}

export default Navbar;
