'use strict';

import mongoose from 'mongoose';
import util from 'util';
import app from './config/express';
import config from './config/env';
import socketService from './services/socket.service';
import adDaemon from './services/daemons/group.daemon';

const debug = require('debug')('groups_board:index');
const port = config.port;
const connection = _connect();

global.__base = __dirname + '/';

connection
    .on('error', console.log) // eslint-disable-line no-console
    .on('disconnected', _connect)
    .once('open', _listen);

function _listen () {
    app.listen(port);
    console.log('Express app started on port ' + port); // eslint-disable-line no-console
    socketService.listen();
    adDaemon.listener();
}

function _connect () {
    mongoose.Promise = global.Promise;
    return mongoose.connect(config.db, config.mongoose_opts).connection;
}

if (config.MONGOOSE_DEBUG == true) {
    //mongoose.set('debug', (collectionName, method, query, doc) => {
    //    debug(`${collectionName}.${method}`, util.inspect(query, false, 20), doc);
    //});
    mongoose.set('debug', true);
}
