'use strict';

import Messages from '../models/messages.model';
import Conversation from '../models/conversation.model';
import upload from '../config/multer/group';
import cfg from '../config/env';

/**
 * Get data from one groups
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next}
 */
//function get (req, res, next) {
//    Groups.get({ _id: req.params._id })
//        .then((user) => res.json(user))
//        .catch((e) => res.json(e))
//}

/**
 * Get data from all groups
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next}
 */
//function getAll (req, res, next) {
//    Groups.getAll()
//        .then((groups) => res.json(groups))
//        .catch((e) => res.json(e))
//}

/**
 *
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next} - Next function
 */
function pagination (req, res, next) {
	new Promise((resolve, reject) => {
		if(req.query.idRoom)
			resolve({Room: req.query.idRoom})
		else if(req.query.idConversation) {
			var q = req.query.idConversation.split(",")
			Conversation.getConversation(q)
			.then(c => resolve({conversationId: c._id}))
			.catch(e => res.json({suscess: false, error: e}))
		}
	})
	.then(o => {
	    Messages.pagination(o, { page: req.query.page, limit: Number(req.query.limit) })
	        .then(r => res.json({userAvatarPrefix:cfg.userAvatarPrefix(), r}))
	})
	.catch(e => res.json({suscess: false, error: e}))
}

function messageStatus (req, res, next) {
	    Messages.status(req.query)
	    .then(data => res.json({success: true, data}))
		.catch(e => res.json({suscess: false, error: e}))
}

export default { pagination, messageStatus };
