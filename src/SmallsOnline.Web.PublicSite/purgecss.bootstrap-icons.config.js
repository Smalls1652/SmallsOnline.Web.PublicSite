module.exports = {
    content: [
        "**/*.razor",
        "../SmallsOnline.Web.Lib/src/SmallsOnline.Web.Lib.Components/**/*.razor",
        "wwwroot/index.html"
    ],
    css: [
        "node_modules/bootstrap-icons/font/bootstrap-icons.css"
    ],
    safelist: [
        /terminal-fill$/,
        /filetype-cs$/,
        /globe$/,
        /file-earmark-code-fill$/,
        /phone-fill$/
    ],
    output: "wwwroot/css/bootstrap-icons/bootstrap-icons.trimmed.css"
  }