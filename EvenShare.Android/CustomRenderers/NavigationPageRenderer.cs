using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EvenShare;
using EvenShare.Droid.CustomRenderers;
using Xamarin.Forms;
using AppCompToolbar = Android.Support.V7.Widget.Toolbar;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(NavigationPageRenderer))]
namespace EvenShare.Droid.CustomRenderers
{
    public class NavigationPageRenderer : Xamarin.Forms.Platform.Android.AppCompat.NavigationPageRenderer
    {
        public AppCompToolbar toolbar;

        public NavigationPageRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var context = (Activity)base.Context;
            toolbar = context.FindViewById<AppCompToolbar>(Droid.Resource.Id.toolbar);

            if (toolbar != null)
            {
                for (int index = 0; index < toolbar.ChildCount; index++)
                {
                    if (toolbar.GetChildAt(index) is TextView)
                    {
                        var title = toolbar.GetChildAt(index) as TextView;
                        float toolbarCenter = toolbar.MeasuredWidth / 2;
                        float titleCenter = title.MeasuredWidth / 2;
                        title.SetX(toolbarCenter - titleCenter);
                        title.SetTextColor(Android.Graphics.Color.Black);

                        if (toolbar.NavigationIcon != null)
                        {
                            toolbar.NavigationIcon.SetColorFilter(
                                Android.Graphics.Color.Black,
                                Android.Graphics.PorterDuff.Mode.SrcAtop);
                        }
                        if (toolbar.OverflowIcon != null)
                        {
                            toolbar.OverflowIcon.SetColorFilter(
                                Android.Graphics.Color.Black,
                                Android.Graphics.PorterDuff.Mode.SrcAtop);
                        }
                    }
                }              
            }
        }
    }
}