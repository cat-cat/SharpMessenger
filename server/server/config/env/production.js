'use strict';

var ip = require('ip').address();
console.log(ip)

export default {
    env: 'production',
    MONGOOSE_DEBUG: false,
    db: 'mongodb://localhost/groups',
    mongoose_opts: { server: { socketOptions: { keepAlive: 1 } } },
    port: process.env.PORT || 4040,
    jwt: '..trax241Hfnsa.tibidoh',
    filePrefix:  function() {return 'http://' + ip + ':' + this.port +'/images/groups/'},
    userAvatarPrefix: function() {return 'http://' + ip + ':' + this.port +'/images/users/'},
    nx: 'https://perfectmoney.is/api/step1.asp',
    srv: ip,
    nxacc: 'U14023919'
};
