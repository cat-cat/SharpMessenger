'use strict';

import socketService from '../services/socket.service';

function setIsTyping (req, res, next) {
	socketService.setIsTyping(req.user._id, req.query.client, req.query.room, req.query.timestamp)
	res.json({success: true})
}

export default { setIsTyping };
