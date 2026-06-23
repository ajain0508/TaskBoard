import { useEffect, useState } from "react";

import api from "../services/api";
import useApi from "../hooks/useApi";

import ProjectCard from "../components/ProjectCard";

import "../styles/projects.css";

function Projects() {
  const { data, setData, loading, error, execute } = useApi();

  const [form, setForm] = useState({
    name: "",
    description: "",
  });

  useEffect(() => {
    loadProjects();
  }, []);

  function loadProjects() {
    execute(() => api.get("/projects"));
  }

  async function deleteProject(id) {
    const confirmDelete = window.confirm("Delete this project?");

    if (!confirmDelete) return;

    await execute(() => api.delete(`/projects/${id}`));

    setData((prev) => {
      if (!prev) return prev;

      return {
        ...prev,
        data: prev.data.filter((project) => project.id !== id),
      };
    });
  }

  function handleChange(e) {
    setForm({
      ...form,

      [e.target.name]: e.target.value,
    });
  }

  async function createProject(e) {
    e.preventDefault();

    await execute(() => api.post("/projects", form));

    setForm({
      name: "",
      description: "",
    });

    loadProjects();
  }

  async function deleteProject(projectId) {
    const confirmDelete = window.confirm("Delete this project?");

    if (!confirmDelete) return;

    await execute(() => api.delete(`/projects/${projectId}`));

    loadProjects();
  }

  async function updateProject(id, projectData){

    await execute(()=>api.put(`/projects/${id}`,projectData));

    loadProjects();
  }

  

  return (
    <div className="projects">
      <h1>Projects</h1>

      <form className="project-form" onSubmit={createProject}>
        <input
          name="name"
          placeholder="Project name"
          value={form.name}
          onChange={handleChange}
        />

        <textarea
          name="description"
          placeholder="Description"
          value={form.description}
          onChange={handleChange}
        />

        <button>Create Project</button>
      </form>

      {loading && <p>Loading...</p>}

      {error && <p>{error}</p>}

      <div className="project-list">
        {data?.map((project) => (
          <ProjectCard key={project.id} project={project} onDelete={deleteProject} onUpdate={updateProject} />
        ))}
      </div>
    </div>
  );
}

export default Projects;
