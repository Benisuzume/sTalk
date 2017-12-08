using sTalk.Libraries.Communication.Packet.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace sTalk.Client.Windows.Controls
{
    public partial class RoomInstance : UserControl
    {
        public RoomInstance(Room room)
        {
            InitializeComponent();
            DataContext = this;

            Room = room;
        }

        public event Action<Room> Join;

        public Room Room { get; private set; }

        public bool Busy
        {
            set
            {
                btnJoin.IsEnabled = !value;
            }
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            Join(Room);
        }
    }
}