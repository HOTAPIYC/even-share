using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace EvenShare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpenseView : CustomContentPage
    {
        private ExpenseViewModel _viewModel;

        public ExpenseView(ExpenseViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = _viewModel;

            if (EnableBackButtonOverride)
            {
                CustomBackButtonAction = () => { GoBack(); };
            }

            Task.Run(async () => { await _viewModel.Init(); }).Wait();
        }

        private async void GoBack()
        {
            await Navigation.PopAsync(true);
        }
    }
}