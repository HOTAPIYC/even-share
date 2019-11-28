using Xamarin.Forms;

namespace EvenShare
{
    public class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage(Page startupPage) : base(startupPage)
        {
            // This is just a target for the custom renderer
        }
    }
}
