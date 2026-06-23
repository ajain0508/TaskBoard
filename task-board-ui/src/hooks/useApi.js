import { useState } from "react";

function useApi() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const execute = async (apiCall) => {
    try {
      setLoading(true);
      setError("");

      const response = await apiCall();

      if (response.data) {
        setData(response.data);
      }

      return response.data;
      
    } catch (err) {
      setError(err.response?.data || "Something went wrong");
    } finally {
      setLoading(false);
    }
  };

  return {
    data,
    setData,
    loading,
    error,
    execute,
  };
}

export default useApi;
