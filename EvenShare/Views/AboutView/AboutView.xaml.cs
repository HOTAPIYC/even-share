using EvenShare.Strings;
using Xamarin.Forms.Xaml;

namespace EvenShare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutView : CustomContentPage
    {
        public AboutView()
        {
            InitializeComponent();

            // This is just a static page, no view model here.

            if (EnableBackButtonOverride)
            {
                CustomBackButtonAction = () => { GoBack(); };
            }
        }

        private async void GoBack()
        {
            await Navigation.PopAsync(true);
        }
    }
}