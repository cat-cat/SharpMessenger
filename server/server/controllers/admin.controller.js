'use strict';

import Admin from '../models/admin.model';

/**
 * Create admin
 * @param req {req} - Request
 * @param res {res} - Response
 */
function create (req, res) {
    Admin.get({ email: req.query.email })
        .then(user => (!user) ? Admin.create(req.query) : res.json({success: false, message: 'User exist'}))
        .then(() => res.json({success: true}))
        .catch(e => res.json({success: false}));
}

export default { create };
