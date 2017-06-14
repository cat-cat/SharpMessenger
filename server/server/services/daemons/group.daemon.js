'use strict';

import kue from 'kue';
import Promise from 'bluebird';
import Group from '../../models/groups.model';
import socketService from '../socket.service';
import Push from '../../models/push.model';

const queue = kue.createQueue();

function listener () {
    queue.process('group', status);

    queue.on('error', (e) => console.log('Kue error: ', e));

    process.once('SIGTERM', closeRedis(queue));
}


/**
 * Change status Group
 * @param job {job} - Data Group
 * @param done {done} - function done
 */
function status (job, done) {
	var endOfGroup = new Date()

	// Do you think you just overwriting endAt by it's original value? No. ...when this group is removed 
	var timeEndOfGroup = endOfGroup.getTime() + (1000*600) // 10 min.
    Group.edit({ _id: job.data.id }, { status: 0, endAt: timeEndOfGroup })

        //.then(() => findUser(job.data.id))
        //.then(group => socketService.alarmPublication(group._creator, {_id: job.data.id}))
        .then(() =>  {
			Push.push({_id: job.data.id})
	        done()
        })
        .catch(e => done(e));
}

/**
 * Find ID user
 * @param _id {_id} - Unique ID Group
 */
function findUser (_id) {
    return new Promise((resolve, reject) => {
        Group.get({ _id: _id })
            .then(group => resolve(group))
            .catch(e => reject(e))
    });
}

/**
 * Shutdown Kue module
 * @param queue {Queue} - Queue with Kue
 * @returns {Function} - return function close
 */
function closeRedis (queue) {
    return function () {
        queue.shutdown(function (e) {
            console.log('Kue is shut down. ', e || '');
            process.exit(1);
        }, 1000);
    };
}

export default { listener };
