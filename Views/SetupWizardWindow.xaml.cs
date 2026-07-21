using JobSearchApp.ViewModels;
using JobSearchApp.Services;
using System.Windows;

namespace JobSearchApp.Views
{
    public partial class SetupWizardWindow : Window
    {
        private readonly SetupWizardViewModel _viewModel = new();

        public SetupWizardWindow()
        {
            InitializeComponent();
            Title = $"První nastavení – {ApplicationVersionService.DisplayName}";
            DataContext = _viewModel;
            _viewModel.SetupCompleted += OnSetupCompleted;
        }

        // PasswordBox.Password se z bezpečnostních důvodů nedá bindovat přímo -
        // tohle je standardní obchvat: při každé změně přepíšeme hodnotu do ViewModelu.
        private void AppPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.AppPassword = AppPasswordBox.Password;
        }

        private void OnSetupCompleted()
        {
            var main = new MainWindow();
            main.Show();
            Close();
        }
    }
}
