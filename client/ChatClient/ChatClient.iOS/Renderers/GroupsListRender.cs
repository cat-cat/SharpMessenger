using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using ChatClient.iOS.Renderers;
using ChatClient.Core.UI.Controls;

using CoreGraphics;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GroupsListView), typeof(ListGroupsRenderer))]
namespace ChatClient.iOS.Renderers
{
    public class ListGroupsRenderer : ViewRenderer<ListView, UITableView> {
        public override CGSize SizeThatFits(CGSize cgSize)
        {

            if (Control != null)
                return base.SizeThatFits(cgSize);
            return SizeF.Empty;
        }
    }
  
}
