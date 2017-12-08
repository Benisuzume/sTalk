using sTalk.Libraries.Communication.Packet;
using sTalk.Libraries.Communication.Packet.Data;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sTalk.Client.Windows.Controls
{
    public partial class ContactInstance : UserControl, INotifyPropertyChanged
    {
        private Brush _statusColor;

        public ContactInstance(User user)
        {
            InitializeComponent();
            DataContext = this;

            Username = user.Name;
            DisplayName = user.Nickname ?? user.Name;

            SetStatus(user.Status);
        }

        public event Action<string> SendMessage;
        public event Action<string> Delete;
        public event Action<string> Block;
        public event PropertyChangedEventHandler PropertyChanged;

        public Brush StatusColor
        {
            get { return _statusColor; }
            private set
            {
                if (_statusColor != value)
                {
                    _statusColor = value;
                    RaisePropertyChanged("StatusColor");
                }
            }
        }

        public string Username { get; private set; }
        public string DisplayName { get; private set; }

        public void SetStatus(Status status)
        {
            switch (status)
            {
                case Status.Online:
                    StatusColor = Brushes.YellowGreen;
                    break;

                case Status.Offline:
                    StatusColor = Brushes.DarkGray;
                    break;
            }
        }

        private void mnuSendMessage_Click(object sender, RoutedEventArgs e)
        {
            SendMessage?.Invoke(Username);
        }

        private void mnuDelete_Click(object sender, RoutedEventArgs e)
        {
            Delete?.Invoke(Username);
        }

        private void mnuBlock_Click(object sender, RoutedEventArgs e)
        {
            Block?.Invoke(Username);
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}