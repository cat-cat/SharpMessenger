'use strict';

import multer from 'multer';
import crypto from 'crypto';
import path from 'path';
import mkdirp from 'mkdirp';
import mime from 'mime';

const storage = multer.diskStorage({
    destination: (req, file, cb) => {
        const dir = __base + '/uploads/users';

        mkdirp(dir, e => cb(e, dir));
    },
    filename: (req, file, cb) => {
        let ext = path.extname(file.originalname);
        ext = ext.length>1 ? ext : "." + mime.extension(file.mimetype);
        crypto.pseudoRandomBytes(16, (err, raw) => {
            cb(null, (err ? undefined : raw.toString('hex') ) + ext);
        });
    }
});

const formatFilter = (req, file, cb) => {
    (file.mimetype === 'image/jpeg' ||
        file.mimetype === 'image/gif' ||
        file.mimetype === 'image/png' ||
        file.mimetype === 'image/tiff') ? cb(null, true) : cb('Type file is invalid');
};

const upload = multer({ storage: storage, fileFilter: formatFilter, limits: { files: 1, fileSize: 10 * 1024 * 1024 } });

export default upload.single('userimage');
