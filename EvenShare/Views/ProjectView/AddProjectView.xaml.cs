using Xamarin.Forms.Xaml;

namespace EvenShare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProjectView : CustomContentPage
    {
        private ProjectViewModel _viewModel;

        public AddProjectView(ProjectViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = _viewModel;

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