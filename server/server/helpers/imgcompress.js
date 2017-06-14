
const util = require('util');
const Imagemin = require('imagemin');
const imageminPngquant = require('imagemin-pngquant');
const imageminGiflossy = require('imagemin-giflossy');
const imageminJpegoptim = require('imagemin-jpegoptim');

var thumb = require('node-thumbnail').thumb;


var fs = require('fs');

//function imgcompress(file) {
//					Imagemin([file.path], 'tmp/images', {
//						plugins: [
////							imageminJpegoptim({max: 80}),
//							imageminPngquant({quality: '65-80'}),
//							imageminGiflossy({lossy: 80})
//						]
//					}).then(files => {
//						//=> [{data: <Buffer 89 50 4e …>, path: 'build/images/foo.jpg'}, …]
//						//fs.unlinkSync(file.path)
//						fs.rename(files[0].path, file.path, function (err) {
//						  if (err) console.log(err);
//						});						
//					}).catch(e => console.log(e))
//}

function imgcompress(file, dest) {
	thumb({
	  source: file.path, // could be a filename: dest/path/image.jpg
	  destination: __base + dest,
	  concurrency: 1,
	  prefix: '',
	  suffix: '',
	  digest: false,
	  hashingType: 'sha1', // 'sha1', 'md5', 'sha256', 'sha512'
	  width: 250,
	  quiet: false, // if set to 'true', console.log status messages will be supressed
	  overwrite: true,
	  basename: undefined, // basename of the thumbnail. If unset, the name of the source file is used as basename.
	  ignore: false, // Ignore unsupported files in "dest"
	  logger: function(message) {
	    //console.log(message);
	  }
	}, function(files, err, stdout, stderr) {
		if(err != undefined)
	  		console.log('***error creating thumb for file: ' + file.path + ", error: " + util.inspect(err));
	});
}

export default { imgcompress }
