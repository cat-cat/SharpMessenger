'use strict';

var FCM = require('fcm-push');

var serverKey = 'server_key';
var fcm = new FCM(serverKey);

var apn = require('apn');

var options = {
  token: {
    key: "../APNsAuthKey_TJ7VP5YYZZ.p8",
    keyId: "key_id",
    teamId: "team_id"
  },
  production: false
};

var apnProvider = new apn.Provider(options);

var note = new apn.Notification();

note.expiry = Math.floor(Date.now() / 1000) + 1800; // Expires 30min from now.
//note.badge = 3;
note.sound = "ping.aiff";
note.alert = ""; // alert set below, search this file for note.alert = 
note.payload = {'messageFrom': 'John Appleseed'};
note.topic = "ru.groups.board";


function get (query) {
		console.log("APN push id: %o", query._apn_push_id);
		return apnProvider.send(note, query._apn_push_id)
}

function push(group_id) {
		var sevenDaysOld = new Date()
		sevenDaysOld.setDate(sevenDaysOld.getDate()-7)
        Users.find({})
        .exec()
        .then(users => {
      //  	var appleIds = [];
      //  	//var droidIds = [];
      //  	users.forEach( function( user ) {
      //  		user._id[0].push_ids.forEach( function (push_id) {
      //  			if(push_id.startsWith("droid:")) {
      //  				console.log(push_id)
						//var message = {
						//    to: push_id.substring(6), // required fill with device token or topics
						//    collapse_key: 'your_collapse_key', 
						//    data: {
						//        your_custom_data_key: 'your_custom_data_value'
						//    },
						//    notification: {
						//        title: group_id,
						//        body: 'Body of your push notification'
						//    }
						//};
						//fcm.send(message, () => {/* prevent message: https://github.com/nandarustam/fcm-push/pull/25/files */})
					 //   .then(function(response){
					 //       console.log("Successfully sent with response: ", response);
					 //   })
					 //   .catch(function(err){
					 //       console.log("Something has gone wrong!");
					 //       console.error(err);
					 //   })
      //  				//droidIds.push(push_id.substring(5))
      //  			}
      //  			else
      //  				appleIds.push(push_id)
      //  		})
      //  	})

      //  	note.alert = "" + group_id;
      //  	apnProvider.send(note, appleIds)
      //  	.then(res => console.log(res))
      //  	.catch(e => console.log(e))

        })
        .catch(e => console.log(e));
}

export default {get, push};
