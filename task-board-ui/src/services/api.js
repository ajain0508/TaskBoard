import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5283/api",
});

export default api;
