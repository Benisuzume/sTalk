using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sTalk.Client.Windows.Controls
{
    public partial class PublicMessageInstance : UserControl
    {
        public PublicMessageInstance(Brush theme, string initial, string username, string message)
        {
            InitializeComponent();
            DataContext = this;

            Brush = theme;
            Initial = initial;
            Username = username;
            Message = message;
            Time = DateTime.Now.ToString("HH:mm:ss");

            if (initial == null)
            {
                lblInitial.Visibility = Visibility.Collapsed;
                lblUsername.Visibility = Visibility.Collapsed;
                prgSending.IsActive = true;
            }

            var regex = new Regex(@"[\u0600-\u06FF\u0750-\u077F\uFB50-\uFEFF]");
            txtMessage.FlowDirection = regex.Matches(Message).Count == 0 ? FlowDirection.LeftToRight : FlowDirection.RightToLeft;
        }

        public Brush Brush { get; private set; }
        public string Initial { get; private set; }
        public string Username { get; private set; }
        public string Message { get; private set; }
        public string Time { get; private set; }

        public void Sent()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                prgSending.IsActive = false;
            }));
        }
    }
}