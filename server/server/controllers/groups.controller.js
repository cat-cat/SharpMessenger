'use strict';

import Groups from '../models/groups.model';
import upload from '../config/multer/group';
import cfg from '../config/env';
import imghlp from '../helpers/imgcompress';
import QueueService from '../services/queue.service';

/**
 * Get data from one groups
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next}
 */
function get (req, res, next) {
    Groups.get({ _id: req.query._id })
        .then((group) => res.json({success: true, error: null, data: {filePrefix:cfg.filePrefix(), userAvatarPrefix:cfg.userAvatarPrefix(), groups:group}}))
        .catch((e) => res.json({success: false, error: e, data: {}}))
}

/**
 * Get data from all groups
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next}
 */
function getAll (req, res, next) {
    Groups.getAll()
        .then((groups) => res.json(groups))
        .catch((e) => res.json(e))
}

/**
 *
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next} - Next function
 */
function pagination (req, res, next) {
	var q = req.query
	if(req.query.endAt)
		Object.assign(q, {sort: "endAt"})
	else if(req.query.nominal)
		Object.assign(q, {sort:	"-nominal"})
	else if(req.query.cost)
		Object.assign(q, {sort: "-cost"})
	else if(req.query.lastBet)
		Object.assign(q, {sort: "-lastBet"})

	Object.assign(q, {populate: { path: '_creator currentWinner', select: 'name image _id'}})
		
		// { page: req.query.page, limit: req.query.limit }
	var o = req.query.endAt || req.query.lastBet ? {endAt: {$gt: new Date()}} : {}
    Groups.pagination(o, q)
    .then(r => res.json({filePrefix:cfg.filePrefix(), groups:r}))
        .catch(e => res.json({success: false}))
}

/**
 * Create group
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next} - Next function
 */
function create (req, res, next) {
    upload(req, res, function (e) {
		if(e) {
			console.log(e)
			return res.json({success: false, error: e})
		}

    	var group = req.body;

    	// check that nominal is not less than 10
    	if(Number(group.nominal) < 10) {
    		// at client AdvertCreate.cs: 104 - DependencyService.Get<IExceptionHandler>().ShowMessage(Response.ResponseObject["error"]["errors"]["_creator"]["message"].ToString());
    		res.json({success:false, error :{ errors: {_creator: {message: "Nominal can't be less than 10"}}}})
    		return
		}

    	Object.assign(group, { _creator: req.user._id });
    	group = (req.file) ? Object.assign(group, {image: req.file.filename }) : group;

        (e) ? res.json({ success: false, message: e }) :
            Groups.create(group)
                .then((group) => res.json({success: true, _id: group._id}))
                .catch(e => {
                    res.json({success: false, error: e})
                })

        if(req.file)
        	imghlp.imgcompress(req.file, '/uploads/groups')
    })
}

/**
 * Get the Groups history
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next} - Next function
 */
function history (req, res, next) {
    Groups.getAll({ '_creator': req.user._id})
    .then(groups => res.json(groups))
    .catch(e => res.json({success: false, message: e}))
}

/**
 * Edit Groups
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next} - Next function
 */
function edit (req, res, next) {
	upload(req, res, function (e) {
		if(e) {
			console.log(e)
			return res.json({success: false, message: e})
		}

        if(req.file)
        	imghlp.imgcompress(req.file, '/uploads/groups')

	    let group = req.body;
	    Groups.edit({_id: group.id}, group)
	    .then(groups => res.json(groups))
	    .catch(e => res.json({success: false, message: e}))
    })
}

/**
 * Edit Groups
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next} - Next function
 */
function remove (req, res, next) {	
	Groups.get({_id: req.query.id, lastBet: {$exists: true}})
	.then(group => {
		if(group != null) { // there's bets for this group
			if((new Date(group.endAt).getTime() - new Date().getTime()) < (1000*600)) { // if less than 10 min. until group over
				res.json({success: false, message: 'group is ending'})					
			}
			else if(req.query.delete) {
				res.json({success: false, message: 'bets available'})
			} else {
				QueueService.remove(req.query.id);
				res.json({success: true, message: req.query.id})
			}
		} else { // no bets for this group
			if(req.query.delete) {
				Groups.edit({_id: req.query.id}, {$set: {endAt: new Date()}}) // set endAt to current time, so ended field will return true
					.then(r => { 
						QueueService.remove(req.query.id)
						res.json({success: true, message: r})
					})
					.catch(e => res.json({success: false, message: e}))
			} else {
				res.json({success: false, message: 'remove this group?'})
			}

		}	
	})
	.catch(e => res.json({success: false, message: e}))
}

export default { get, getAll, pagination, create, history, edit, remove };
