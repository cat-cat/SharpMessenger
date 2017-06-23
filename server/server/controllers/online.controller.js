'use strict';

import socketService from '../services/socket.service';

function check (req, res, next) {
	res.json({success: true, data:socketService.getOnline(req.query.ids, req.query.client)})
}

export default { check };
