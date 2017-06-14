'use strict';

import Favorites from '../models/favorites.model';
import cfg from '../config/env';

/**
 *
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next} - Next function
 */
function pagination (req, res, next) {
    Favorites.pagination({user: req.user._id, page: req.query.page, limit: req.query.limit})
    .then(r => {
    	var groups = [] 
    	r.docs.forEach(function (item) {
  			groups.push(item.adid);
		})
    	res.json({filePrefix:cfg.filePrefix(), favorites:{docs:groups, total:r.total, limit:r.limit, page:r.page, pages:r.pages}})
    })
    .catch(e => res.json({success: false, message: e}))
}

export default { pagination };
