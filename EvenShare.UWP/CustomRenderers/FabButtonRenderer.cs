using EvenShare;
using EvenShare.UWP.CustomRenderers;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(FabButton), typeof(FabButtonRenderer))]
namespace EvenShare.UWP.CustomRenderers
{
    public class FabButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null)
            {
                var backgroundColor = (SolidColorBrush)this.Control.BackgroundColor;

                if (backgroundColor.Color == Windows.UI.Colors.Red)
                {
                    this.Control.Style = 
                        Windows.UI.Xaml.Application.Current.Resources["RedFabButtonStyle"] as Windows.UI.Xaml.Style;
                }
                if (backgroundColor.Color == Windows.UI.Colors.LightGray)
                {
                    this.Control.Style = 
                        Windows.UI.Xaml.Application.Current.Resources["GrayFabButtonStyle"] as Windows.UI.Xaml.Style;
                }  
                // Add more conditions to support additional button styling. A style can not be changed at
                // runtime, therefore it has to be implemented individually for each variant.
            }
        }
    }
}
