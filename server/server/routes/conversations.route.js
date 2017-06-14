'use strict';

import express from 'express';
import validate from 'express-validation';
import paramValidation from '../config/param-validation';
import auth from '../controllers/auth.controller';
import ChatController from '../controllers/conversations.controller';

const chatRoutes = express.Router();


// View dialogs to and from authenticated user
  chatRoutes.get('/', auth.user.controlApi, ChatController.getConversations);

  // Retrieve single conversation
  chatRoutes.get('/:conversationId', auth.user.controlApi, ChatController.getConversation);

  // Send reply in conversation
  chatRoutes.post('/:conversationId', auth.user.controlApi, ChatController.sendReply);

  // Start new conversation
  chatRoutes.post('/new/:recipient', auth.user.controlApi, ChatController.newConversation);

export default chatRoutes;
