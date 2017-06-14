'use strict';

import mongoose from 'mongoose';
import Promise from 'bluebird';
import mongoosePaginate from 'mongoose-paginate';
import User from './user.model';

const Schema = mongoose.Schema;

/**
 * Groups Schema
 */
const FavoritesSchema = new Schema({
    adid: {
        type: Schema.Types.ObjectId,
        ref: 'Groups'
    },
    _creator: {
    	type: Schema.Types.ObjectId,
    	ref: 'User'
    }
}, { collection: 'favorites', timestamps: true })

FavoritesSchema.statics = {

    pagination ({user, page, limit}) {
        return this.paginate({_creator: user}, {page: Number(page), limit: Number(limit), populate: { path: 'adid', populate: {path: '_creator currentWinner', select: 'image name _id'}}, sort: '-updatedAt'});
    },

    create (data) {
        return new Promise((resolve,reject) => {
            this(data).save((e, group) => (e) ? reject(e) : resolve(group))
        });
    },
};

FavoritesSchema.plugin(mongoosePaginate);

/**
 * @typedef FavoritesSchema
 */
export default mongoose.model('favorites', FavoritesSchema);
