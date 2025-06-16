import { useState } from "react";
import { Text, TextInput, View, Image, TouchableOpacity } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { getDocs, setDoc, where } from "firebase/firestore";
import { initializeApp } from "firebase/app";
import { getFirestore, query, collection } from "firebase/firestore";
import * as SecureStore from "expo-secure-store";
import jwt from "jsonwebtoken";

interface User {
  email: string;
  password: string;
}

const firebaseConfig = {
  apiKey: "AIzaSyBDTuZkhh6Fd2D2WSZud9j3ze0QEFQHKOo",
  authDomain: "mytradingapp-1cf73.firebaseapp.com",
  projectId: "mytradingapp-1cf73",
  storageBucket: "mytradingapp-1cf73.firebasestorage.app",
  messagingSenderId: "478247382470",
  appId: "1:478247382470:web:5136e73320a7c5261196c7",
  measurementId: "G-3KJE7JF15P",
};

const app = initializeApp(firebaseConfig);
const db = getFirestore(app);

const companyLogo = require("../../assets/images/logo.png");

export default function SignIn() {
  const [formData, setFormData] = useState<User>({
    email: "",
    password: "",
  });

  const [error, setError] = useState<string>("");

  const handleChange = (elem: string, value: string) => {
    setFormData({
      ...formData,
      [elem]: value,
    });
  };

  const generateToken = (userData: User): string => {
    const payload = { ...userData };
    const secretKey: string = "tutorial_a_very_long_secret_key_trading_app";
    const options: jwt.SignOptions = { expiresIn: "1h" };
    const jwtToken: string = jwt.sign({ data: payload }, secretKey, options);
    return jwtToken;
  };

  const setTokenToKeyStore = async (token: string) => {
    try {
      await SecureStore.setItemAsync("userToken", token);
      console.log("Token stored successfully!");
    } catch (error) {
      console.log("Error Happened while storing tokens!");
    }
  };

  const handleFormSubmit = async () => {
    if (formData.email && formData.password) {
      try {
        const userQuery = query(
          collection(db, "users"),
          where("email", "==", formData.email)
        );
        const querySnapshot = await getDocs(userQuery);
        if (querySnapshot.empty) {
          setError("No record with such email exists!");
          return;
        }

        querySnapshot.forEach((doc) => {
          const userData = doc.data() as User;
          if (userData.password === formData.password) {
            //generate jwt token
            const token = generateToken(userData);
            //set the keystorevalue
            setTokenToKeyStore(token);
            setError("");
          }
        });
      } catch (error) {
        console.error("Error retrieving user:", error);
        setError("Error happened.Data could not be retrieved!");
      }
    }
  };

  return (
    <SafeAreaView className="flex-1 bg-primary-black">
      <View>
        <Image source={companyLogo} className="h-40 w-40" />
        <Text className="my-4 ml-4 color-slate-50 text-xl font-extrabold">
          Log in to XTrading
        </Text>

        <View className="flex my-5">
          <Text className="ml-5 my-3 color-slate-50">Email</Text>
          <TextInput
            className="border rounded-md w-10/12 ml-5 border-primary-yellow color-slate-200"
            value={formData.email}
            onChangeText={(text) => handleChange("email", text)}
          />
        </View>
        <View className="flex">
          <Text className="ml-5 my-3 color-slate-50">Password</Text>
          <TextInput
            className="border border-primary-yellow rounded-md w-10/12 ml-5 color-slate-200"
            value={formData.password}
            secureTextEntry={true}
            onChangeText={(text) => handleChange("password", text)}
          />
        </View>
        <TouchableOpacity
          className="items-center p-4 w-10/12 rounded-md ml-5 my-10 bg-primary-yellow"
          onPress={handleFormSubmit}
        >
          <Text className="font-semibold">LogIn</Text>
        </TouchableOpacity>
        <Text className="color-slate-300">{JSON.stringify(formData)}</Text>
      </View>
    </SafeAreaView>
  );
}
