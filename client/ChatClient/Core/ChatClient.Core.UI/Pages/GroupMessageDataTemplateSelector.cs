using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChatClient.Core.UI { 
    class GroupMessageDataTemplateSelector : Xamarin.Forms.DataTemplateSelector
    {
        public GroupMessageDataTemplateSelector()
        {
            // Retain instances!
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Common.Models.ChatMessage;
            if (messageVm == null)
                return null;
            return messageVm.IsMine ? this.outgoingDataTemplate : this.incomingDataTemplate;
        }

        private readonly DataTemplate incomingDataTemplate;
        private readonly DataTemplate outgoingDataTemplate;
    }
}
