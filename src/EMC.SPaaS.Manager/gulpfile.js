/// <binding BeforeBuild='clean, min' AfterBuild='min' Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

var webroot = "./wwwroot/";

var paths = {
    controllersjs: webroot + "js/controllers/*.js",
    controllersminJs: webroot + "js/controllers/*.min.js",
    appjs: webroot + "js/*.js",
    appminJs: webroot + "js/*.min.js",
    css: webroot + "css/**/*.css",
    minCss: webroot + "css/**/*.min.css",
    concatcontrollersJsDest: webroot + "js/controllers/controllers.min.js",
    concatappJsDest: webroot + "js/app.min.js",
    concatCssDest: webroot + "css/style.min.css"
};

gulp.task("clean:controllersjs", function (cb) {
    rimraf(paths.concatcontrollersJsDest, cb);
});

gulp.task("clean:appjs", function (cb) {
    rimraf(paths.concatappJsDest, cb);
});



gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:controllersjs", "clean:appjs","clean:css"]);

gulp.task("minControllers:js", function () {
    return gulp.src([paths.controllersjs, "!" + paths.controllersminJs], { base: "." })
        .pipe(concat(paths.concatcontrollersJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});


gulp.task("minApp:js", function () {
    return gulp.src([paths.appjs, "!" + paths.appminJs], { base: "." })
        .pipe(concat(paths.concatappJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["minControllers:js","minApp:js", "min:css"]);
