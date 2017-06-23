'use strict';

import socketioJwt from 'socketio-jwt';
import User from '../models/user.model';
import cfg from '../config/env';
import Conversation from '../models/conversation.model';

const app = require('express')();
const http = require('http').Server(app);
const _ = require('underscore')._;
const io = require('socket.io')(http);
const util = require('util');
const Room = require('../models/room.js');
var mongo = require('mongodb').MongoClient;
var ObjectId = require('mongodb').ObjectID;
var db_collection_messages = null;
var db_collection_conversations = null;

/**
 * Listen socket service and register listeners
 */
function listen() {
    http.listen(cfg.socketPort, () => {
        console.log('Socket service started on port ' + cfg.socketPort);
        mongo.connect(cfg.db, function(err, db) {
            if (err) {
                console.log("Error connecting Mongo for sockets: " + util.inspect(err, false, null));
            }
            db_collection_messages = db.collection('messages');
            db_collection_conversations = db.collection('conversations');
            listeners();
        });
    });
}

//io.set("log level", 1);
var people = {};
var rooms = {};
var sockets = [];
//var chatHistory = {};

/**
 * Listeners
 */
function listeners() {
    io
    .on('connection', socketioJwt.authorize({
            secret: cfg.jwt,
            timeout: 60000
                     }), console.log('Socket connection on port ' + cfg.socketPort))
        .on('authenticated', function(socket) {
            //console.log("authenticated: " + socket.decoded_token._id);
            //        joinRoom(socket);
            connectedListeners(socket);
            //        nearestAd(socket);
        });
    //    .on('connection', nearestAd);
}

/**
 * Connected listeners
 * @param socket {socket} - socket connection
 */
function connectedListeners(socket) {

    socket.onclose = function(reason) {

	  var i = sockets.indexOf(socket);
	  if(i >= 0) {
	  	//console.log("deleted from sockets")
	  	sockets.splice(i, 1);
	  }

	 // var numClients = io.sockets.adapter.rooms[socket.room]
	 // if (numClients == undefined) {
		////console.log(socket.decoded_token._id + " was last in a room " + socket.room)
		//delete rooms[socket.room]
	 // }

	  var rooms = Object.keys(socket.rooms)
	  rooms.forEach(function(room) {
		  try { // in case there are no sockets in room
		  	io.sockets.in(room).emit("update-people", {onlineStatus: false, id: people[socket.id].id})
		  } catch(err) {
		  }
	  })

	  people[socket.id].participants.forEach(function(id) {
		for(var key in people) {
			if(id == people[key].id) {
				try {
					io.sockets.connected[key].emit("update-people", {onlineStatus: false, id: people[socket.id].id})
				} catch (err) {
				}
			}
		};
	  })

	  delete people[socket.id];

      Object.getPrototypeOf(this).onclose.call(this, reason);
    }



	socket.on('disconnect', function() {
	  console.log('Got disconnect! ' + socket.decoded_token._id);
	});

    socket.on("joinserver", function(name, device) {
        console.log("joined server: " + name);
        //                  var exists = false;
        var ownerRoomID = null;
        var inRoomID = null;

        //                  _.find(people, function(key,value) {
        //                         if (key.name.toLowerCase() === name.toLowerCase())
        //                         return exists = true;
        //                  });
        //                  if (exists) {//provide unique username:
        //                      var randomNumber=Math.floor(Math.random()*1001)
        //                      do {
        //                          var proposedName = name+randomNumber;
        //                          _.find(people, function(key,value) {
        //                             if (key.name.toLowerCase() === proposedName.toLowerCase())
        //                                 return exists = true;
        //                          });
        //                      } while (!exists);
        //                          socket.emit("exists", {msg: "The username already exists, please pick another one.", proposedName: proposedName});
        //                  } else {
        people[socket.id] = {
            "id": socket.decoded_token._id,
            "name": name,
            "owns": ownerRoomID,
            "inroom": inRoomID,
            "device": device,
            "participants": []
        };
        sockets.push(socket);
        var sizePeople = _.size(people);
        var sizeRooms = _.size(rooms);
        //socket.emit("update-people", {
        //    people: people,
        //    count: sizePeople
        //});
        //socket.emit("roomList", {
        //    rooms: rooms,
        //    count: sizeRooms
        //});
        socket.emit("joined"); //extra emit for GeoLocation
        //io.sockets.emit("update", people[socket.id].name + " is online.")
        //                  }
    });

    //        socket.on("getOnlinePeople", function(fn) {
    //                  fn({people: people});
    //        });

    //        socket.on("typing", function(data) {
    //                  if (typeof people[socket.id] !== "undefined")
    //                  io.sockets.in(socket.room).emit("isTyping", {isTyping: data, person: people[socket.id].name});
    //                  });

    socket.on("send", function(data) {
        var msTime = data.date,
        msg = data.message;
        var conversationId = data.conversationId;
        var whitespacePattern = /^\s*$/;
        if (data.message == null || whitespacePattern.test(data.message)) {
            console.log('Invalid! Cannot insert empty string.');
            socket.emit("update", 'Did you typed in a MESSAGE yet?');
            return;
        }

        var re = /^[w]:.*:/;
        var whisper = re.test(msg);
        var whisperStr = msg.split(":");
 	 	var found = false;
        if (whisper) {
            var whisperTo = whisperStr[1];
            var keys = Object.keys(people);
            if (keys.length != 0) {
                for (var i = 0; i < keys.length; i++) {
                    if (people[keys[i]].id === whisperTo) {
                        var whisperId = keys[i];
                        found = true;
                        if (socket.id === whisperId) { //can't whisper to ourselves
                            socket.emit("update", "You can't whisper to yourself.");
                        }
//                        break;
                    }
                }
            }
            var whisperTo = whisperStr[1];
            var whisperMsg = whisperStr[2];


            new Promise((resolve, reject) => {
        		if(conversationId != null)
        			resolve({_id: conversationId})

            	return Conversation.getConversation([socket.decoded_token._id, whisperTo])
            	.then(c => {
                		if(c)
                			resolve(c) 

			        	resolve(Conversation.create([socket.decoded_token._id, whisperTo]))			        								        		
			     })		
           	})
        	.then(function(conv, error) {
                db_collection_messages.insert({
                	conversationId: ObjectId(conv._id),
                    createdAt: msTime,
                    Name: people[socket.id].name,
                    Message: whisperMsg,
                    _creator: ObjectId(socket.decoded_token._id),
                    _to:ObjectId(whisperTo)
                }, function(error, message) {
                	if(error)
                		console.log(error)

                    Conversation.updateWithMessage(message.ops[0])
                    .then((c, err) => {
						if(c) {
							//console.log("updated conversation: " + util.inspect(message,false,null));

							socket.emit("conversation", message.ops[0].conversationId);

							User.get({_id:ObjectId(socket.decoded_token._id)}, {image: 1})
							.then(u => { 
				                io.sockets.connected[socket.id].emit("whisper", {
				                	userAvatarPrefix:cfg.userAvatarPrefix(),
				                    msTime: msTime,
				                    socketID: people[socket.id],
				                    msg: whisperMsg,
				                    userImage: u.image || false,
				                    participantOnline: io.sockets.connected[whisperId] == undefined ? false : true 
				                })

				                User.get({_id:ObjectId(whisperTo)}, {image: 1})
				                .then( u => {
					                io.sockets.connected[whisperId].emit("whisper", {
					                	userAvatarPrefix:cfg.userAvatarPrefix(),
					                    msTime: msTime,
					                    socketID: people[socket.id],
					                    msg: whisperMsg,
					                    userImage: u.image || false,
					                    participantOnline: true
					                })
					            })
					            .catch(e => console.log(e))
			                })
			                .catch(e => console.log(e))
						}
					})
					.catch(e => console.log(e))
                });
		    })
        	.catch(e => console.log({success: false, error: e}))
        } else { // if not wisper
            if (io.sockets.adapter.sids[socket.id][socket.room] !== undefined) {

				User.get({_id:ObjectId(socket.decoded_token._id)}, {image: 1})
				.then(u => { 
	              io.sockets.in(socket.room).emit("chat", {
				        userAvatarPrefix:cfg.userAvatarPrefix(),
	                    msTime: msTime,
	                    socketID: people[socket.id],
	                    userImage: u.image || false,
	                    msg: msg
	                });
	                //socket.emit("isTyping", false);
	                db_collection_messages.insert({
	                    Room: socket.room,
	//                    createdAt: msTime,
	                    Name: people[socket.id].name,
	                    Message: msg,
	                    _creator: ObjectId(socket.decoded_token._id),
	                    _to:ObjectId(data.id)
	                }, function() {
	                    //  console.log(data.name + ' inserted a message into db');
	                });
                });

            } else {
                socket.emit("update", "Please connect to a room.");
            }
        }
    });

    //Room functions
    function CreateRoom(name) {
    	// TODO: sometimes crashes with: Cannot read property 'inroom' of undefined
        if (people[socket.id].inroom) {
            socket.emit("update", "You are in a room. Please leave it first to create your own.");
        } else if (!people[socket.id].owns) {
            //                  var id = uuid.v4();
            var id = name;
            var room = new Room(name, id, socket.id);
            rooms[id] = room;
            var sizeRooms = _.size(rooms);
            socket.emit("roomList", {
                rooms: rooms,
                count: sizeRooms
            });
            //add room to socket, and auto join the creator of the room
            socket.room = name;
            socket.join(socket.room);
            people[socket.id].owns = id;
            people[socket.id].inroom = id;
            room.addPerson(socket.id);
            socket.emit("update", "Welcome to " + room.name + ".");
            socket.emit("sendRoomID", {
                id: id
            });
            //                      chatHistory[socket.room] = [];
        } else {
            socket.emit("update", "You have already created a room.");
        }
    };

    socket.on("check", function(name, fn) {
        var match = false;
        _.find(rooms, function(key, value) {
            if (key.name === name)
                return match = true;
        });
        fn({
            result: match
        });
    });

    socket.on("removeRoom", function(id) {
        var room = rooms[id];
        if (socket.id === room.owner) {
            purge(socket, "removeRoom");
        } else {
            socket.emit("update", "Only the owner can remove a room.");
        }
    });

    socket.on("joinRoom", function(id) {
        if (io.sockets.adapter.rooms[id] == undefined) {
            CreateRoom(id)
        }
        if (typeof people[socket.id] !== "undefined") {
            var room = rooms[id];
            if (socket.id === room.owner) {
                socket.emit("update", "You are the owner of this room and you have already been joined.");
            } else {
                if (_.contains((room.people), socket.id)) {
                    socket.emit("update", "You have already joined this room.");
                } else {
                    if (people[socket.id].inroom !== null) {
                        socket.emit("update", "You are already in a room (" + rooms[people[socket.id].inroom].name + "), please leave it first to join another room.");
                    } else {
                        room.addPerson(socket.id);
                        people[socket.id].inroom = id;
                        socket.room = room.name;
                        socket.join(socket.room);
                        var user = people[socket.id];
                        io.sockets.in(socket.room).emit("update", user.name + " has connected to " + room.name + " room.");
                        socket.emit("update", "Welcome to " + room.name + ".");
                        socket.emit("sendRoomID", {
                            id: id
                        });
                        //                                      var keys = _.keys(chatHistory);
                        //                                      if (_.contains(keys, socket.room)) {
                        //                                          socket.emit("history", chatHistory[socket.room]);
                        //                                      }
                    }
                }
            }
        } else {
            socket.emit("update", "Please enter a valid name first.");
        }
    });

    socket.on("leaveRoom", function(id) {
        var room = rooms[id];
        if (room)
            purge(socket, "leaveRoom");
    });

    io.on('disconnect', () => {
        changeStatus({
            _id: socket.decoded_token._id,
            status: 0
        })
        console.log('Socket service disconnect on port ' + cfg.socketPort);
    });

    io.on('connect_error', () => console.log('Connection failed'));

    io.on('reconnect_failed', () => console.log('Reconnection failed'));
}

/**
 * Join user to individually room
 * Change status user to online
 * @param socket {socket} - socket connection
 */
function joinRoom(socket) {
    changeStatus({
        _id: socket.decoded_token._id,
        status: 1
    });
    socket.join(socket.decoded_token._id);
}

/**
 * Get nearest Group dates
 * @param socket {socket} - socket connection
 */
function nearestAd(socket) {
    console.log('a user connected')
    socket.emit('hi', 'more data');

    socket.on('hi2', function(d) {

        console.log("hi2" + d);
        socket.emit('hi2back', 'more data');
    });

    // simple test
    socket.on('hi', function(d) {
        console.log("hi" + d);
        socket.emit('hi', 'more data');
    });
}

function hi(socket) {
    console.log('on hi');
}

/**
 * Change status user
 * @param _id {_id} - Unique id user
 * @param status {status} - online status user
 */
function changeStatus({
    _id,
    status
}) {
    User.edit({
            _id: _id
        }, {
            status: status
        })
        .then()
        .catch(e => console.log(e))
}

/**
 * Alarm remove Group
 * @param _id {_id} - Unique ID user
 * @param data {data} - Send JSON user
 */
function alarmPublication(_id, data) {
    io.sockets.to(_id).emit('group:remove', data);
}

/**
 * Alarm nearest date Group
 * @param _id {_id} - Unique ID user
 * @param data {data} - Send JSON user
 */
function alarmNearest(_id, data) {
    io.sockets.to(_id).emit('group:nearest', data);
}

function getOnline(ids, client){
	var dictionary = {}
	var idsArr = ids.split(',')
	idsArr.forEach(function(id) {
		for(var key in people) {
			if(id == people[key].id) {
				dictionary[id] = true
				people[key].participants.push(client)
			}
		};

		if(dictionary[id] == undefined)
			dictionary[id] = false
	});
	return dictionary
}

function setIsTyping(user, client, room, timestamp){
	console.log("is typing called with client: " + client + " room " + util.inspect(room, false, null) + " timestamp "+ timestamp)

	// TODO: move searching in people to redis
	for(var key in people) {
		try {
		      if (room.length > 0 && user == people[key].id) {
			      	io.sockets.connected[key].broadcast.to(room).emit("isTyping", {person: user})
			      	break
		      }
		      else if(client.length > 0 && client == people[key].id) // private chat
		      {
					io.sockets.connected[key].emit("isTyping", {person: user});
					break
		      }
	      } catch (err) {
	      	console.log(err)
	      }
	};
}

export default {
	setIsTyping,
    listen,
    alarmPublication,
    getOnline
};
