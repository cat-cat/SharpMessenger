using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models;

namespace ChatClient.Core.Common.Interfaces
{
    public interface IExceptionHandler {
        Task<User> GreateNewUser();

        void ShowException(string message);

        void ShowMessage(string message);

        Task<bool> YesNoQuestion(string message);
    }
}
