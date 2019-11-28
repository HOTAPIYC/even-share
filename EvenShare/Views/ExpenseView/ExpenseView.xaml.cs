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
    public partial class ExpenseView : ContentPage
    {
        private ExpenseViewModel _viewModel;

        public ExpenseView(ExpenseViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = _viewModel;

            Task.Run(async () => { await _viewModel.Init(); }).Wait();
        }
    }
}