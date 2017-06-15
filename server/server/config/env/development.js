'use strict';

var ip = require('ip').address();
console.log(ip)

export default {
    env: 'development',
    MONGOOSE_DEBUG: true,
    db: 'mongodb://localhost/groups-dev',
    mongoose_opts: { server: { socketOptions: { keepAlive: 1 } } },
    port: process.env.PORT || 2003,
    jwt: '..Hfnsa.',
    filePrefix: function() {return 'http://' + ip + ':' + this.port +'/images/groups/'},
    userAvatarPrefix: function() {return 'http://' + ip + ':' + this.port +'/images/users/'},
    srv: ip,
    socketPort: 8020
};
