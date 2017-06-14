using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using ChatClient.Core.UI.Controls;
using ChatClient.Droid.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(FilterButton), typeof(FilterButtonRenderer))]
namespace ChatClient.Droid.Renderers
{
    public class FilterButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null) {
                this.Control.Gravity = GravityFlags.Left;
            }
        }
    }
}