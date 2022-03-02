const { src, dest } = require("gulp");
const order = require("gulp-order");
const concat = require("gulp-concat");
const babel = require("gulp-babel");

/**
 * Build javascript files
 * @param {string} filesJs
 * @param {string[]} filesJsOrder
 * @param {string?} serverPath
 */
module.exports = function (filesJs, filesJsOrder, serverPath) {
    return src(filesJs)
        .pipe(order(filesJsOrder, { base: "./" }))
        .pipe(concat("app.js"))
        .pipe(
            babel({
                presets: ["@babel/preset-env"],
            })
        )
        .pipe(dest("./dist/js"));
    // .pipe(dest(serverPath + "js"));
};
