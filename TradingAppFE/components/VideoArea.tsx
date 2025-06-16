import {
  Dimensions,
  Text,
  TouchableWithoutFeedback,
  View,
  Image,
} from "react-native";
import YoutubeIframe from "react-native-youtube-iframe";
import { WebView } from "react-native-webview";

const screenWidth = Dimensions.get("window").width;
const screenHeight = Dimensions.get("window").height;
const logo = require("../assets/images/logo.png");

interface VideoArea {
  vidId: string;
}

export default function VideoArea({ vidId }: VideoArea) {
  const videoUrl = `https://www.youtube.com/embed/${vidId}?modestbranding=1&rel=0&showinfo=0&controls=1&fs=1&enablejsapi=1&playsinline=1`;
  const injectedJS = `
    (function() {
      // Remove YouTube's share button
      const intervalId = setInterval(() => {
        const shareButton = document.querySelector('.ytp-share-button');
        if (shareButton) {
          shareButton.style.display = 'none'; // Hide share button
        }

        // Hide watch later control
        const watchLater = document.querySelector('.ytp-watch-later-button');
        if (watchLater) {
          watchLater.style.display = 'none';
        }

        // Hide YouTube's other control
        const controls = document.querySelector('.ytp-right-controls');
        if (controls) {
          controls.style.display = 'none'; // Hide other controls
        }

        //  remove video title and any other elements channel title
        const channelTitle = document.querySelector('.ytp-title-channel');
        if (channelTitle) {
          channelTitle.style.display = 'none'; // Hide title
        }
        
        // Optionally, remove video title and any other elements
        const title = document.querySelector('.ytp-title');
        if (title) {
          title.style.display = 'none'; // Hide title
        }

        // Clear interval once elements are hidden
        if (shareButton && controls && title) {
          clearInterval(intervalId);
        }
      }, 100); // Run every 100ms to check for elements
    })();
  `;
  return (
    <View style={{ height: 220 }}>
      <WebView
        javaScriptEnabled={true}
        domStorageEnabled={true}
        scalesPageToFit={true}
        source={{ uri: videoUrl }}
        allowsInlineMediaPlayback={true}
        mediaPlaybackRequiresUserAction={false}
        injectedJavaScriptBeforeContentLoaded={injectedJS} // Use this for better compatibility
        injectedJavaScript={injectedJS}
      />

      {/* Watermark overlay */}
      <View
        style={{
          position: "absolute",
          top: 100,
          left: 10,
          zIndex: 1,
          opacity: 0.6,
        }}
      >
        <Text className="font-extrabold color-black">
          Welcome, UserName || Welcome, UserName
        </Text>
      </View>
      <TouchableWithoutFeedback>
        <View
          className="absolute"
          style={{
            width: 80,
            height: 80,
            top: 10, // Adjust for exact position
            right: 10,
            zIndex: 10,
            backgroundColor: "transparent",
          }}
        />
      </TouchableWithoutFeedback>
      {/* <YoutubeIframe
        height={screenHeight * 0.3}
        videoId={vidId}
        width={screenWidth}
        initialPlayerParams={{
          modestbranding: false,
          rel: false,
          controls: false,
          showClosedCaptions: false,
          preventFullScreen: false,
        }}
      /> */}
      {/* <TouchableWithoutFeedback>
        <View
          className="absolute bg-transparent"
          style={{
            width: 40,
            height: 40,
            top: 10, // Adjust for exact position
            right: 10,
            zIndex: 10,
          }}
        />
      </TouchableWithoutFeedback>
      <View className="absolute bottom-5 right-5 w-15 h-15 bg-black z-10" /> */}
    </View>
  );
}
