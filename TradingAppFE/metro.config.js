const { getDefaultConfig } = require("expo/metro-config");
const { withNativeWind } = require("nativewind/metro");

const config = getDefaultConfig(__dirname);

// Make sure Metro knows about image types
config.resolver.assetExts.push("png", "jpg", "jpeg", "svg");

module.exports = withNativeWind(config, { input: "./app/global.css" });
