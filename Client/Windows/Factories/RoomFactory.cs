using sTalk.Client.Windows.Controls;
using sTalk.Libraries.Communication.Packet.Data;
using System;
using System.Collections.ObjectModel;

namespace sTalk.Client.Windows.Factories
{
    public static class RoomFactory
    {
        private static ObservableCollection<RoomInstance> _rooms = new ObservableCollection<RoomInstance>();

        public static ObservableCollection<RoomInstance> Rooms
        {
            get
            {
                return _rooms;
            }
        }

        public static bool Busy
        {
            set
            {
                foreach (var room in _rooms)
                    room.Busy = value;
            }
        }

        public static void Create(Room room, Action<Room> join)
        {
            var contact = new RoomInstance(room);
            contact.Join += join;

            for (int i = 0; i < _rooms.Count; i++)
            {
                if (string.Compare(_rooms[i].Room.Name, room.Name) > 0)
                {
                    _rooms.Insert(i, contact);
                    return;
                }
            }

            _rooms.Add(contact);
        }
    }
}