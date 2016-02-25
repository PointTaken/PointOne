var gulp = require('gulp'),
    ios = require('gulp-cordova-build-ios');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
 
gulp.task('default', ['build']);
gulp.task('uglify-and-combine', function () {
    return gulp.src('www/scripts/*.js').pipe(concat('combined.js')).pipe(uglify()).pipe(gulp.dest('min/scripts'));
});
gulp.task('build', function() {
    return gulp.src('www')
        .pipe(ios());
});