const { task, src, dest } = require("gulp");
const config = require("./config");
const order = require("gulp-order");
const concat = require("gulp-concat");
const babel = require("gulp-babel");

// const js = require("./tasks/js")("**/*.js", ["**/*.js"]);

task("js", function () {
    return src("src/**/*.js")
        .pipe(order(["src/**/*.js"], { base: "./" }))
        .pipe(concat("app.js"))
        .pipe(
            babel({
                presets: ["@babel/preset-env"],
            })
        )
        .pipe(dest("./dist/js"));
});

// module.exports = {
//     js,
// };
