'use strict';

import express from 'express';
import istypingCtrl from '../controllers/istyping.controller';
import auth from '../controllers/auth.controller';

const router = express.Router();

/**
 * Get data from all Groups
 * Method: GET Request
 * Params: Null
 * Url: {endpoint}/api/group/
 */
router.route('/')
    .get(auth.user.controlApi, istypingCtrl.setIsTyping)

export default router;
