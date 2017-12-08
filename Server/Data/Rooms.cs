using sTalk.Libraries.Communication.Packet.Data;
using System.Collections.Generic;

namespace sTalk.Server.Data
{
    public static class Rooms
    {
        private static Database _database;

        static Rooms()
        {
            _database = null;
        }

        public static void Initialize(Database database)
        {
            if (_database == null)
                _database = database;
        }

        public static List<Room> Get()
        {
            if (_database == null)
                return null;

            var query =
                "SELECT `id`, `name`, `capacity` " +
                "FROM `rooms`;";
            var reader = _database.ExecuteReader(query);

            var rooms = new List<Room>();

            while (reader.Read())
            {
                var room = new Room()
                {
                    Id = reader.GetUInt32("id"),
                    Name = reader.GetString("name"),
                    Capacity = reader.GetUInt16("capacity"),
                    Message = null
                };

                rooms.Add(room);
            }

            reader.Close();
            return rooms;
        }

        public static Room Get(uint roomId)
        {
            if (roomId == 0)
                return null;

            var query = string.Format(
                "SELECT * " +
                "FROM `rooms` " +
                "WHERE `id` = {0};",
                roomId);
            var reader = _database.ExecuteReader(query);

            if (!reader.Read())
            {
                reader.Close();
                return null;
            }

            var room = new Room()
            {
                Id = reader.GetUInt32("id"),
                Name = reader.GetString("name"),
                Capacity = reader.GetUInt16("capacity"),
                Message = reader.GetString("message")
            };

            reader.Close();
            return room;
        }

        public static bool IsUserBanned(uint userId, uint roomId)
        {
            var query = string.Format(
                "SELECT `id` " +
                "FROM `room_bans` " +
                "WHERE `user_id` = {0} AND `room_id` = {1};",
                userId, roomId);
            var reader = _database.ExecuteReader(query);

            var banned = reader.HasRows;
            reader.Close();

            return banned;
        }
    }
}