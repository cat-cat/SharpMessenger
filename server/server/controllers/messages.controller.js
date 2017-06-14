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
	    Messages.pagination(o, { page: req.query.page, limit: req.query.limit })
	        .then(r => res.json({userAvatarPrefix:cfg.userAvatarPrefix(), r}))
	})
	.catch(e => res.json({suscess: false, error: e}))
}

/**
 * Create group
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next} - Next function
 */
//function create (req, res, next) {
//    upload(req, res, (e) => {
//
//        let group = req.body;
//
//        (e) ? res.json({ success: false, message: e }) :
//            Groups.create(Object.assign(group, { _creator: req.user._id, image: req.file.filename }))
//                .then((group) => res.json({success: true, _id: group._id}))
//                .catch(e => {
//                    console.log(e);
//                    res.json({success: false})
//                })
//    });
//}

/**
 * Get the Groups history
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next} - Next function
 */
//function history (req, res, next) {
//    Groups.getAll({ '_ creator': req.user._id})
//        .then(groups => res.json(groups))
//        .catch(e => res.json({success: false}))
//}

export default { pagination };
