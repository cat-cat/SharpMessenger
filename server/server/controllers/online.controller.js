'use strict';

import socketService from '../services/socket.service';

const nodemailer = require('nodemailer');
//var transporter = nodemailer.createTransport({
//        service: 'gmail',
//        auth: {
//            user: 'rustote@gmail.com', // Your email id
//            pass: '!3351aa99' // Your password
//        }
//    });

var transporter = nodemailer.createTransport({direct:true,
    host: 'smtp.yandex.ru',
    port: 465,
    auth: { 
        user: 'rustote@yandex.ru', 
        pass: '!3351aa' },
    secure: true
});

var shortid = require('shortid');

//check_code(code) {
//	code = db.getCode(email)
//	if(code.expiration >= 10 min)
//		res.(old code)
//	else if(!checkCode(code))
//		res.(invalid code)
//	else // ok
//		res.(user)
	
//}

function check (req, res, next) {
	res.send({success: true, data:socketService.getOnline(req.query.ids)})
}

export default { check };
