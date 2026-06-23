import { useNavigate } from "react-router-dom";
import "../styles/projects.css";
import { useState } from "react";

function ProjectCard({ project, onDelete, onUpdate }) {
  const navigate = useNavigate();

  const [editing, setEditing] = useState(false);

  const [name, setName] = useState(project.name);
  const [description, setDescription] = useState(project.description);

  return (
    <div className="project-card">
      <h2>{project.name}</h2>

      <p>{project.description}</p>

      <button style={{ marginRight : '3px' }} onClick={() => navigate(`/projects/${project.id}/tasks`)}>
        Open Project
      </button>
      <button onClick={() => onDelete(project.id)}>Delete</button>

      {editing ? (
        <div>
          <input style={{ marginTop : '10px' }} value={name} onChange={(e) => setName(e.target.value)} />

          <textarea style={{ margin : '5px' }}
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
          <div>
          <button
            onClick={() => {
              onUpdate(project.id, {
                name,
                description,
              });

              setEditing(false);
            }}
          >
            Save
          </button>
          </div>

          <button style={{ marginTop : '3px' }} onClick={() => setEditing(false)}>Cancel</button>
        </div>
      ) : (
        <button style={{ marginLeft : '3px' }} onClick={() => setEditing(true)}>Edit</button>
      )}
    </div>
  );
}

export default ProjectCard;
