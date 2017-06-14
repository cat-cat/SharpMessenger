'use strict';

var ip = require('ip').address();
console.log(ip)

export default {
    env: 'development',
    MONGOOSE_DEBUG: true,
    db: 'mongodb://localhost/groups-dev',
    mongoose_opts: { server: { socketOptions: { keepAlive: 1 } } },
    port: process.env.PORT || 3003,
    jwt: '..?241Hfnsa.',
    filePrefix: function() {return 'http://' + ip + ':' + this.port +'/images/groups/'},
    userAvatarPrefix: function() {return 'http://' + ip + ':' + this.port +'/images/users/'},
    nx: 'https://perfectmoney.is/api/step1.asp',
    srv: ip,
    nxacc: 'U14023919'
};
