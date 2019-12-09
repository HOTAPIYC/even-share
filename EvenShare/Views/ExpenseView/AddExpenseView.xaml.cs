using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EvenShare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddExpenseView : CustomContentPage
    {
        private ExpenseViewModel _viewModel;

        public AddExpenseView(ExpenseViewModel viewModel)
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
            await Navigation.PopAsync(true);
        }
    }
}