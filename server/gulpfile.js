'use strict';

const gulp = require('gulp');
const babel = require('gulp-babel');
const eslint = require('gulp-eslint');
const gulpUtil = require('gulp-util');
const sourcemaps = require('gulp-sourcemaps');
const clean = require('gulp-clean');
const nodemon = require('gulp-nodemon');

gulp.task('lint', () => {
    return gulp.src(['**/*.js','!node_modules/**', '!gulpfile.js'])
        .pipe(eslint())
        .pipe(eslint.format())
        .pipe(eslint.failAfterError())
        .on('error', error => {
            gulpUtil.log('Stream Exiting With Error: ' + error.message);
        });
});

gulp.task('prebuild', () => {
    return gulp.src('dist/uploads/**/*.*', {base: './dist/'})
    	.pipe(gulp.dest('./'))
});

gulp.task('clean', ['prebuild'], () => {
    return gulp.src('dist', {read: false})
        .pipe(clean());
});

gulp.task('build', ['clean'], () => {
    gulp.src('server/**/*.js')
        .pipe(sourcemaps.init())
        .pipe(babel({
            presets: ['es2015']
        }))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('dist'))
}); 

gulp.task('serve', ['build'], () => {
    return gulp.src('uploads/**/*.*', {base: './'})
    	.pipe(gulp.dest('dist'))
});
