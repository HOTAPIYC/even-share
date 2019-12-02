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
    public partial class StatisticsView : ContentPage
    {
        private StatisticsViewModel _viewModel;
        
        public StatisticsView(StatisticsViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = _viewModel;

            Task.Run(async () => { await _viewModel.Init(); }).Wait();

            foreach (string textline in _viewModel.IndividualTotals)
            {
                var label = new Label();
                label.Text = textline;
                MembersSpending.Children.Add(label);
            }

            foreach (string textline in _viewModel.Results)
            {
                var label = new Label();
                label.Text = textline;
                MembersShare.Children.Add(label);
            }
        }
    }
}