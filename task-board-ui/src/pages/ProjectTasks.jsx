import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import api from "../services/api";
import useApi from "../hooks/useApi";

import TaskCard from "../components/TaskCard";

import "../styles/tasks.css";

function ProjectTasks() {
  const { id } = useParams();

  const { data, setData, loading, error, execute } = useApi();

  const [page, setPage] = useState(1);

  const [form, setForm] = useState({
    title: "",
    description: "",
    priority: "Medium",
    status: "Todo",
    dueDate: "",
  });

  useEffect(() => {
    loadTasks();
  }, [page]);

  async function loadTasks() {
    const result = await execute(() =>
      api.get(`/projects/${id}/tasks?page=${page}&pageSize=5`),
    );
  }

  function changeInput(e) {
    setForm({
      ...form,

      [e.target.name]: e.target.value,
    });
  }

  function getPriority(value) {
    const map = {
      Low: 0,
      Medium: 1,
      High: 2,
      Critical: 3,
    };

    return map[value];
  }

  function getStatus(value) {
    const map = {
      Todo: 0,
      InProgress: 1,
      Review: 2,
      Done: 3,
    };

    return map[value];
  }

  async function createTask(e) {
    e.preventDefault();

    await execute(() =>
      api.post(`/projects/${id}/tasks`, {
        title: form.title,

        description: form.description,

        priority: {
          Low: 0,
          Medium: 1,
          High: 2,
          Critical: 3,
        }[form.priority],

        status: {
          Todo: 0,
          InProgress: 1,
          Review: 2,
          Done: 3,
        }[form.status],

        dueDate: form.dueDate,
      }),
    );

    setForm({
      title: "",
      description: "",
      priority: "Medium",
      status: "Todo",
      dueDate:""
    });

    loadTasks();
  }

  async function deleteTask(taskId) {
    await execute(() => api.delete(`/projects/${id}/tasks/${taskId}`));

    loadTasks();
  }

  async function toggleStatus(task) {
    let nextStatus =
      task.status === 0 ? 1 : task.status === 1 ? 2 : task.status === 2 ? 3 : 0;

    await execute(() =>
      api.put(`/projects/${id}/tasks/${task.id}`, {
        title: task.title,
        description: task.description,
        priority: task.priority,
        status: nextStatus,
      }),
    );

    setData((prev) => {
      if (!prev || !prev.data) {
        return prev;
      }

      return {
        ...prev,
        data: prev.data.map((t) =>
          t.id === task.id
            ? {
                ...t,
                status: nextStatus,
              }
            : t,
        ),
      };
    });
  }

  async function updateTask(taskId, taskData) {
    await execute(() =>
      api.put(
        `/projects/${id}/tasks/${taskId}`,

        taskData,
      ),
    );

    loadTasks();
  }

  return (
    <div className="tasks">
      <h1>Tasks</h1>

      <form className="task-form" onSubmit={createTask}>
        <input
          name="title"
          placeholder="Title"
          value={form.title}
          onChange={changeInput}
        />

        <textarea
          name="description"
          placeholder="Description"
          value={form.description}
          onChange={changeInput}
        />

        <input
          type="date"
          name="dueDate"
          value={form.dueDate}
          onChange={changeInput}
        />

        <select name="priority" value={form.priority} onChange={changeInput}>
          <option>Low</option>

          <option>Medium</option>

          <option>High</option>

          <option>Critical</option>
        </select>

        <select name="status" value={form.status} onChange={changeInput}>
          <option>Todo</option>

          <option>InProgress</option>

          <option>Done</option>
        </select>

        <button>Create Task</button>
      </form>

      {loading && <p>Loading...</p>}

      {error && <p>{error}</p>}

      <div className="task-list">
        {data?.data?.map((task) => (
          <TaskCard
            task={task}
            onDelete={deleteTask}
            onToggle={toggleStatus}
            onUpdate={updateTask}
          />
        ))}
      </div>

      <div className="pagination">
        <button disabled={page === 1} onClick={() => setPage(page - 1)}>
          Previous
        </button>

        <span>Page {page}</span>

        <button onClick={() => setPage(page + 1)}>Next</button>
      </div>
    </div>
  );
}

export default ProjectTasks;
