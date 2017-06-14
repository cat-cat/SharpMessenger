'use strict';

import express from 'express';
import UserCtrl from '../controllers/user.controller';
import auth from '../controllers/auth.controller';

const router = express.Router();

router.route('/')
    .get(auth.user.controlApi, UserCtrl.get)
    .post(UserCtrl.create)
    .put(auth.user.controlApi, UserCtrl.edit);

router.route('/all')
	.get(auth.user.controlApi, UserCtrl.getAll);

router.route('/add/pushId/')
	.put(auth.user.controlApi, UserCtrl.addPushId);

export default router;
