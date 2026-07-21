using JobSearchApp.Data;
using JobSearchApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace JobSearchApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                bool isConfigured;

                using (var db = new AppDbContext())
                {
                    db.Database.Migrate();

                    isConfigured = db.AppSettings.Find(1)?.IsConfigured ?? false;
                }

                Window startWindow = isConfigured ? new MainWindow() : new SetupWizardWindow();
                MainWindow = startWindow;
                startWindow.Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    $"Aplikaci se nepodařilo inicializovat.\n\n{exception.Message}\n\nDatabáze: {AppDbContext.DatabasePath}",
                    "WorkScout – chyba při spuštění",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Shutdown(1);
            }
        }
    }
}
