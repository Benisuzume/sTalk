using Client.Properties;
using MahApps.Metro;
using sTalk.Client.Communication;
using sTalk.Client.Windows;
using System.Windows;

namespace sTalk.Client
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Current, ThemeManager.GetAccent("Blue"), ThemeManager.GetAppTheme("BaseLight"));

            var ipAddress = "192.168.1.100";
            var server = new Server(Settings.Default.PublicKey, ipAddress, Settings.Default.ServerPort);

            var logIn = new LogInWindow(server);
            logIn.Show();
        }
    }
}