'use strict';

import Admin from '../../models/admin.model';
import passport from 'passport';
import local from 'passport-local';

const LocalStrategy = local.Strategy;

/**
 * Serialize sessions.
 */
passport.serializeUser(function (user, done) {
    done(null, user._id);
});

/**
 * Deserialize sessions
 */
passport.deserializeUser(function (id, done) {
    Admin.get({ _id: id })
        .then(user => done(null, user))
        .catch(err => done(err));
});

/**
 * SignIn
 */
passport.use(
    'signin-admin',
    new LocalStrategy(
        {
            usernameField : 'email',
            passwordField : 'password',
            passReqToCallback : true
        },
        (req, email, password, done) => {
            Admin.get({ email: email })
                .then(user => {
                    (!user) ? done(null, false, { message: 'User not found' }) :
                        (!user.authenticate(password)) ? done(null, false, { message: 'Invalid password'}) :
                            done(null, user);
                })
                .catch(err => {
                    return done(err);
                });
        }));

export default passport;
