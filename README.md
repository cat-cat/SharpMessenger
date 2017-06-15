# SharpChat - cross platform chat client and server

Application features:

 * Server and Client complete source code
 * WWW alive test server, no need to setup your own - for fastest dive in code.
 * Private and Group rooms
 * Authomatic user registration upon first startup of the client
 * List of dialogs (private rooms, whispering)
 * List of group rooms
 * Setting user's and/or group avatar
 * Complete UI implementation using XAML and renderers
 * 100% C# - no platform specific languages used
 * Xamarin.iOS and Xamarin.Android 90% common source code
 * Localized for several languages
 * Push notifications
 * Node.js server
 * Socket.IO server chat implementation
 * MongoDB and Redis backend for data storage
 * People can setup a room. One person can create one room and join multiple rooms
 * User's Geo location detection
 * Chat history messages paging
 * ... and much more
 
##

### Getting client to work


* Clone this repo
* Open in Xamarin Studio or MS Visual Studio solution at path \<repo root\>/client/ChatClient.sln
* Wait until all packages are restored and resources generated
* Choose appropriate project to run (ChatClient.iOS or ChatClient.Driod). Note, you can run iOS project only on Mac.
* Select one of "Release" configurations for running the project
* Select the target for running - simulator or a real device
* Run the project and enjoy!

> For debugging:

* Stop running the app
* Select one of the "Debug" running configurations
* Edit file  [Constants.cs](https://github.com/cat-cat/SharpChat/blob/PreRelease/client/ChatClient/Core/ChatClient.Core.Common/Properties/Constants.cs) - set debugURL the same as baseURL
* Run and enjoy debugging!

##

### Getting server to work
> Note, you don't need to set up the server if you only interested in client development. For your convinience there's already set up running server at ip address specified in baseURL variable in file [Constants.cs](https://github.com/cat-cat/SharpChat/blob/PreRelease/client/ChatClient/Core/ChatClient.Core.Common/Properties/Constants.cs)

* Clone this repo
* Install on your machine [Node.js 6+](https://nodejs.org/en/download/ "Node.js download")
* Install and run on your machine [MongoDB 3.2+](https://www.mongodb.com/download-center#community)
* Install and run on your machine [Redis](https://redis.io/download)
* cd to \<repo root\>/client 
* In system prompt run `npm install -g forever`
* then run `npm install -g yarn`
* then run `npm yarn install`
> For starting Chat Server in release mode:
> * run `yarn build`
> * run `forever start dist/index.js`
> * for finding location of server's log run `forever list`
> * for stopping Chat server run `forever stop 0`


> For starting Chat Server in debug mode:
> * run `yarn watch-dev`
> * see logs of the server in console
> * for stopping server press Ctrl+C

Note: for client to be able to work with you locally running server you should set debugURL variable at [Constants.cs](https://github.com/cat-cat/SharpChat/blob/PreRelease/client/ChatClient/Core/ChatClient.Core.Common/Properties/Constants.cs) to IP address of your server, which can be found at the server's log.
