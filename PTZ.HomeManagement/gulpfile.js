﻿/// <binding eforeBuild='Generate_App, Generate_LBD, CopyEmailTemplates' />
"use strict";

var gulp = require("gulp"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    htmlmin = require("gulp-htmlmin"),
    replace = require('gulp-replace'),
    uglify = require("gulp-uglify"),
    merge = require("merge-stream"),
    gulpSequence = require('gulp-sequence'),
    sass = require("gulp-sass"),
    bundleconfig = require("./bundleconfig.json");

var regex = {
    css: /\.css$/,
    html: /\.(html|htm)$/,
    js: /\.js$/
};

//gulp.task("xxx_min:js", function () {
//    var tasks = getBundles(regex.js).map(function (bundle) {
//        return gulp.src(bundle.inputFiles, { base: "." })
//            .pipe(concat(bundle.outputFileName))
//            .pipe(uglify())
//            .pipe(gulp.dest("."));
//    });
//    return merge(tasks);
//});

//gulp.task("xxx_min:css", function () {
//    var tasks = getBundles(regex.css).map(function (bundle) {
//        return gulp.src(bundle.inputFiles, { base: "." })
//            .pipe(concat(bundle.outputFileName))
//            .pipe(cssmin())
//            .pipe(gulp.dest("."));
//    });
//    return merge(tasks);
//});

function getBundles(regexPattern) {
    return bundleconfig.filter(function (bundle) {
        return regexPattern.test(bundle.outputFileName);
    });
}

gulp.task("Generate_LBD", function () {
    gulp.src('lib/lbd/sass/index.scss')
        .pipe(sass())
        .pipe(gulp.dest('wwwroot/lib/lbd/css'));

    gulp.src('lib/**/*')
        .pipe(gulp.dest('wwwroot/lib/'));
});

gulp.task("Generate_App", function () {
    gulp.src('Styles/app.scss')
        .pipe(sass())
        .pipe(gulp.dest('wwwroot/lib/'));

    gulp.src('Scripts/app.js')
        .pipe(gulp.dest('wwwroot/lib/'));

    gulp.src('Media/**/*')
        .pipe(gulp.dest('wwwroot/media/'));
});

gulp.task("CopyEmailTemplates", function () {
    gulp.src('Views/Shared/EmailTemplates/*')
        .pipe(gulp.dest('wwwroot/Views/Shared/EmailTemplates/'));
});