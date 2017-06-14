'use strict';

import express from 'express';
import auth from '../controllers/auth.controller';

const router = express.Router();

router.route('/admin')
    .post(auth.admin.signIn);

export default router;
