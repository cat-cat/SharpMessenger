using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChatClient.Core.UI.Controls;
using ChatClient.Core.Common.Models;
using ChatClient.iOS.Renderers;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MessageViewCell), typeof(MessageRenderer))]
namespace ChatClient.iOS.Renderers
{
    public class MessageRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var textVm = item.BindingContext as ChatMessage;
            if (textVm != null)
            {
				var cell = tv.DequeueReusableCell(!textVm.IsMine ? ChatMessageCell.KeyLeft : ChatMessageCell.KeyRight) as ChatMessageCell;
	            if (cell == null)
	                cell = new ChatMessageCell(!textVm.IsMine);
				cell.Update(textVm);
	            return cell;
            }
            return base.GetCell(item, reusableCell, tv);
        }
    }
}