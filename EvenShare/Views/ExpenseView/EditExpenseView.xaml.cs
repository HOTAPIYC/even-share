using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EvenShare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditExpenseView : CustomContentPage
    {
        private ExpenseViewModel _viewModel;

        public EditExpenseView(ExpenseViewModel viewModel)
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
            var answer = await DisplayAlert("", "Do you want to go back without saving any changes?", "GO BACK", "CANCEL");

            if (answer)
            {
                _viewModel.Reset();
                await Navigation.PopAsync(true);
            }
        }
    }
}