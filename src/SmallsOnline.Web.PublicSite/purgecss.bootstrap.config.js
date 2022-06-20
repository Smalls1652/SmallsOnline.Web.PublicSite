module.exports = {
    content: [
        "**/*.razor",
        "../SmallsOnline.Web.Lib/src/SmallsOnline.Web.Lib.Components/**/*.razor",
        "wwwroot/index.html"
    ],
    css: [
        "node_modules/bootstrap/dist/css/bootstrap.css"
    ],
    output: "wwwroot/css/bootstrap/bootstrap.trimmed.css"
  }