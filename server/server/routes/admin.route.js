'use strict';

import express from 'express';
import adminCtrl from '../controllers/admin.controller';

const router = express.Router();

/**
 * Create admin
 * Method: POST Request
 * Params: email, password
 * Url: {endpoint}/api/admin/
 */
router.route('/')
    .post(adminCtrl.create);

export default router;
