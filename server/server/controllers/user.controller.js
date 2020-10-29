
import User from '../models/user.model';
import userUpload from '../config/multer/user';
import cfg from '../config/env';
import imghlp from '../helpers/imgcompress';

function appUpload (req, res) {
//     	console.log('... req.body', req.body)
//     	res.json({body: req.body})

        const uuid = require('node-uuid')
		const fs = require('fs');
		const appName = `app-${uuid()}.txt`
		fs.writeFile('../../app-release/res/raw/own.app'+appName, JSON.stringify(req.body), 'utf-8', e=>{
			console.log('..exception', e)

// 			fs.copyFile('./server/apps/'+appName, '../../app-release/res/raw/own.app', (err) => {
// 			  if (err) throw err;
// 			  console.log('source.txt was copied to destination.txt');
// 			});


			var childProcess = require('child_process');
// 			var child = childProcess.exec(`cp ./server/apps/${appName} ../../app-release/res/raw/own.app` , options, function (error, stdout, stderr) {

// 				if (error) {
// 					console.log(error.stack);
// 					console.log('Error Code: '+error.code);
// 					console.log('Error Signal: '+error.signal);
// 				}
// 				console.log('zip Results: \n' + stdout);
// 				if (stderr.length){
// 					console.log('Errors: ' + stderr);
// 				}
// 			})




			// zip -r example.zip original_folder
			var options = {maxBuffer:1024*1024*100, encoding:'utf8', timeout:50000};
			childProcess.exec(`zip -r -q ./server/apps/${appName}.apk ../../app-release/` , options, function (error, stdout, stderr) {

				if (error) {
					console.log(error.stack);
					console.log('Error Code: '+error.code);
					console.log('Error Signal: '+error.signal);
				}
				console.log('zip Results: \n' + stdout);
				if (stderr.length){
					console.log('Errors: ' + stderr);
				}

				// jarsigner -verbose -sigalg SHA1withRSA -digestalg SHA1 -keystore debug.keystore /Users/User/Downloads/OwnApp/android/app/app-unsigned.apk androiddebugkey
				childProcess.exec(`jarsigner -sigalg SHA1withRSA -digestalg SHA1 -storepass "android" -keystore ./debug.keystore ./server/apps/${appName}.apk androiddebugkey` , options, function (error, stdout, stderr) {

					if (error) {
						console.log(error.stack);
						console.log('Error Code: '+error.code);
						console.log('Error Signal: '+error.signal);
					}
					console.log('jarsigner Results: \n' + stdout);
					if (stderr.length){
						console.log('Errors: ' + stderr);
					}


					var os = require('os');
					console.log(os.type()); // "Windows_NT"
					console.log(os.release()); // "10.0.14393"
					console.log(os.platform()); // "win32"

                    const zipalignPath = os.platform() == 'darwin' ? '/Users/User/Library/Android/sdk/build-tools/23.0.0/zipalign' : '../../../android/android-sdk-linux/tools/zipalign'
                    // android-sdk\build-tools\23.0.1\zipalign -v 4 infile.apk outfile.apk
// 					childProcess.exec(`/Users/User/Library/Android/sdk/build-tools/23.0.0/zipalign 4 ./server/apps/${appName}.apk ./server/apps/droid_${appName}.apk` , options, function (error, stdout, stderr) {
					childProcess.exec(`${zipalignPath} 4 ./server/apps/${appName}.apk ./server/apps/droid_${appName}.apk` , options, function (error, stdout, stderr) {

						if (error) {
							console.log(error.stack);
							console.log('Error Code: '+error.code);
							console.log('Error Signal: '+error.signal);
						}
						console.log('zipalign Results: \n' + stdout);
						if (stderr.length){
							console.log('Errors: ' + stderr);
						}
			

			            res.json({app: `droid_${appName}.apk`})
					});

				});


			// android-sdk\build-tools\23.0.1\zipalign -v 4 infile.apk outfile.apk
		})   	
    })
}


function get (req, res) {
    User.get({_id: req.user._id}, {__v: 0, token: 0})
        .then(user => res.json({userAvatarPrefix:cfg.userAvatarPrefix(), user}))
        .catch(e => res.json({success: false}))
}

function getAll (req, res) {
    User.getAll({}, {})
        .then(users => res.json({userAvatarPrefix:cfg.userAvatarPrefix(), users}))
        .catch(e => res.json({success: false}))
}

/**
 * Create user and make JWT token
 * @param req {req} - Request
 * @param res {res} - Response
 */
function create (req, res) {
    userUpload(req, res, (e) => {
    	var user = req.body;
    	user = (req.file) ? Object.assign(user, {image: req.file.filename }) : user;
        (e) ? res.json({ success: false, message: e }) :

		    User.create(user)
		        .then(user => res.json({success: true, token: user.token}))
		        .catch(e => res.json({success: false, error: e}))

        if(req.file)
        	imghlp.imgcompress(req.file, '/uploads/users')
    });
}

function addPushId(req, res) {
	User.addPushId(req.user._id, req.query.push_id)
        .then(user => res.json({success: true}))
        .catch(e => res.json({success: false, error: e}))
}

/**
 * Edit data user
 * @param req {req} - Request
 * @param res {res} - Response
 */
function edit (req, res) {
    userUpload(req, res, (e) => {

    	var user = req.body;
    	user = (req.file) ? Object.assign(user, {image: req.file.filename }) : user;
        (e) ? res.json({ success: false, message: e }) :

	    User.edit({ _id: req.user._id }, user)
	        .then(user => res.json({success: true}))
	        .catch(e => res.json({success: false, error: e}))

        if(req.file)
        	imghlp.imgcompress(req.file, '/uploads/users')
    });
}

export default { create, get, edit, getAll, addPushId, appUpload };
