using WorkScout.ViewModels;
using System.Windows;

namespace WorkScout.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
            _viewModel.ResetRequested += OnResetRequested;
        }

        private void OnResetRequested()
        {
            var wizard = new SetupWizardWindow();
            wizard.Show();
            Close();
        }
    }
}
