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
		  conversationId: {
		    type: Schema.Types.ObjectId,
		    ref: 'Conversation',
		    required: true
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
    }, { collection: 'messages', strict: false, timestamps: true });


MessagesSchema.post('save', function () {
    QueueService({ id: this._id }, this.endAt);
});

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

    search(params) {
	    return this.find(params)
	    .select('createdAt body author')
	    .sort('-createdAt')
	    .populate({
	      path: 'author',
	      select: 'profile.firstName profile.lastName'
	    })
	    .exec(function(err, messages) {
	      if (err) {
	        res.send({ error: err });
	        return next(err);
	      }

	      res.status(200).json({ conversation: messages });
    	});
	}
}

MessagesSchema.plugin(mongoosePaginate);

/**
 * @typedef MessagesSchema
 */
export default mongoose.model('Messages', MessagesSchema);
