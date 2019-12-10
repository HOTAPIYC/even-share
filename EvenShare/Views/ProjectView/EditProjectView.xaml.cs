using Xamarin.Forms;
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

        private void Cancel(object sender, System.EventArgs e)
        {
            GoBack();
        }

        private async void GoBack()
        {
            var answer = await DisplayAlert("", "Do you want to go back without saving any changes?", "GO BACK", "CANCEL");

            if (answer)
            {
                _viewModel.Reset();
                await Navigation.PopAsync(true);
            }
        }

        private async void DeleteMemberRequest(object sender, System.EventArgs e)
        {
            var answer = await DisplayAlert("", "Do you want to delete this project member and all connected expenses?", "DELETE", "CANCEL");

            if (answer && _viewModel.DeleteMember.CanExecute(null))
            {
                _viewModel.DeleteMember.Execute(null);
            }
        }
    }
}