using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EvenShare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddExpenseView : ContentPage
    {
        private ExpenseViewModel _viewModel;

        public AddExpenseView(ExpenseViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = _viewModel;
        }
    }
}