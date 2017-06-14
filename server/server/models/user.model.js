'use strict';

import mongoose from 'mongoose';
import jwt from 'jwt-simple';
import cfg from '../config/env';

/**
 * User Schema
 */
const UserSchema = new mongoose.Schema({
	email: {type: String, trim: true, index: { unique: false }},
	checkCode: String,
	checkCodeUpdate: Date,
    name: String,
    phones: [{
        type: String
    }],
    push_ids: [{
	    type: String
    }],
    image: String,
    geo: {
        lat: String,
        lng: String
    },
    status: {
        type: Number,
        required: true,
        enum: [0, 1],
        default: 0
    },
    balance: {
    	type: Number,
    	required: false,
    	default: 0
	},
    token: { type: String, required: true }
}, { collection: 'User', strict: false, timestamps:true });

/**
 * Validate data user
 */
UserSchema.pre('validate', function(next) {
    this.token = this.makeJwt(this._id);
    next();
});

UserSchema.statics = {
    /**
     * Get data from one user
     * @param options {options} - Options for search user
     * @param filter {filter} - Filter for search
     * @returns {Promise|Promise.<TResult>}
     */
    get (options, filter) {
        return this.findOne(options, filter || null)
            .exec()
            .then((user) => {
                return user;
            })
            .catch((e) => {
                return Promise.reject(e);
            })
    },

    /**
     * Get data from one user
     * @param options {options} - Options for search user
     * @param filter {filter} - Filter for search
     * @returns {Promise|Promise.<TResult>}
     */
    getAll (options, filter) {
        return this.find(options, filter || null)
            .exec()
            .then((users) => {
                return users;
            })
            .catch((e) => {
                return Promise.reject(e);
            })
    },

    /**
     * Create user
     * @param data {data} - Data user
     * @returns {Promise}
     */
    create (data) {
        return new Promise((resolve,reject) => {
	            this(data).save((err, user) => {
	                if(err) 
	                	reject(err);

	                resolve(user)	          
            })
        });
    },

    addPushId(userId, push_id) {
        return new Promise((resolve,reject) => {
	        this.update({_id:userId}, {$push: {push_ids: push_id}})
	        .exec()
	        .then(user => resolve(user))
	        .catch(e => reject(e))
        });
    },

    /**
     * Edit data user
     * @param opts {options} - Options for search
     * @param data {data} - Data user
     * @returns {Promise.<TResult>|Promise}
     */
    edit (opts, data) {
        return this.update(opts, data)
            .exec()
            .then(user => user)
            .catch(e => Promise.reject(e))
    }
};

/**
 * Methods
 */
UserSchema.methods = {
    /**
     * Generated JWT token
     * @param _id Unique ID user
     * @returns {String} - JWT token
     */
    makeJwt: (_id) => {
        return jwt.encode({ _id: _id }, cfg.jwt);
    }
};

/**
 * @typedef UserSchema
 */
export default mongoose.model('User', UserSchema);
