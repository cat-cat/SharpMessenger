'use strict';

import mongoose from 'mongoose';
import crypto from 'crypto';
import Promise from 'bluebird';

/**
 * Admin Schema
 */
const AdminSchema = new mongoose.Schema({
    email: { type: String, required: true, unique: true },
    hashed_password: { type: String, required: true },
    salt: { type: String, required: true }
}, { collection: 'Admin' });

/**
 * Virtuals methods
 */
AdminSchema
    .virtual('password')
    .set(function(password) {
        this._password = password;
        this.salt = this.makeSalt();
        this.hashed_password = this.encryptPassword(password);
    })
    .get(function() { return this._password });

AdminSchema.statics = {
    /**
     * Get data from one user
     * @param options
     * @returns {Promise.<TResult>|Promise}
     */
    get (options) {
        return this.findOne(options)
            .exec()
            .then((user) => {
                return user;
            })
            .catch((e) => {
                return Promise.reject(e);
            })
    },

    /**
     * Create user
     * @param data {data} - Data user (email, password)
     * @returns {Promise.<TResult>|Promise}
     */
    create (data) {
        return new Promise((resolve,reject) => {
            new this(data).save((err) => {
                if(err) return reject(err);
                resolve()
            })
        });
    }
};

/**
 * Methods
 */
AdminSchema.methods = {

    /**
     * Authenticate - check if the passwords are the same
     *
     * @param {String} plainText
     * @return {Boolean}
     * @api public
     */
    authenticate: function (plainText) {
        return this.encryptPassword(plainText) === this.hashed_password;
    },

    /**
     * Make salt
     *
     * @return {String}
     * @api public
     */
    makeSalt: function () {
        return Math.round((new Date().valueOf() * Math.random())) + '';
    },

    /**
     * Encrypt password
     *
     * @param {String} password
     * @return {String}
     * @api public
     */
    encryptPassword: function (password) {
        if (!password) return '';
            return crypto.createHmac('sha1', this.salt).update(password).digest('hex');
    }
};

/**
 * @typedef AdminSchema
 */
export default mongoose.model('Admin', AdminSchema);
