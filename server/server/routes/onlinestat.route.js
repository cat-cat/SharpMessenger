'use strict';

import express from 'express';
import onlineCtrl from '../controllers/online.controller';
import auth from '../controllers/auth.controller';

const router = express.Router();

/**
 * Get data from all Groups
 * Method: GET Request
 * Params: Null
 * Url: {endpoint}/api/group/
 */
router.route('/')
    .get(auth.user.controlApi, onlineCtrl.check)

export default router;
