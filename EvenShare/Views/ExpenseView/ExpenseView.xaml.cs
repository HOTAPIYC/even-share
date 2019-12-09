using System.Threading.Tasks;
using Xamarin.Forms;
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

        private async void DeleteRequest(object sender, System.EventArgs e)
        {
            var answer = await DisplayAlert("", "Do you want to delete this expense?", "DELETE", "CANCEL");

            if (answer)
            {
                MessagingCenter.Send(this, "DeleteExpense");
            }
        }
    }
}