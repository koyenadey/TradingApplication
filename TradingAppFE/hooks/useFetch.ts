import { useEffect, useState } from "react";

export const useFetch = <T>(url: string) => {
  const [loadedData, setLoadedData] = useState<T | undefined>(undefined);
  const [errorMsg, setError] = useState("");
  const [isdataLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchData = async (url: string) => {
      try {
        const response = await fetch(url);
        if (!response.ok) throw new Error("Failed to fetch the data");
        const result: T = await response.json();
        setLoadedData(result);
        setIsLoading(false);
        setError("");
      } catch (err: any) {
        setError(err.message);
      }
    };
    fetchData(url);
  }, [url]);
  return { loadedData, isdataLoading, errorMsg };
};
