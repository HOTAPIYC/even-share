using EvenShare.Strings;
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

            // Make sure view model is fully loaded,
            // as this view model is also used by the sub pages.
            Task.Run(async () => { await _viewModel.Init(); }).Wait();
        }

        private async void GoBack()
        {
            await Navigation.PopAsync(true);
        }

        private async void DeleteRequest(object sender, System.EventArgs e)
        {
            var answer = await DisplayAlert(
                "",
                AppResources.DialogDeleteExpense,
                AppResources.DialogDelete,
                AppResources.DialogCancel);

            if (answer && _viewModel.DeleteExpense.CanExecute(null))
            {
                _viewModel.DeleteExpense.Execute(null);
            }
        }
    }
}