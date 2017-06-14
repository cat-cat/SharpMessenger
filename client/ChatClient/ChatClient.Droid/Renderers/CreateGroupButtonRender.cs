#region

using ChatClient.Core.UI.Controls;
using ChatClient.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

#endregion

[assembly: ExportRenderer(typeof(CreateGroupButton), typeof(CreateGroupButtonRender))]

namespace ChatClient.Droid.Renderers {
    public class CreateGroupButtonRender : ButtonRenderer {
        #region Fields

        private Android.Widget.Button btn;

        #endregion

        #region Protected Methods and Operators

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e) {
            base.OnElementChanged(e);

            if (e.OldElement == null) {
                btn = Control;
               
                btn.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.create_group_button_normal));//setbSetBackgroundImage(UIImage.FromFile("create_group_button_normal.png"), UIControlState.Normal);
            }
        }

        #endregion
    }
}