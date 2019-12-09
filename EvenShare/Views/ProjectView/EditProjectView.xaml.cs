using Xamarin.Forms.Xaml;

namespace EvenShare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProjectView : CustomContentPage
    {
        private ProjectViewModel _viewModel;

        public EditProjectView(ProjectViewModel viewModel)
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
            _viewModel.Reset();
            await Navigation.PopAsync(true);
        }
    }
}