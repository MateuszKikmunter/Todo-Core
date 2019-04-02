"use strict";

var gulp = require("gulp"),
   // concat = require("gulp-concat"),
  //  cssmin = require("gulp-cssmin"),
 //   htmlmin = require("gulp-htmlmin"),
    uglify = require("gulp-uglify"),
    babel = require("gulp-babel"),
   // uglify = require("gulp-uglify-es").default,
   // merge = require("merge-stream"),
  //  del = require("del"),
    bundleconfig = require("./bundleconfig.json");

//import gulp from "gulp";
//import uglify from "gulp-uglify";
//import babel from "gulp-babel";

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

gulp.task("min:css", function () {
    var tasks = getBundles(regex.css).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(cssmin())
            .pipe(gulp.dest("."));
    });
    return merge(tasks);
});

gulp.task("min:html", function () {
    var tasks = getBundles(regex.html).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(htmlmin({ collapseWhitespace: true, minifyCSS: true, minifyJS: true }))
            .pipe(gulp.dest("."));
    });
    return merge(tasks);
});

gulp.task("clean", function () {
    var files = bundleconfig.map(function (bundle) {
        return bundle.outputFileName;
    });

    return del(files);
});

gulp.task("watch", function () {
    getBundles(regex.js).forEach(function (bundle) {
        gulp.watch(bundle.inputFiles, ["min:js"]);
    });

    getBundles(regex.css).forEach(function (bundle) {
        gulp.watch(bundle.inputFiles, ["min:css"]);
    });

    getBundles(regex.html).forEach(function (bundle) {
        gulp.watch(bundle.inputFiles, ["min:html"]);
    });
});

function getBundles(regexPattern) {
    return bundleconfig.filter(function (bundle) {
        return regexPattern.test(bundle.outputFileName);
    });
}