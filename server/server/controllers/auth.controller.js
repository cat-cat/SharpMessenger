'use strict';

import passport from 'passport';

/**
 * Middleware methods for the administrator control
 * @type {{signIn: admin.signIn, controlApi: admin.controlApi, controlPage: admin.controlPage}}
 */
const admin = {
    /**
     * Method for authentication control
     * @param req {req} - Request
     * @param res {res} - Response
     * @param next {next} - Next function
     */
    signIn: function (req, res, next) {
        passport.authenticate('signin-admin', {
            successRedirect: '/create',
            failureRedirect: '/'
        })(req, res, next);
    },

    /**
     * Method for control API methods
     * @param req {req} - Request
     * @param res {res} - Response
     * @param next {next} - Next function
     */
    controlApi: function (req, res, next) {
        (req.isAuthenticated()) ? next() : res.status(403).json({success: false});
    },

    /**
     * Method for control administrator pages
     * @param req {req} - Request
     * @param res {res} - Response
     * @param next {next} - Next function
     */
    controlPage: function (req, res, next) {
        (req.isAuthenticated()) ? next() : res.redirect('/');
    }
};

/**
 * Middleware methods for the administrator control
 * @type {{controlApi: user.controlApi}}
 */
const user = {
    /**
     * Method for control API methods
     * @param req {req} - Request
     * @param res {res} - Response
     * @param next {next} - Next function
     */
    controlApi: function (req, res, next) {
        passport.authenticate('signin-jwt', {
            session: false
        })(req, res, next);
    }
};


export default { admin, user };
