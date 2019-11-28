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
    public partial class ProjectView : ContentPage
    {
        private ProjectViewModel _viewModel;

        public ProjectView(ProjectViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = _viewModel;

            Task.Run(async () => { await _viewModel.Init(); }).Wait();
        }
    }
}