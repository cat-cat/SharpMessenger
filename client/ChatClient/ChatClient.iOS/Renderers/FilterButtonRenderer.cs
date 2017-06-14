using System;
using System.Collections.Generic;
using System.Text;

using ChatClient.Core.UI.Controls;
using ChatClient.iOS.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FilterButton), typeof(FilterButtonRenderer))]
namespace ChatClient.iOS.Renderers
{
  public  class FilterButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null) {
                Control.TitleLabel.LineBreakMode = UIKit.UILineBreakMode.WordWrap;
            }
        }
    }
}
