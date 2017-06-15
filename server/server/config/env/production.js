'use strict';

var ip = require('ip').address();

export default {
    env: 'production',
    MONGOOSE_DEBUG: false,
    db: 'mongodb://localhost/groups',
    mongoose_opts: { server: { socketOptions: { keepAlive: 1 } } },
    port: process.env.PORT || 3040,
    jwt: '..tribidoh',
    filePrefix:  function() {return 'http://' + ip + ':' + this.port +'/images/groups/'},
    userAvatarPrefix: function() {return 'http://' + ip + ':' + this.port +'/images/users/'},
    srv: ip,
    socketPort: 8020
};
