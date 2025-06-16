import { UIStateContext } from "@/context/UIStateContext";
import { useContext } from "react";
import { View, Text, ScrollView, Dimensions } from "react-native";

const screenHeight = Dimensions.get("window").height;

interface VideoDescType {
  description: string;
  title: string;
}

export default function VideoDescription({
  description,
  title,
}: VideoDescType) {
  return (
    <View className="mb-5">
      <Text className="text-white text-xl mt-4 font-extrabold">{title}</Text>
      <ScrollView style={{ height: screenHeight * 0.5, marginTop: 10 }}>
        <Text className="text-white text-sm">{description}</Text>
      </ScrollView>
    </View>
  );
}
