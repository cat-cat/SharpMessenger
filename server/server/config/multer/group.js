'use strict';

import multer from 'multer';
import crypto from 'crypto';
import path from 'path';
import mkdirp from 'mkdirp';
import mime from 'mime';

const storage = multer.diskStorage({
    destination: (req, file, cb) => {
        const dir = __base + '/uploads/groups';

        mkdirp(dir, e => cb(e, dir));
    },
    filename: (req, file, cb) => {
        let ext = path.extname(file.originalname);
        ext = ext.length>1 ? ext : "." + mime.extension(file.mimetype);
        crypto.pseudoRandomBytes(16, (err, raw) => {
            cb(null, (err ? undefined : raw.toString('hex') ) + ext);
        });
    },
    _handleFile (req, file, cb) {
	  this.getDestination(req, file, function (err, path) {
	    if (err) return cb(err)

	     console.log("path: " + path)
	  })
	}
});

//var MyCustomStorage = function (opts) {
//	function MyCustomStorage(opts) {
//	  this.getDestination = (opts.destination)
//	  this.getFilename = (opts.filename)
//  }
//}

//MyCustomStorage.prototype._handleFile = function _handleFile (req, file, cb) {
//  this.getDestination(req, file, function (err, path) {
//    if (err) return cb(err)

//     console.log("path: " + path)
//  })
//}

//MyCustomStorage.prototype._removeFile = function _removeFile (req, file, cb) {
//  console.log("group.js: should remove file")
//}

//const storage = MyCustomStorage({
//    destination: (req, file, cb) => {
//        const dir = __base + '/uploads/groups';

//        mkdirp(dir, e => cb(e, dir));
//    },
//    filename: (req, file, cb) => {
//        let ext = path.extname(file.originalname);
//        ext = ext.length>1 ? ext : "." + mime.extension(file.mimetype);
//        crypto.pseudoRandomBytes(16, (err, raw) => {
//            cb(null, (err ? undefined : raw.toString('hex') ) + ext);
//        });
//    }
//});


const formatFilter = (req, file, cb) => {
    (file.mimetype === 'image/jpeg' ||
        file.mimetype === 'image/gif' ||
        file.mimetype === 'image/png' ||
        file.mimetype === 'image/tiff') ? cb(null, true) : cb('Type file is invalid');
};

const upload = multer({ storage: storage, fileFilter: formatFilter, limits: { files: 1, fileSize: 10 * 1024 * 1024 }});

export default upload.single('adimage');
