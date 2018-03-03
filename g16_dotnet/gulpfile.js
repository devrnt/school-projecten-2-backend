const gulp = require('gulp'),
    fs = require('fs'),
    sass = require('gulp-sass');

// other content removed

gulp.task('sass', function () {
    return gulp.src('Styles/main.scss')
        .pipe(sass({ outputStyle: 'expanded' }))
        .pipe(gulp.dest('wwwroot/css'));
});