import { Stack, useRouter } from "expo-router";
import {
  UIStateContext,
  UiStateContextType,
  UIStateProvider,
} from "@/context/UIStateContext";
import "./global.css";
import { useFetch } from "@/hooks/useFetch";
import { useContext, useEffect, useState } from "react";
import * as SecureStore from "expo-secure-store";
import { SafeAreaView } from "react-native-safe-area-context";
import { ActivityIndicator } from "react-native";
import Config from "react-native-config";
export interface PlayListItem {
  etag: string;
  id: string;
  kind: string;
  snippet: {
    title: string;
    description: string;
    resourceId: {
      videoId: string;
    };
    thumbnails: {
      default: {
        url: string;
      };
    };
  };
}

export interface PlaylistResponse {
  etag: string;
  items: PlayListItem[];
  kind: string;
  pageInfo: {
    resultsPerPage: number;
    totalResults: number;
  };
}

const apiKey = Config.API_KEY;
const playlistId = Config.PLAYLIST_ID;
const playlistUrl = `${Config.PLAYLIST_URL}&playlistId=${playlistId}&key=${apiKey}`;

export default function RootLayout() {
  return (
    <UIStateProvider>
      <AppContent />
    </UIStateProvider>
  );
}

function AppContent() {
  const { loadedData, isdataLoading } = useFetch<PlaylistResponse>(playlistUrl);
  const { setPlaylistId, setCurrentVidId, setData, setIsLoading } = useContext(
    UIStateContext
  ) as UiStateContextType<PlayListItem[]>;

  const [isLoggedIn, setIsLoggedIn] = useState<boolean | null>(null);
  const router = useRouter();

  useEffect(() => {
    const checkLoginStatus = async () => {
      const token = await SecureStore.getItemAsync("userToken");
      if (token) setIsLoggedIn(true);
      else {
        setIsLoggedIn(false);
        router.replace("/auth/signin");
      }
    };

    checkLoginStatus();
  }, []);

  useEffect(() => {
    if (loadedData) {
      setData(loadedData.items);
      setCurrentVidId(loadedData.items[0].snippet.resourceId.videoId);
      setPlaylistId(playlistId);
      setIsLoading(isdataLoading);
    }
  }, [loadedData, isdataLoading]);

  return (
    <Stack screenOptions={{ headerShown: false }}>
      <Stack.Screen name="(tabs)" />
      <Stack.Screen name="auth/signin" />
    </Stack>
  );
}
