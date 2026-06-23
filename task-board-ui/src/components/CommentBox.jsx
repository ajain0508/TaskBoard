import { useEffect, useState } from "react";
import api from "../services/api";
import useApi from "../hooks/useApi";

function CommentBox({ taskId }) {
  const { data, execute } = useApi();

  const [comment, setComment] = useState("");

  const comments = Array.isArray(data) ? data : data?.data || [];

  useEffect(() => {
    loadComments();
  }, [taskId]);

  async function loadComments() {
    await execute(() => api.get(`/tasks/${taskId}/comments`));
  }

  async function addComment(e) {
    e.preventDefault();

    if (!comment.trim()) return;

    await execute(() =>
      api.post(`/tasks/${taskId}/comments`, {
        author: "User",
        body: comment,
      }),
    );

    setComment("");

    loadComments();
  }

  return (
    <div className="comment-box">
      <h3>Comments</h3>

      <form onSubmit={addComment}>
        <input
          placeholder="Add comment"
          value={comment}
          onChange={(e) => setComment(e.target.value)}
        />

        <button>Add</button>
      </form>

      <div>
        {comments.map((c) => (
          <div key={c.id} className="comment">
            <strong>Author : {c.author}</strong>

            <p>{c.body}</p>
          </div>
        ))}
      </div>
    </div>
  );
}

export default CommentBox;
