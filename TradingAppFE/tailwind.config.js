/** @type {import('tailwindcss').Config} */
module.exports = {
  // NOTE: Update this to include the paths to all of your component files.
  content: ["./app/**/*.{js,jsx,ts,tsx}"],
  presets: [require("nativewind/preset")],
  theme: {
    extend: {
      colors: {
        primary: {
          black: "#00071b",
          yellow: "#d6a80f",
        },
        secondary: {
          light: "#e0e2ea",
          dark: "#B1B2B5",
        },
      },
    },
  },
  plugins: [],
};
