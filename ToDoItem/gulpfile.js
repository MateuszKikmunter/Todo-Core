"use strict";

var gulp = require("gulp"),
    uglify = require("gulp-uglify"),
    babel = require("gulp-babel"),
    bundleconfig = require("./bundleconfig.json");

var regex = {
    css: /\.css$/,
    html: /\.(html|htm)$/,
    js: /\.js$/
};

gulp.task("min", ["min:js", "min:css", "min:html"]);

const dirs = {
    src: "wwwroot/js/*.js",
    dest: "wwwroot/js/dist/"
}

gulp.task("min:js", () => {
    return gulp.src(dirs.src)
        .pipe(babel({ presets: ['es2015'] }))
        .pipe(uglify())
        .pipe(gulp.dest(dirs.dest));
});

gulp.task("watch",
    () => {
        gulp.watch(dirs.src, ["build"]);
    });

gulp.task("default", ["watch"]);