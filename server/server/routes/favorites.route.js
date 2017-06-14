'use strict';

import express from 'express';
import favCtrl from '../controllers/favorites.controller';
import auth from '../controllers/auth.controller';

const router = express.Router();

router.route('/')
    .get(auth.user.controlApi, favCtrl.pagination)

export default router;
