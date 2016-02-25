var gulp = require("gulp"),
    cordova = require("cordova-lib").cordova;       // Load gulp
var uglify = require("gulp-uglify"); // Load gulp-uglify
var concat = require("gulp-concat"); // Load gulp-concat

gulp.task("combine-and-uglify", function () {
    return gulp.src('www/scripts/*.js')
        .pipe(concat('combined.js'))
        .pipe(uglify())
        .pipe(gulp.dest('min/scripts'));
});


gulp.task("default", function (callback) {
    cordova.build({
        "platforms": ["android"],
        "options": {
            argv: ["--release", "--gradleArg=--no-daemon"]
        }
    }, callback);
});