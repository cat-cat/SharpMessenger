'use strict';

import User from '../models/user.model';
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
    User.get({email: req.query.email.toLowerCase(), checkCode: req.query.check_code, checkCodeUpdate: {$gt: new Date().getTime()- (1000*600)/*10 min.*/}})
        .then((user, error) => {
        	if(user != null)
        		return {success: true, user: user}
        	else
        		return {success: false, error:'Code checking failed'}
        })
        .then(upd => res.send(upd)) 
        .catch((e) => res.send({success: false, error:e}))
}


function send (req, res, next) {
	var sid = shortid.generate();
	transporter.sendMail({
	    from: 'rustote@yandex.ru',
	    to: req.query.email,
	    subject: 'Group verification code',
	    html: '<html><body>your password: <b>' + sid + '</b></body></html>'  ,
  	}, function(err, reply) {
	    //console.dir(reply);

  		if(err)
	    	console.log(err && err.stack);

		User.get({email: req.query.email.toLowerCase()})
		.then(user => { 
			if(user!=null) {
				return User.edit({email: req.query.email.toLowerCase()}, {checkCode: sid, checkCodeUpdate: new Date()})
				.then(upd => { return {success: true, result: upd}})
			}
			else 
				return {success: false, error:'No user with such email'}
		})
	     
        .then(upd => res.send(upd))
        .catch(e => res.json({success: false, error: e}))
	    
	});
}


function add (req, res, next) {
	var sid = shortid.generate();
	transporter.sendMail({
	    from: 'rustote@yandex.ru',
	    to: req.query.email,
	    subject: 'Group verification code',
	    html: '<html><body>your password: <b>' + sid + '</b></body></html>'  ,
  	}, function(err, reply) {
	    //console.dir(reply);

  		if(err)
	    	console.log(err && err.stack);

		User.get({email: req.query.email.toLowerCase()})
		.then(user => {
			if(user==null) {
					return User.edit({_id:req.user._id}, {email: req.query.email.toLowerCase(), checkCode: sid, checkCodeUpdate: new Date()})
					.then(upd => {return {success: true, result: upd}})
			}
			else
				return {success: false, error:'email already exists'}
		})
	     
        .then(upd => res.send(upd))
        .catch(e => res.json({success: false, error: e}))
	});
}

export default { check, send, add };
