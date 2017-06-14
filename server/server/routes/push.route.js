'use strict';

import express from 'express';
import pushCtrl from '../controllers/push.controller';

const router = express.Router();

/**
 * Get data from one Groups
 * Method: GET Request
 * Params: _id (Unique id groups)
 * Url: {endpoint}/api/push/{apn_push_id}
 */
router.route('/')
    .get(pushCtrl.get);

export default router;
