'use strict';

import express from 'express';
import userRoutes from './user.route';
import groupsRoutes from './groups.route';
import authRoutes from './auth.route';
import adminRoutes from './admin.route';
import pushRoutes from './push.route';
import messagesRoutes from './messages.route';
import chatRoutes from './conversations.route';
import mailRoutes from './mail.route';
import favRoutes from './favorites.route';

const router = express.Router();

router.use('/user', userRoutes);
router.use('/group', groupsRoutes);
router.use('/auth', authRoutes);
router.use('/admin', adminRoutes);
router.use('/push', pushRoutes);
router.use('/messages', messagesRoutes);
router.use('/chat', chatRoutes);
router.use('/mail', mailRoutes);
router.use('/favorites', favRoutes);

export default router;
