using sTalk.Client.Communication;
using sTalk.Libraries.Communication;
using sTalk.Libraries.Communication.Packet;
using System;
using System.Windows;

namespace sTalk.Client.Windows
{
    public partial class LogInWindow
    {
        private Server _server;

        public LogInWindow(Server server)
        {
            InitializeComponent();

            _server = server;
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            var username = Sanitize.Username(txtUsername.Text);
            if (username == null)
            {
                txtUsername.Text = "";
                txtUsername.Focus();
                return;
            }

            var password = txtPassword.Password.Trim();
            if (string.IsNullOrEmpty(password))
            {
                txtPassword.Password = "";
                txtPassword.Focus();
                return;
            }

            Busy(true);
            lblStatus.Text = "Connecting to the server...";

            _server.LogIn(username, password, ((result, rooms, contacts, blocks) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    switch (result)
                    {
                        case Result.Failure:
                            Busy(false);
                            lblStatus.Text = "Unable to connect to server, please try again later.";
                            break;

                        case Result.NotFound:
                        case Result.Wrong:
                            txtUsername.Text = "";
                            txtPassword.Password = "";
                            Busy(false);
                            txtUsername.Focus();
                            lblStatus.Text = "You have entered an invalid username or password.";
                            break;

                        case Result.Banned:
                            Busy(false);
                            lblStatus.Text = "Sorry, your account has been banned.";
                            break;

                        case Result.Success:
                            var main = new MainWindow(_server, rooms, contacts, blocks);
                            Close();
                            main.Show();
                            break;
                    }
                }));
            }));
        }

        private void lnkForgotPassowrd_Click(object sender, RoutedEventArgs e)
        {
            lblStatus.Text = "lnkForgotPassowrd_Click";
        }

        private void lnkDontHaveAnAccount_Click(object sender, RoutedEventArgs e)
        {
            lblStatus.Text = "lnkDontHaveAnAccount_Click";
        }

        private void Busy(bool busy)
        {
            busy = !busy;

            txtUsername.IsEnabled = busy;
            txtPassword.IsEnabled = busy;
            btnLogIn.IsEnabled = busy;
            btnLogIn.Visibility = busy ? Visibility.Visible : Visibility.Hidden;
            prgBusy.IsActive = !busy;
        }
    }
}