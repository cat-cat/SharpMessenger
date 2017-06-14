'use strict';

import express from 'express'
import bodyParser from 'body-parser'
import methodOverride from 'method-override'
import logger from 'morgan'
import cookieParser from 'cookie-parser';
import session from 'express-session';
import helmet from 'helmet';
import redis from 'redis';
import redis_store from 'connect-redis'
import routes from '../routes/index.route'
import passport from './passport';
import config from './env';

const app = express();
const redisStore = redis_store(session);
const clientRedis  = redis.createClient();

global.__base = __dirname + '/';

clientRedis.on('connect', logRedis('connect'));
clientRedis.on('ready', logRedis('ready'));
clientRedis.on('error', logRedis('error'));

function logRedis(type) {
    return function() {
        switch(type)  {
            case 'connect': console.log('Successfully connected to the database Redis');
                break;
            case 'ready': console.log('Ready connection the database Redis');
                break;
            case 'error': {
                console.log('Error connecting to database Redis');
                _exit();
            }
                break;
        }
    }
}

if (config.env.NODE_ENV === 'development') {
    app.use(logger('dev'));
}

app.use(helmet());

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(cookieParser());
console.log("__base: " + __base)
app.use('/images/groups', express.static(__base + '../uploads/groups'));
app.use('/images/users', express.static(__base + '../uploads/users'));

app.use(session({
    store: new redisStore({
        host: 'localhost',
        port: 6379,
        client: clientRedis
    }),
    secret: 'groups_board?',
    name: 'groups_board.?',
    resave: false,
    saveUninitialized: true
}));

app.use(methodOverride());

app.use(passport.initialize());
app.use(passport.session());
app.use('/api', routes);

function _exit () {
    process.exit(1);
}

export default app;
