import { Tabs } from "expo-router";
import { Image, ImageBackground, Text, View } from "react-native";
import {
  QueryClient,
  QueryClientProvider,
  useQuery,
} from "@tanstack/react-query";
import {
  UIStateContext,
  UiStateContextType,
  UIStateProvider,
} from "@/context/UIStateContext";
import { useContext } from "react";
import { PlayListItem } from "../_layout";
import { NavigationContainer } from "@react-navigation/native";
import { createBottomTabNavigator } from "@react-navigation/bottom-tabs";
import Library from "./library";
import Index from ".";
import Profile from "./profile";

const libIcon = require("../../assets/icons/menu.png");
const homeIcon = require("../../assets/icons/home.png");
const profIcon = require("../../assets/icons/person.png");

const queryClient = new QueryClient();
const Tab = createBottomTabNavigator();

const TabIcon = ({ iconText, iconName, focused }: any) => {
  return focused ? (
    <>
      <ImageBackground
        source={require("../../assets/images/highlight.png")}
        className="flex flex-row w-full flex-1 min-w-[120px] min-h-16 mt-5 justify-center items-center rounded-full overflow-hidden"
      >
        <Image source={iconName} className="size-5" tintColor="#0e0e0e" />
        <Text className="text-base font-semibold ml-2">{iconText}</Text>
      </ImageBackground>
    </>
  ) : (
    <View className="size-full justify-center items-center mt-4 rounded-full">
      <Image source={iconName} tintColor="#A8B5DB" className="size-7" />
      {/* <Text className="text-[#A8B5DB] text-[8px]">{iconText}</Text> */}
    </View>
  );
};

export default function TabsLayout() {
  const { currentVidId, data, isLoading } = useContext(
    UIStateContext
  ) as UiStateContextType<PlayListItem[]>;

  return (
    //<QueryClientProvider client={queryClient}>

    <Tab.Navigator
      screenOptions={{
        tabBarShowLabel: false,
        tabBarItemStyle: {
          width: "100%",
          height: "100%",
          justifyContent: "center",
          alignItems: "center",
        },
        tabBarStyle: {
          backgroundColor: "#00071b",
          height: 56,
          borderRadius: 50,
          position: "absolute",
          overflow: "hidden",
          marginBottom: 36,
          marginHorizontal: 15,
          borderWidth: 1,
          borderColor: "#0F0D23",
        },
      }}
    >
      <Tab.Screen
        name="index"
        component={Index}
        options={{
          title: "Home",
          headerShown: false,
          tabBarIcon: ({ focused }) => (
            <TabIcon iconText="Home" iconName={homeIcon} focused={focused} />
          ),
        }}
      />
      <Tab.Screen
        name="library"
        component={Library}
        options={{
          title: "Library",
          headerShown: false,
          tabBarIcon: ({ focused }) => (
            <TabIcon iconText="Library" iconName={libIcon} focused={focused} />
          ),
        }}
      />
      <Tab.Screen
        component={Profile}
        name="profile"
        options={{
          title: "Profile",
          headerShown: false,
          tabBarIcon: ({ focused }) => (
            <TabIcon iconText="Profile" iconName={profIcon} focused={focused} />
          ),
        }}
      />
    </Tab.Navigator>

    //</QueryClientProvider>
  );
}
