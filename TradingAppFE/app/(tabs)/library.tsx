import { useQuery } from "@tanstack/react-query";
import { useContext, useEffect } from "react";
import {
  ActivityIndicator,
  FlatList,
  Text,
  View,
  Image,
  TouchableOpacity,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { UIStateContext, UiStateContextType } from "@/context/UIStateContext";
import { PlayListItem } from "../_layout";
import { Link } from "expo-router";

export default function Library() {
  const { data, error, isLoading, setCurrentVidId } = useContext(
    UIStateContext
  ) as UiStateContextType<PlayListItem[]>;

  const handleVideoSelection = (videoId: string) => {
    setCurrentVidId(videoId);
  };

  // const { data, isLoading, error } = useQuery({
  //   queryKey: ["youtubeplaylist", playlistId],
  //   queryFn: fetchPlaylist,
  //   staleTime: 24 * (60 * 60 * 1000),
  // });

  // useEffect(() => {
  //   if (data?.items?.length > 0) {
  //     if (!currPlayListId) setPlaylistId(playlistId);
  //     if (!currentVidId)
  //       setCurrentVidId(data?.items[0]?.snippet?.resourceId?.videoId);
  //   }
  // }, [data, setPlaylistId, setCurrentVidId]);

  if (isLoading) {
    return (
      <SafeAreaView className="flex-1 justify-center items-center">
        <ActivityIndicator size="large" color="#0000ff" />
      </SafeAreaView>
    );
  }
  if (error)
    return (
      <SafeAreaView className="flex-1 justify-center items-center">
        <Text style={{ color: "red" }}>Error: {error}</Text>
      </SafeAreaView>
    );

  return (
    <SafeAreaView className="flex-1 p-2 bg-primary-black">
      <Text className="text-secondary-light text-2xl my-2 mx-2">
        Welcome to Library.
      </Text>
      {data && data?.length > 0 ? (
        <FlatList
          data={data}
          keyExtractor={(item) => item.snippet.resourceId.videoId}
          renderItem={({ item }) => (
            <TouchableOpacity
              className="flex-1 flex-row justify-between my-2 py-4 bg-primary-yellow rounded-[10px]"
              onPress={() =>
                handleVideoSelection(item.snippet.resourceId.videoId)
              }
            >
              <Image
                className="min-w-[80px] min-h-[50px] mx-2 rounded-[10px]"
                source={{ uri: item.snippet.thumbnails.default.url }}
              />
              <View className="flex-1">
                <Text className="text-primary-black-700 text-sm">
                  {item.snippet.title}
                </Text>
              </View>
            </TouchableOpacity>
          )}
        />
      ) : (
        <Text>No videos found in this playlist</Text>
      )}
    </SafeAreaView>
  );
}
