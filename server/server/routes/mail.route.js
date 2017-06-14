'use strict';

import express from 'express';
import mailCtrl from '../controllers/mail.controller';
import auth from '../controllers/auth.controller';

const router = express.Router();

/**
 * Get data from all Groups
 * Method: GET Request
 * Params: Null
 * Url: {endpoint}/api/group/
 */
router.route('/')
    .get(auth.user.controlApi, mailCtrl.check)
    .post(auth.user.controlApi, mailCtrl.send)
    .put(auth.user.controlApi, mailCtrl.add)

export default router;
