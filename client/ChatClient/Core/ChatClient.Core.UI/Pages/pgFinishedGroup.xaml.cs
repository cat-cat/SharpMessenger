using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models;
using ChatClient.Core.UI.ViewModels;

using Xamarin.Forms;

namespace ChatClient.Core.UI.Pages
{
    public partial class pgFinishedGroup : ContentPage
    {
        public pgFinishedGroup(Group group)
        {
            InitializeComponent();
            BindingContext = new GroupViewModel(group,true);
        }
        public pgFinishedGroup(string groupId)
        {
            InitializeComponent();
            BindingContext = new GroupViewModel(groupId);
        }
        private void VisualElement_OnSizeChanged(object sender, EventArgs e) {
            //rwImag.Height = imgGroup.Height;
        }
    }
}
