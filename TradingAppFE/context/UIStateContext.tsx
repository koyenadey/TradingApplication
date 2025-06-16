import { createContext, ReactNode, useContext, useState } from "react";

export interface UiStateContextType<T> {
  isLoading: boolean;
  setIsLoading: (value: boolean) => void;
  currPlayListId: string;
  setPlaylistId: (value: string) => void;
  currentVidId: string;
  setCurrentVidId: (value: string) => void;
  data: T | undefined;
  setData: (value: T) => void;
  error: string;
  setError: (value: string) => void;
}

interface UiStateContextProps {
  children: ReactNode;
}

export const UIStateContext = createContext<UiStateContextType<any>>({
  isLoading: true,
  setIsLoading: () => {},
  currPlayListId: "",
  setPlaylistId: () => {},
  currentVidId: "",
  setCurrentVidId: () => {},
  data: undefined,
  setData: () => {},
  error: "",
  setError: () => {},
});

export const UIStateProvider = ({ children }: UiStateContextProps) => {
  const [currentVidId, setCurrentVidId] = useState<string>("");
  const [currPlayListId, setPlaylistId] = useState<string>("");
  const [data, setData] = useState<any | undefined>(undefined);
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(true);

  return (
    <UIStateContext.Provider
      value={{
        isLoading,
        setIsLoading,
        currentVidId,
        setCurrentVidId,
        currPlayListId,
        setPlaylistId,
        data,
        setData,
        error,
        setError,
      }}
    >
      {children}
    </UIStateContext.Provider>
  );
};
