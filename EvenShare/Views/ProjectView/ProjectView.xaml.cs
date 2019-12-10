using System.Threading.Tasks;
using Xamarin.Forms;
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

            Task.Run(async () => { await _viewModel.Init(); }).Wait();
        }

        private async void DeleteRequest(object sender, System.EventArgs e)
        {
            var answer = await DisplayAlert("", "Do you want to delete this project?", "DELETE", "CANCEL");

            if (answer && _viewModel.DeleteProject.CanExecute(null))
            {
                _viewModel.DeleteProject.Execute(null);
            }
        }
    }
}