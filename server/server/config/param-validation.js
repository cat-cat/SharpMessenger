'use strict';

import Joi from 'joi';

export default {
    // Groups methods validations
    Groups: {
        // GET {endpoint}/api/group/{_id}
        getAd: {
            query: {
                _id: Joi
                    .string()
                    .regex(/^(?=[a-f\d]{24}$)(\d+[a-f]|[a-f]+\d)/i)
                    .required()
                    .label('Invalid _id')
            }
        },
        // GET {endpoint}/api/group/
        get: {
            query: {
                page: Joi.number().required(),
                limit: Joi.number().required()
            }
        }
    }
}
