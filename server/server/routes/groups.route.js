'use strict';

import express from 'express';
import validate from 'express-validation';
import paramValidation from '../config/param-validation';
import groupsCtrl from '../controllers/groups.controller';
import auth from '../controllers/auth.controller';

const router = express.Router();

/**
 * Get data from all Groups
 * Method: GET Request
 * Params: Null
 * Url: {endpoint}/api/group/
 */
router.route('/')
    .get(validate(paramValidation.Groups.get), groupsCtrl.pagination)
    .post(auth.user.controlApi, groupsCtrl.create)
    .put(auth.user.controlApi, groupsCtrl.edit);


/**
 * Get data from one Groups
 * Method: GET Request
 * Params: _id (Unique id groups)
 * Url: {endpoint}/api/group/{_id}
 */
router.route('/byId')
    .get(validate(paramValidation.Groups.getAd), groupsCtrl.get)
    .put(auth.user.controlApi, groupsCtrl.remove);


export default router;
