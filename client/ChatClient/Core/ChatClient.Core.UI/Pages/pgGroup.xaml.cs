using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models;
using ChatClient.Core.UI.Controls;
using ChatClient.Core.UI.ViewModels;

using Xamarin.Forms;

namespace ChatClient.Core.UI.Pages
{
    public partial class pgGroup : ContentPage
    {
        public pgGroup(Group group)
        {
            InitializeComponent();
            BindingContext=new GroupViewModel(group);
            
        }

        public pgGroup(string groupId) {
            InitializeComponent();
            BindingContext = new GroupViewModel(groupId);
        }
      
        protected override void OnBindingContextChanged()
        {
            BindingContext = BindingContext;
            base.OnBindingContextChanged();
        }

        private void VisualElement_OnSizeChanged(object sender, EventArgs e) {
         //   rwImag.Height = imgGroup.Height;
        }
    }
}
