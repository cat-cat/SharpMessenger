#region

using ChatClient.iOS.Renderers;
using ChatClient.Core.UI.Controls;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#endregion

[assembly: ExportRenderer(typeof(CreateGroupButton), typeof(CreateGroupButtonRender))]

namespace ChatClient.iOS.Renderers {
    public class CreateGroupButtonRender : ButtonRenderer {
        #region Fields

        private UIButton btn;

        #endregion

        #region Constractors and Destructors

        public CreateGroupButtonRender() {
        }

        #endregion

        #region Protected Methods and Operators

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e) {
            base.OnElementChanged(e);

            if (e.OldElement == null) {
                btn = (UIButton)Control;
                btn.SetBackgroundImage(UIImage.FromFile("create_group_button_normal.png"), UIControlState.Normal);
            }
        }

        #endregion
    }
}