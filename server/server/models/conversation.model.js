import messages from './messages.model';
const mongoose = require('mongoose'),  
Schema = mongoose.Schema;
import mongoosePaginate from 'mongoose-paginate';



// Schema defines how chat messages will be stored in MongoDB
const ConversationSchema = new Schema({  
	participants: [{ type: Schema.Types.ObjectId, ref: 'User'}],
	lastMessage: {
	    type: Schema.Types.ObjectId,
	    ref: 'Messages'
	}

}, {collection: "conversations", timestamps: true});

ConversationSchema.statics = {
	    pagination (opts, { page, limit }) {
	        return this.paginate(opts || {}, { page: Number(page), limit: Number(limit), populate: {path: 'lastMessage', populate: { path: '_creator _to', select: 'image name _id' }}, sort: '-updatedAt' });
	    },
	   
		create(participants) {
	    	return this({participants: participants}).save()			
		},

		updateWithMessage(message) {
			return this.update({_id: message.conversationId}, {$set: {lastMessage: message._id}})
			.exec()
		},

		getConversation(participants) {
        	return this.findOne({ participants: {$all: participants}}).lean().exec()
		}

		//,
	 // // Only return one message from each conversation to display as snippet
	 // search(params) {
	 //   return this.find(params)
	 //   .sort('-updatedAt')
	 //   //.populate("lastMessage")
	 //   .populate({path: 'lastMessage', populate: { path: '_creator _to', select: 'image name _id' }})
	 //   .exec(function(err, conversations) {
	 //     if (err)
	 //       return Promise.reject({ error: err })

	 //     return Promise.resolve(conversations)

	 // 	});
	 //}
}

ConversationSchema.plugin(mongoosePaginate);

module.exports = mongoose.model('Conversation', ConversationSchema);  