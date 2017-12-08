using MahApps.Metro.Controls;
using sTalk.Client.Communication;
using sTalk.Libraries.Communication.Packet;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace sTalk.Client.Windows.Controls
{
    public partial class PublicChatInstance : UserControl
    {
        private Server _server;
        private Dictionary<string, PublicMessageInstance> _messages;
        private Regex _regex;
        private RoomUserInstance _self;

        public PublicChatInstance(Server server, string message, List<string> usernames)
        {
            InitializeComponent();

            _server = server;
            _server.PublicMessageReceived += PublicMessageReceived;
            _server.PublicMessageResultReceived += PublicMessageResultReceived;
            _server.UserJoinedRoom += UserJoinedRoom;
            _server.UserLeftRoom += UserLeftRoom;

            _messages = new Dictionary<string, PublicMessageInstance>();
            _regex = new Regex(@"[\u0600-\u06FF\u0750-\u077F\uFB50-\uFEFF]");

            if (!string.IsNullOrWhiteSpace(message))
            {
                // var publicMessage = new PublicMessageInstance(Brushes.AliceBlue ,"[System Message]", message);
                // pnlMessages.Children.Add(publicMessage);
            }

            _self = new RoomUserInstance(null, null);
            pnlUsers.Children.Add(_self);

            usernames.ForEach(((username) =>
            {
                var userInstance = new RoomUserInstance(username, "Status goes here...");
                pnlUsers.Children.Add(userInstance);
            }));
        }

        private void PublicChatInstance_Unloaded(object sender, RoutedEventArgs e)
        {
            _server.PublicMessageReceived -= PublicMessageReceived;
            _server.PublicMessageResultReceived -= PublicMessageResultReceived;
            _server.UserJoinedRoom -= UserJoinedRoom;
            _server.UserLeftRoom -= UserLeftRoom;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0)
            {
                var scrollViewer = (ScrollViewer)sender;
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ExtentHeight);
            }
        }

        private void txtMessage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Shift)
                return;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                var message = txtMessage.Text.Trim();
                if (string.IsNullOrEmpty(message))
                    txtMessage.Text = "";
                else
                    SendMessage(message);
            }
        }

        private void txtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            var message = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(message))
            {
                TextBoxHelper.SetWatermark(txtMessage, "Message...");

                txtMessage.FlowDirection = FlowDirection.LeftToRight;
                return;
            }

            TextBoxHelper.SetWatermark(txtMessage, "Press enter to send message");

            txtMessage.FlowDirection = _regex.Matches(message).Count == 0 ? FlowDirection.LeftToRight : FlowDirection.RightToLeft;
        }

        private void PublicMessageReceived(string username, string message)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var userInstance = FindUserInstance(username);
                var publicMessage = userInstance.PublicMessage(message);

                pnlMessages.Children.Add(publicMessage);
            }));
        }

        private void PublicMessageResultReceived(Result result, string guid)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var publicMessage = _messages[guid];
                _messages.Remove(guid);

                //todo: Check result, if Result.Banned...

                publicMessage.Sent();
            }));
        }

        private void UserJoinedRoom(string username)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var userInstance = new RoomUserInstance(username, "Status goes here...");
                pnlUsers.Children.Add(userInstance);
            }));
        }

        private void UserLeftRoom(string username)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var userInstance = FindUserInstance(username);
                pnlUsers.Children.Remove(userInstance);
            }));
        }

        private void SendMessage(string message)
        {
            var guid = Guid.NewGuid().ToString("N");

            var publicMessage = _self.PublicMessage(message);
            _messages.Add(guid, publicMessage);

            pnlMessages.Children.Add(publicMessage);
            _server.PublicMessage(guid, message);

            txtMessage.Text = "";
            txtMessage.Focus();
        }

        private RoomUserInstance FindUserInstance(string username)
        {
            foreach (RoomUserInstance userInstance in pnlUsers.Children)
            {
                if (userInstance.Username == username)
                {
                    return userInstance;
                }
            }

            return null;
        }
    }
}