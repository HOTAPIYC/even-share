using EvenShare.Strings;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace EvenShare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectView : CustomContentPage
    {
        private ProjectViewModel _viewModel;

        public ProjectView(ProjectViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = _viewModel;

            // Make sure view model is fully loaded,
            // as this view model is also used by the sub pages.
            Task.Run(async () => { await _viewModel.Init(); }).Wait();
        }

        private async void DeleteRequest(object sender, System.EventArgs e)
        {
            var answer = await DisplayAlert(
                "", 
                AppResources.DialogDeleteProject, 
                AppResources.DialogDelete, 
                AppResources.DialogCancel);

            if (answer && _viewModel.DeleteProject.CanExecute(null))
            {
                _viewModel.DeleteProject.Execute(null);
            }
        }
    }
}