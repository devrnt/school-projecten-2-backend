const gulp = require('gulp'),
    fs = require('fs'),
    sass = require('gulp-sass');

gulp.task('sass', function () {
    return gulp.src('Styles/main.scss')
        .pipe(sass({ outputStyle: 'expanded' }))
        .pipe(gulp.dest('wwwroot/css'));
});

// onchange exec task 'sass'
gulp.task('sass:watch', function () {
    gulp.watch('Styles/*.scss', ['sass'])
});