var gulp = require('gulp');
var concat = require('gulp-concat');
var fs = require('fs');

var files = fs.readdirSync('./src/');

files = files.sort(function (a, b) {
    if (a.match(/\d+\./) && b.match(/\d+\./)) {
        var aN = a.match(/\d+\./)[0];
        var bN = b.match(/\d+\./)[0];
        return aN - bN;
    }
});

files = files.map(f => './src/' + f);

console.log(files);

gulp.task('default', function () {
    return gulp.src(files)
        .pipe(concat('migrations.sql'))
        .pipe(gulp.dest('./dist/'))
		.pipe(gulp.dest('../Arrba/Data/'));
});


