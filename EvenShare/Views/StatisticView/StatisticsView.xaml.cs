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
    public partial class StatisticsView : CustomContentPage
    {
        private StatisticsViewModel _viewModel;
        
        public StatisticsView(StatisticsViewModel viewModel)
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

            foreach (string textline in _viewModel.IndividualTotals)
            {
                var label = new Label();
                label.Text = textline;
                label.FontSize = 17;
                MembersSpending.Children.Add(label);
            }

            foreach (string textline in _viewModel.Shares)
            {
                var label = new Label();
                label.Text = textline;
                label.FontSize = 17;
                MembersShare.Children.Add(label);
            }
        }

        private async void GoBack()
        {
            await Navigation.PopAsync(true);
        }
    }
}