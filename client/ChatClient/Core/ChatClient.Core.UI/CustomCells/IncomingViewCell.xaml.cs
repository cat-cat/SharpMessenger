using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatClient.Core.Common;

using Xamarin.Forms;
using System.Collections.Specialized;
using ChatClient.Core.Common.Models;

namespace ChatClient.Core.UI
{
    public partial class IncomingViewCell : ViewCell
    {
        public IncomingViewCell()
        {
            InitializeComponent();
        }

        void OnEvent(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItem = (KeyValuePair<k, object>)e.NewItems[0];
            var bc = (ChatMessage)BindingContext;
            if (newItem.Key == k.OnMessageSendProgress)
            {
                var d = (Dictionary<string, object>)newItem.Value;
                if ((string)d["guid"] == bc.guid && (ChatMessage.Status)d["status"] == ChatMessage.Status.Deleted)
                {
                    bc.Message = "<deleted>";
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    Message_Label.Text = bc.Message;
                    //});
                }
            }
            else if (newItem.Key == k.OnMessageEdit)
            {
                var d = (Dictionary<string, object>)newItem.Value;
                if ((string)d["guid"] == bc.guid)
                {
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    Message_Label.Text = (string)d["message"];
                    //});
                    bc.Message = (string)d["message"];
                }
            }
        }

        private void ViewCell_Appearing(object sender, EventArgs e)
        {
            v.h(new k[] { k.OnMessageEdit, k.OnMessageSendProgress }, OnEvent);

            var bc = (ChatMessage)BindingContext;
            v.Add(k.MessageSendProgress, bc);

			if (bc.ReplyGuid != null)
				// TODO: make this good
				bc.Message = "REPLIED: " + bc.Message + " TO: " + bc.ReplyQuote;
        }

        private void ViewCell_Disappearing(object sender, EventArgs e)
        {
            v.m(OnEvent);
        }
    }
}
