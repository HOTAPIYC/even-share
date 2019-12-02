using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EvenShare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProjectView : ContentPage
    {
        private ProjectViewModel _viewModel;

        public EditProjectView(ProjectViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = _viewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            _viewModel.Reset();

            return base.OnBackButtonPressed();
        }
    }
}