﻿using EvenShare.Strings;
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
            var answer = await DisplayAlert(
                "",
                AppResources.DialogCancelEdit,
                AppResources.DialogGoBack,
                AppResources.DialogCancel);

            if (answer)
            {
                _viewModel.Reset();
                await Navigation.PopAsync(true);
            }
        }
    }
}