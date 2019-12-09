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

            Task.Run(async () => { await _viewModel.Init(); }).Wait();
        }
    }
}