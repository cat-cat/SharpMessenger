'use strict';

import express from 'express';
import messagesCtrl from '../controllers/messages.controller';
import auth from '../controllers/auth.controller';

const router = express.Router();

/**
 * Get data from all Groups
 * Method: GET Request
 * Params: Null
 * Url: {endpoint}/api/group/
 */
router.route('/')
    .get(auth.user.controlApi, messagesCtrl.pagination);
//    .post(auth.user.controlApi, groupsCtrl.create);

router.route('/messageStatus')
    .get(auth.user.controlApi, messagesCtrl.messageStatus);


/**
 * Get data from one Groups
 * Method: GET Request
 * Params: _id (Unique id groups)
 * Url: {endpoint}/api/group/{_id}
 */
//router.route('/:_id')
//    .get(validate(paramValidation.Groups.getAd), groupsCtrl.get);

export default router;
