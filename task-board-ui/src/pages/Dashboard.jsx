import { useEffect } from "react";

import api from "../services/api";
import useApi from "../hooks/useApi";

import "../styles/dashboard.css";

function Dashboard() {
  const { data, loading, error, execute } = useApi();

  useEffect(() => {
    execute(() => api.get("/dashboard"));
  }, []);

  if (loading) return <h3>Loading...</h3>;

  if (error) return <h3>{error}</h3>;

  return (
    <div className="dashboard">
      <h1>Dashboard</h1>

      <div className="dashboard-cards">
        <div className="card">
          <h3>Projects</h3>

          <p>{data?.totalProjects}</p>
        </div>

        <div className="card">
          <h3>Tasks</h3>

          <p>{data?.totalTasks}</p>
        </div>

        <div className="card">
          <h3>Completed</h3>

          <p>{data?.completedTasks}</p>
        </div>

        <div className="card">
          <h3>Overdue</h3>

          <p>{data?.overdueTasks}</p>
        </div>
      </div>
    </div>
  );
}

export default Dashboard;
