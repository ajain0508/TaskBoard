import { useState } from "react";
import CommentBox from "./CommentBox";
import "../styles/tasks.css";

function TaskCard({ task, onDelete, onToggle, onUpdate }) {
  const [showComments, setShowComments] = useState(false);
  const [editing, setEditing] = useState(false);

  const [title, setTitle] = useState(task.title);

  const [description, setDescription] = useState(task.description);

  const [priority, setPriority] = useState(task.priority);

  const [dueDate, setDueDate] = useState(task.dueDate?.split("T")[0] || "");

  return (
    <div className="task-card">
      <h3>{task.title}</h3>

      <p>{task.description}</p>

      <p>
        Priority:
        {["Low", "Medium", "High", "Critical"][task.priority]}
      </p>

      <p>
        Status:
        {["Todo", "InProgress", "Review", "Done"][task.status]}
      </p>

      {task.dueDate && (
        <p>Due Date: {new Date(task.dueDate).toLocaleDateString()}</p>
      )}

      {editing ? (
        <div className="task-edit">
          <input value={title} onChange={(e) => setTitle(e.target.value)} />

          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />

          <select
            value={priority}
            onChange={(e) => setPriority(Number(e.target.value))}
          >
            <option value={0}>Low</option>

            <option value={1}>Medium</option>

            <option value={2}>High</option>

            <option value={3}>Critical</option>
          </select>

          <input
            type="date"
            value={dueDate}
            onChange={(e) => setDueDate(e.target.value)}
          />
          <button
            onClick={() => {
              onUpdate(
                task.id,

                {
                  title,
                  description,
                  priority,
                  status: task.status,
                  dueDate,
                },
              );

              setEditing(false);
            }}
          >
            Save
          </button>

          <button onClick={() => setEditing(false)}>Cancel</button>
        </div>
      ) : (
        <button onClick={() => setEditing(true)}>Edit</button>
      )}

      <button onClick={() => onToggle(task)}>Toggle Status</button>

      <button onClick={() => onDelete(task.id)}>Delete</button>

      <button onClick={() => setShowComments(!showComments)}>Comments</button>

      {showComments && <CommentBox taskId={task.id} />}
    </div>
  );
}

export default TaskCard;
