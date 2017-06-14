'use strict';

import Push from '../models/push.model';

/**
 * Get data from one groups
 * @param req {req} - Request
 * @param res {res} - Response
 * @param next {next}
 */
function get (req, res, next) {
    Push.get({ _apn_push_id: req.query.apn_push_id })
        .then((result) => res.json(result))
        .catch((e) => res.json(e))
}


export default { get };
