'use strict';

import passport from 'passport';
import Strategy from 'passport-jwt';
import User from '../../models/user.model';
import cfg from '../env';

const ExtractJwt = Strategy.ExtractJwt;
const JwtStrategy = Strategy.Strategy;

passport.use(
    'signin-jwt',
    new JwtStrategy(
        {
            secretOrKey: cfg.jwt,
            jwtFromRequest: ExtractJwt.fromAuthHeader()
        },
        (payload, done) => {
            User.get({ _id: payload._id })
                .then(user => (user) ? done(null, { _id: user._id }) : done(null, false ))
                .catch(e => done(e))
        }));

export default passport;
