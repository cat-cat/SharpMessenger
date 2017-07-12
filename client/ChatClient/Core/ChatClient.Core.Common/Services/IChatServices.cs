using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models;

namespace ChatClient.Core.Common.Services
{
    public interface IChatServices
    {
		//void SetRoomID(string _roomID);
        Task Connect();
        Task Send(ChatMessage message, string roomName);
        //Task JoinRoom(string roomName);
        //event EventHandler<ChatMessage> OnMessageReceived;

        void Disabled();
    }
}
