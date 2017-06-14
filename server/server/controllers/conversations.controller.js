'use strict';

import Conversation from '../models/conversation.model';
import Message from '../models/messages.model';
import cfg from '../config/env';

function getConversations(req, res, next) {  
  // Only return one message from each conversation to display as snippet
  Conversation.pagination({ participants: req.user._id }, req.query)
        .then((conversations) => res.json({userAvatarPrefix:cfg.userAvatarPrefix(), conversations: conversations}))
        .catch((e) => res.json(e))
}

function getConversation(req, res, next) {  
  Message.find({ conversationId: req.params.conversationId })
        .then((messages) => res.json(messages))
        .catch((e) => res.json(e))
}

function newConversation(req, res, next) {  
  if(!req.params.recipient) {
    res.status(422).send({ error: 'Please choose a valid recipient for your message.' });
    return next();
  }

  if(!req.body.composedMessage) {
    res.status(422).send({ error: 'Please enter a message.' });
    return next();
  }

  const conversation = new Conversation({
    participants: [req.user._id, req.params.recipient]
  });

  conversation.save(function(err, newConversation) {
    if (err) {
      res.send({ error: err });
      return next(err);
    }

    const message = new Message({
      conversationId: newConversation._id,
      body: req.body.composedMessage,
      author: req.user._id
    });

    message.save(function(err, newMessage) {
      if (err) {
        res.send({ error: err });
        return next(err);
      }

      res.status(200).json({ message: 'Conversation started!', conversationId: conversation._id });
      return next();
    });
  });
}

function sendReply(req, res, next) {  
  const reply = new Message({
    conversationId: req.params.conversationId,
    body: req.body.composedMessage,
    author: req.user._id
  });

  reply.save(function(err, sentReply) {
    if (err) {
      res.send({ error: err });
      return next(err);
    }

    res.status(200).json({ message: 'Reply successfully sent!' });
    return(next);
  });
}

// DELETE Route to Delete Conversation
function deleteConversation(req, res, next) {  
  Conversation.findOneAndRemove({
    $and : [
            { '_id': req.params.conversationId }, { 'participants': req.user._id }
           ]}, function(err) {
        if (err) {
          res.send({ error: err });
          return next(err);
        }

        res.status(200).json({ message: 'Conversation removed!' });
        return next();
  });
}

// PUT Route to Update Message
function updateMessage(req, res, next) {  
  Conversation.find({
    $and : [
            { '_id': req.params.messageId }, { 'author': req.user._id }
          ]}, function(err, message) {
        if (err) {
          res.send({ error: err});
          return next(err);
        }

        message.body = req.body.composedMessage;

        message.save(function (err, updatedMessage) {
          if (err) {
            res.send({ error: err });
            return next(err);
          }

          res.status(200).json({ message: 'Message updated!' });
          return next();
        });
  });
}

export default { getConversations, getConversation, newConversation, sendReply, updateMessage };
