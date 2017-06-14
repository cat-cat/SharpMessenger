'use strict';

import kue from 'kue';

const queue = kue.createQueue();


function remove(group_id) {
	//console.log(group_id)
	queue.delayed( function( err, ids ) {
	  if(err) {
	  	console.log(err)
	  	return;
	  }
	//  console.log(ids);
	  ids.forEach( function( id ) {
	    kue.Job.get( id, function( err, job ) {
	      if(job.data.id == group_id) {
		    job.remove( function(){
		      console.log( 'job removed ', job.id );
		    });
          }
	    });
	  });
	});	
}

function group (data, date) {
    queue
        .create('group', data)
        .delay(date)
        .removeOnComplete( true )
        .save();
}

function remove(group_id) {
	//console.log(group_id)
	queue.delayed( function( err, ids ) {
	  if(err) {
	  	console.log(err)
	  	return;
	  }
	//  console.log(ids);
	  ids.forEach( function( id ) {
		    kue.Job.get( id, function( err, job ) {
		      if(job.data.id == group_id) {
			      // Your application should check if job is a stuck one
			      job.delay(1) // process job just now
			      .removeOnComplete( true )
		          .update(function() {
       				console.log(JSON.stringify(job));
     		 	  });
	          }
		    });
	  });
	});
}


/**
 * Create Job in Queue
 * @param data {data} - Data Job (example: id, message, etc.)
 * @param date {date} - Expiration date
 * @returns {Job}
 */

export default  { group, remove, remove };