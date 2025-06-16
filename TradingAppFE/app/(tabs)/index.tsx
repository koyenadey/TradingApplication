import VideoArea from "@/components/VideoArea";
import VideoDescription from "@/components/VideoDescription";
import { UIStateContext, UiStateContextType } from "@/context/UIStateContext";
import { useContext, useEffect, useState } from "react";
import {
  Text,
  View,
  Image,
  ScrollView,
  Dimensions,
  Button,
  ActivityIndicator,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { PlayListItem } from "../_layout";

const screenWidth = Dimensions.get("window").width;

export default function Index() {
  const { currentVidId, data, isLoading } = useContext(
    UIStateContext
  ) as UiStateContextType<PlayListItem[]>;
  // useEffect(() => {
  //   if (!isLoading && data) {
  //     console.log("Data is loaded: ", data);

  //     // Handle the loaded data here
  //   } else console.log("Outside isLoading");
  // }, [isLoading, data]);

  let videoItem: PlayListItem | undefined;
  if (data && data?.length > 0) {
    videoItem = data.find(
      (item) => item.snippet.resourceId.videoId === currentVidId
    );
  }

  if (isLoading) {
    return (
      <SafeAreaView className="flex-1 justify-center items-center">
        <ActivityIndicator size="large" color="#0000ff" />
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView className="flex-1 bg-black">
      <ScrollView
        className="flex-1 px-2 pb-42 text-white"
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{ minHeight: "100%", paddingBottom: 10 }}
      >
        <VideoArea vidId={currentVidId} />
        <VideoDescription
          description={videoItem?.snippet.description ?? ""}
          title={videoItem?.snippet.title ?? "Sample Video"}
        />
      </ScrollView>
    </SafeAreaView>
  );
}
