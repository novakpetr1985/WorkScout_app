using WorkScout.ViewModels;
using WorkScout.Services;
using System.Windows;

namespace WorkScout.Views
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

        // SECURITY: WPF PasswordBox záměrně nevystavuje bindovatelnou dependency property.
        // Code-behind pouze předá aktuální hodnotu ViewModelu; heslo se nezapisuje do XAML.
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
