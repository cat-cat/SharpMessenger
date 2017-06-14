using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Transitions;
using Android.Util;
using Android.Views;
using Android.Widget;

using ChatClient.Core.UI.Controls;
using ChatClient.Core.UI.Pages;
using ChatClient.Droid.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GroupListImage), typeof(GroupListImageRender))]
namespace ChatClient.Droid.Renderers
{
   public class GroupListImageRender:ImageRenderer {
        Page page;
        NavigationPage navigPage;
       private ViewCell lviewCell;
       private Xamarin.Forms.Image limage;
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Image> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null) {

                limage = e.NewElement;
                lviewCell = GetContainingViewCell(e.NewElement);
                if (lviewCell != null)
                {
                    lviewCell.Disappearing += LCell_Disappearing;
                    page = GetContainingPage(e.NewElement);
                    if (page.Parent is TabbedPage)
                    {
                        page.Disappearing += PageContainedInTabbedPageDisapearing;
                        return;
                    }

                    navigPage = GetContainingNavigationPage(page);
                    if (navigPage != null)
                        navigPage.Popped += OnPagePopped;
                }
                else if ((page = GetContainingTabbedPage(e.NewElement)) != null)
                {
                    page.Disappearing += PageContainedInTabbedPageDisapearing;
                }
            }
        }

        private void LCell_Disappearing(object sender, EventArgs e) {
            limage = null;
        }

        void PageContainedInTabbedPageDisapearing(object sender, EventArgs e)
        {
            this.Dispose(true);
            page.Disappearing -= PageContainedInTabbedPageDisapearing;
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            limage = null;
        }

        private void OnPagePopped(object s, NavigationEventArgs e)
        {
            if (e.Page == page)
            {
                this.Dispose(true);
                navigPage.Popped -= OnPagePopped;
            }
        }

        private Page GetContainingPage(Xamarin.Forms.Element element)
        {
            Element parentElement = element.ParentView;
            if (typeof(Page).IsAssignableFrom(parentElement.GetType()))
                return (Page)parentElement;
            else
                return GetContainingPage(parentElement);
        }

        private ViewCell GetContainingViewCell(Xamarin.Forms.Element element)
        {
            Element parentElement = element.Parent;

            if (parentElement == null)
                return null;

            if (typeof(ViewCell).IsAssignableFrom(parentElement.GetType()))
                return (ViewCell)parentElement;
            else
                return GetContainingViewCell(parentElement);
        }

        private TabbedPage GetContainingTabbedPage(Element element)
        {
            Element parentElement = element.Parent;

            if (parentElement == null)
                return null;

            if (typeof(TabbedPage).IsAssignableFrom(parentElement.GetType()))
                return (TabbedPage)parentElement;
            else
                return GetContainingTabbedPage(parentElement);
        }

        private NavigationPage GetContainingNavigationPage(Element element)
        {
            Element parentElement = element.Parent;

            if (parentElement == null)
                return null;

            if (typeof(NavigationPage).IsAssignableFrom(parentElement.GetType()))
                return (NavigationPage)parentElement;
            else
                return GetContainingNavigationPage(parentElement);
        }
    }
}