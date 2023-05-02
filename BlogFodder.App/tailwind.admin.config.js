/** @type {import('tailwindcss').Config} */
module.exports = {
  mode: "jit",
  content: ["Pages/**/*.{razor,cshtml,html}", "Shared/**/*.{razor,cshtml,html}", "Components/**/*.{razor,cshtml,html}"],
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        indigo: {
          1000: '#20255B'
        },
      }
    }
  },
  plugins: [require("@tailwindcss/forms"), require("@tailwindcss/typography")]
}