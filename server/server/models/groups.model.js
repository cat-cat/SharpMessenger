'use strict';

import mongoose from 'mongoose';
import Promise from 'bluebird';
import mongoosePaginate from 'mongoose-paginate';
import QueueService from '../services/queue.service';
import User from './user.model';
import Groups from './groups.model';
import Favorites from './favorites.model';

const Schema = mongoose.Schema;

/**
 * Groups Schema
 */
const GroupsSchema = new Schema({
    name: {
        type: String,
        required: true
    },
    statusMessage: {
        type: String,
    },
    image: {
        type: String
    },
    endAt: {
        type: Date,
        default: () => + new Date() + 24 * 60 * 60 * 1000
    },
    _creator: {
        type: Schema.Types.ObjectId,
        ref: 'User'
    },
    views: {
        type: Number,
        required: true,
        default: 0
    },
    cost: {
        type: Number
    },
    nominal: {
    	type: Number,
    	required: true
    },
    status: {
        type: Number,
        enum: [0 /* users were notificated of this group by ending time */, 1 /* users were notificated of this group by percentage from nominal */, 2 /* users weren't notificated of this group */],
        default: 2,
        required: true
    },
    lastBet: {
    	type: Date
    },
    currentWinner: {
        type: Schema.Types.ObjectId,
        ref: 'User'
    }

}, { collection: 'Groups', timestamps: true, toObject: {
  virtuals: true
  },
  toJSON: {
  virtuals: true 
  } })
GroupsSchema.virtual('ended')
.get(function () {
  return new Date(this.endAt) < new Date() || this.cost >= this.nominal
});

/**
 * Validate creator
 */
GroupsSchema.path('_creator').validate((creator, respond) => {
    	Groups.get({ _creator: creator, endAt: {$gt: new Date()}})
        .then(group => { 
        	return (group == null) ? respond(true) : respond(false)
        })
        .catch(e => {console.log(e); respond(false)})
}, 'Duplicate an active group for path `{PATH}` with value `{VALUE}`');

//     QueueService.group({ id: this._id }, 1430/*10 ten minutes earlier than andAt*/*60*1000);
GroupsSchema.post('save', function () {
    QueueService.group({ id: this._id }, 1430/*10 ten minutes earlier than andAt*/*60*1000);
});

GroupsSchema.statics = {
    /**
     * Get data from one groups
     * @param options {options} - Options for search
     * @returns {Promise.<TResult>|Promise}
     */
    get (options) {
        return this.findOne(options)
        	.populate({ path: '_creator currentWinner', select: 'image name _id' })
            .exec()
            .then((groups) => groups)
            .catch(e => Promise.reject(e))
    },

    /**
     * Get data from all groups
     * @param opts {options} - Options for search Groups
     * @param filter {filter} - Filter
     * @returns {Promise|Promise.<TResult>}
     */
    getAll (opts, filter) {
        return this.find(opts || null, filter || null)
            .exec()
            .then((groups) => groups)
            .catch(e => Promise.reject(e))
    },

    /**
     * Get all data using pagination
     * @param opts {opts} - Options for search
     * @param page {page} - Page
     * @param limit {limit} - Limit
     * @returns {*}
     */
    pagination (opts, filter) {
        return this.paginate(opts || {}, filter || {});
    },

    /**
     * Create Group
     * @param name {name} - Name Group
     * @param image {image} - Image Url
     * @param _creator {_creator} - User
     * @param cost {cost} - Price
     */
    create (data) {
        return new Promise((resolve,reject) => {
            this(data).save((e, group) => (e) ? reject(e) : 

	            Favorites.create({_creator: group._creator, adid: group._id})
	            .then((fav, e) => (e) ? reject(e) : resolve(group))
	            .catch(e=> reject(e)))
        });
    },

    /**
     * Edit data Group
     * @param opts {options} - Options for search Group
     * @param data {data} - Data Group
     * @returns {Promise.<TResult>|Promise}
     */
    edit (opts, data) {
        return this.update(opts, data)
            .exec()
            .then(r => r)
            .catch(e => Promise.reject(e))
    },
};

GroupsSchema.plugin(mongoosePaginate);

/**
 * @typedef GroupsSchema
 */
export default mongoose.model('Groups', GroupsSchema);
