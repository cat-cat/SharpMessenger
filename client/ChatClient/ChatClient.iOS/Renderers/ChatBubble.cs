using System;
using Xamarin.Forms;
using Foundation;

using MonoTouch.Dialog;

using UIKit;

namespace ChatClient.iOS.Renderers
{
    public class ChatBubble : MonoTouch.Dialog.Element, IElementSizing
    {
        bool isLeft;
        private string _nickname;
        private DateTime _timestamp;
        private string _image;
        public ChatBubble(bool isLeft, string text,string nickname,DateTime timestamp,string image)
            : base(text)
        {
            this.isLeft = isLeft;
            _nickname = nickname;
            _timestamp = timestamp;
            _image = image;
        }


        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(isLeft ? BubbleCell.KeyLeft : BubbleCell.KeyRight) as BubbleCell;
            if (cell == null)
                cell = new BubbleCell(isLeft);
            cell.Update(Caption,_nickname,_timestamp,_image);
            return cell;
        }

        public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return BubbleCell.GetSizeForText(tableView, Caption).Height + BubbleCell.BubblePadding.Height;
        }
    }
}