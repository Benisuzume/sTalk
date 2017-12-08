using sTalk.Client.Communication;
using sTalk.Client.Windows.Controls;
using sTalk.Client.Windows.Factories;
using sTalk.Libraries.Communication;
using sTalk.Libraries.Communication.Packet;
using sTalk.Libraries.Communication.Packet.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace sTalk.Client.Windows
{
    public partial class MainWindow
    {
        private Server _server;
        private TabItem _roomTab;

        public MainWindow(Server server, List<Room> rooms, List<User> contacts, List<string> blocks)
        {
            InitializeComponent();

            _server = server;
            _server.UserStatusChanged += UserStatusChanged;

            // Load rooms
            rooms.ForEach(
                (room) => RoomFactory.Create(room, btnJoin_Click)
                );

            // Load contacts
            contacts.ForEach(
                (user) => ContactFactory.Create(user, mnuSendMessage_Click, mnuDelete_Click, mnuBlock_Click)
                );
        }

        private void UserStatusChanged(string username, Status status)
        {
            var contactInstance = ContactFactory.Find(username);
            if (contactInstance == null)
                return;

            contactInstance.SetStatus(status);
        }

        private void mnuSendMessage_Click(string username)
        {

        }

        private void mnuDelete_Click(string username)
        {
            _server.DeleteContact(username, ((result) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    switch (result)
                    {
                        case Result.Failure:
                            lblStatus.Text = "Unable to connect to server, please try again later.";
                            break;

                        case Result.Useless:
                            lblStatus.Text = "Account does not exist in your contacts.";
                            break;

                        case Result.NotFound:
                            lblStatus.Text = "Username not found.";
                            break;

                        case Result.Success:
                            ContactFactory.Delete(username);
                            break;
                    }
                }));
            }));
        }

        private void mnuBlock_Click(string username)
        {
            _server.BlockUser(username, ((result) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    switch (result)
                    {
                        case Result.Failure:
                            lblStatus.Text = "Unable to connect to server, please try again later.";
                            break;

                        case Result.Useless:
                            lblStatus.Text = "You are not in any room.";
                            break;

                        case Result.NotFound:
                            lblStatus.Text = "Username not found.";
                            break;

                        case Result.Success:
                            ContactFactory.Delete(username);
                            break;
                    }
                }));
            }));
        }

        private void btnBlocks_Click(object sender, RoutedEventArgs e)
        {
            if (flyContacts.IsOpen)
                flyContacts.IsOpen = false;

            if (flyRooms.IsOpen)
                flyRooms.IsOpen = false;

            flyBlocks.IsOpen = !flyBlocks.IsOpen;
        }

        private void btnContacts_Click(object sender, RoutedEventArgs e)
        {
            if (flyBlocks.IsOpen)
                flyBlocks.IsOpen = false;

            if (flyRooms.IsOpen)
                flyRooms.IsOpen = false;

            flyContacts.IsOpen = !flyContacts.IsOpen;
        }

        private void btnRooms_Click(object sender, RoutedEventArgs e)
        {
            if (flyBlocks.IsOpen)
                flyBlocks.IsOpen = false;

            if (flyContacts.IsOpen)
                flyContacts.IsOpen = false;

            flyRooms.IsOpen = !flyRooms.IsOpen;
        }

        private void btnLeaveRoom_Click(object sender, RoutedEventArgs e)
        {
            btnLeaveRoom.IsEnabled = false;

            _server.LeaveRoom((result) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    switch (result)
                    {
                        case Result.Failure:
                            lblStatus.Text = "Unable to connect to server, please try again later.";
                            break;

                        case Result.Useless:
                            lblStatus.Text = "You are not in any room.";
                            break;

                        case Result.Success:
                            btnLeaveRoom.Visibility = Visibility.Collapsed;
                            btnRooms.Visibility = Visibility.Visible;
                            tabMain.Items.Remove(_roomTab);
                            _roomTab = null;
                            break;
                    }

                    btnLeaveRoom.IsEnabled = true;
                }));
            });
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var username = Sanitize.Username(txtUsername.Text);
            if (username == null)
            {
                txtUsername.Text = "";
                txtUsername.Focus();
                return;
            }

            btnAdd.IsEnabled = false;

            _server.AddContact(username, ((result, user) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    switch (result)
                    {
                        case Result.Failure:
                            lblStatus.Text = "Unable to connect to server, please try again later.";
                            break;

                        case Result.Useless:
                            lblStatus.Text = "This account is already in your contact list.";
                            break;

                        case Result.NotFound:
                            lblStatus.Text = "Username not found.";
                            break;

                        case Result.Banned:
                            lblStatus.Text = "Sorry, you are blocked by this account.";
                            break;

                        case Result.Success:
                            ContactFactory.Create(user, mnuSendMessage_Click, mnuDelete_Click, mnuBlock_Click);

                            lblStatus.Text = "Account succesfully added to your contact list.";
                            txtUsername.Text = "";
                            break;
                    }

                    btnAdd.IsEnabled = true;
                }));
            }));
        }

        private void btnJoin_Click(Room room)
        {
            RoomFactory.Busy = true;

            _server.JoinRoom(room.Id, ((result, message, usernames) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    switch (result)
                    {
                        case Result.Failure:
                            lblStatus.Text = "Unable to connect to server, please try again later.";
                            break;

                        case Result.Useless:
                            lblStatus.Text = "You are already in a room.";
                            break;

                        case Result.NotFound:
                            lblStatus.Text = "Cannot join this room at the moment, please try again later.";
                            break;

                        case Result.Banned:
                            lblStatus.Text = "Sorry, you have been banned from this room.";
                            break;

                        case Result.Full:
                            lblStatus.Text = "This room is full, please try another room.";
                            break;

                        case Result.Success:
                            flyRooms.IsOpen = false;
                            btnRooms.Visibility = Visibility.Collapsed;
                            btnLeaveRoom.Visibility = Visibility.Visible;

                            _roomTab = new TabItem()
                            {
                                Header = room.Name,
                                Content = new PublicChatInstance(_server, message, usernames),
                                IsSelected = true
                            };

                            tabMain.Items.Add(_roomTab);
                            break;
                    }

                    RoomFactory.Busy = false;
                }));
            }));
        }
    }
}