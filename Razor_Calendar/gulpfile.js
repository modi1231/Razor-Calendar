/// <binding BeforeBuild='_main_dev' />
 
var gulp = require('gulp');
var del = require("del");
var concat = require('gulp-concat');
var sass = require('gulp-sass');

//gulp.task('_main_dev', ['clean', 'lib_dev', 'sass']);
//gulp.task('clean', function ()
//{
//    del(["wwwroot/js/*.js", "wwwroot/css/*.css", "wwwroot/lib/*.*"]);
//});

//gulp.task('_main_dev', ['clean', 'jquery', 'bootstrap_css']);

gulp.task('clean', function ()
{
    del(["wwwroot/js/*.js", "wwwroot/css/*.css"]);/*, "wwwroot/lib/*.*"*/
});


//gulp.task('lib_dev', function ()
//{
//    gulp.src(["node_modules/bootstrap/dist/css/bootstrap.css"])
//        .pipe(gulp.dest("wwwroot/lib"));

//});

//gulp.task('sass', function ()
//{
//    gulp.src('scss/**/*.scss')
//        .pipe(sass().on('error', sass.logError))
//        .pipe(concat('site.css'))
//        .pipe(gulp.dest('wwwroot/css'));
//});

gulp.task('jquery', function ()
{
    gulp.src(["node_modules/jquery/dist/jquery.js"])
        .pipe(gulp.dest("wwwroot/js"));
});

gulp.task('bootstrap_css', function ()
{
    gulp.src(["node_modules/bootstrap/dist/css/bootstrap.css"])
        .pipe(gulp.dest("wwwroot/css"));
});

