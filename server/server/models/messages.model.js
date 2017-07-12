'use strict';

import mongoose from 'mongoose';
import Promise from 'bluebird';
import mongoosePaginate from 'mongoose-paginate';
import QueueService from '../services/queue.service';
import User from './user.model';

const Schema = mongoose.Schema;

/**
 * Messages Schema
 */
const MessagesSchema = new Schema({
		replyQuote: {
			type: String
		},
		replyId: {
	        type: Schema.Types.ObjectId,
	        ref: 'messages'
		},
		replyGuid: {
	        type: String
		},
		messageEdited: {
			type: Boolean,
			default: false
		},
		status: {
			type: Number,
			default: 1 /* - delivered, 2 - read, 3 - deleted */
		},
		guid: {
			type: String,
			index: true
		},
		  conversationId: {
		    type: Schema.Types.ObjectId,
		    ref: 'Conversation'
		  },
	     Room: {
	         type: String,
	         required: false
	     },
	     Name: {
	         type: String,
	         required: false
	     },
	    Message: {
	        type: String,
	        required: true
	    },
	    _creator: {
	        type: Schema.Types.ObjectId,
	        ref: 'User',
	        required: true
	    },
	    _to: {
	        type: Schema.Types.ObjectId,
	        ref: 'User',
	        required: true
	    },
    }, { collection: 'messages', strict: false, timestamps: true }
);


MessagesSchema.statics = {
    /**
     * Get all data using pagination
     * @param opts {opts} - Options for search
     * @param page {page} - Page
     * @param limit {limit} - Limit
     * @returns {*}
     */
    pagination (opts, { page, limit }) {
        return this.paginate(opts || {}, { page: page, limit: limit, populate: { path: '_creator _to', select: 'image name _id'}, sort: 'createdAt' }, (e, r) =>
            (e) ? Promise.reject(e) : Promise.resolve(r));
    },

	status(params) {
		 return this.findOne({ guid: { $in: params.guids.split(',') } }, function (err, doc) {
		 	if(doc == null) // not found, probably new message didn't saved yet on server
		 		return

		 	if(params.read != undefined)
		 		doc.status = 2
		 	else if(params.deleted != undefined)
		 		doc.status = 3

			// not exclusive condition in relation to status	
		 	if(params.edited != undefined) {
		 		doc.messageEdited = true
		 		doc.Message = params.message
		 	}

		 	doc.save()
		 }) 
	},

	insert(data) {
		return this.create(data, function (err) {
		    if (err) 
		    	return console.log(err);
	    })
	}
}

MessagesSchema.plugin(mongoosePaginate);

/**
 * @typedef MessagesSchema
 */
export default mongoose.model('Messages', MessagesSchema);
