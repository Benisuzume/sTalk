using sTalk.Libraries.Communication.Packet;
using sTalk.Libraries.Communication.Packet.Data;
using System.Collections.Generic;

namespace sTalk.Server.Data
{
    public static class Contacts
    {
        private static Database _database;

        static Contacts()
        {
            _database = null;
        }

        public static void Initialize(Database database)
        {
            if (_database == null)
                _database = database;
        }

        public static List<User> GetContacts(uint userId)
        {
            if (_database == null)
                return null;

            var query = string.Format(
                "SELECT `users`.`id`, `users`.`username`, `users`.`nickname` " +
                "FROM `users` " +
                "LEFT JOIN `contacts` " +
                "ON `contacts`.`contact_id` = `users`.`id` " +
                "WHERE `contacts`.`user_id` = {0} and `contacts`.`blocked` = 0;",
                userId);
            var reader = _database.ExecuteReader(query);

            var contacts = new List<User>();

            while (reader.Read())
            {
                var contact = new User()
                {
                    Id = reader.GetUInt32("id"),
                    Name = reader.GetString("username"),
                    Nickname = reader.GetString("nickname", null)
                };

                contact.Status = Communication.Server.IsOnline(contact) ? Status.Online : Status.Offline;
                contacts.Add(contact);
            }

            reader.Close();
            return contacts;
        }

        public static List<string> GetBlocks(uint userId)
        {
            if (_database == null)
                return null;

            var query = string.Format(
                "SELECT `users`.`username` " +
                "FROM `users` " +
                "LEFT JOIN `contacts` " +
                "ON `contacts`.`contact_id` = `users`.`id` " +
                "WHERE `contacts`.`user_id` = {0} and `contacts`.`blocked` = 1;",
                userId);
            var reader = _database.ExecuteReader(query);

            var blocks = new List<string>();

            while (reader.Read())
            {
                var block = reader.GetString("username");

                blocks.Add(block);
            }

            reader.Close();
            return blocks;
        }

        public static void Add(uint userId, uint contactId)
        {
            if (_database == null)
                return;

            Unblock(userId, contactId);

            var query = string.Format(
                "INSERT INTO `contacts` (" +
                "`user_id`, `contact_id`, `blocked`" +
                ") VALUES (" +
                "{0}, {1}, 0" +
                ");",
                userId, contactId);
            _database.ExecuteNonQuery(query);
        }

        public static void Delete(uint userId, uint contactId)
        {
            if (_database == null)
                return;

            var query = string.Format(
                "DELETE FROM `contacts` " +
                "WHERE `user_id` = {0} AND `contact_id` = {1} AND `blocked` = 0;",
                userId, contactId);
            _database.ExecuteNonQuery(query);
        }

        public static void Block(uint userId, uint contactId)
        {
            if (_database == null)
                return;

            Delete(userId, contactId);
            Delete(contactId, userId);

            var query = string.Format(
                "INSERT INTO `contacts` (" +
                "`user_id`, `contact_id`, `blocked`" +
                ") VALUES (" +
                "{0}, {1}, 1" +
                ");",
                userId, contactId);
            _database.ExecuteNonQuery(query);
        }

        public static void Unblock(uint userId, uint contactId)
        {
            if (_database == null)
                return;

            var query = string.Format(
                "DELETE FROM `contacts` " +
                "WHERE `user_id` = {0} AND `contact_id` = {1} AND `blocked` = 1;",
                userId, contactId);
            _database.ExecuteNonQuery(query);
        }

        public static bool IsContact(uint userId, uint contactId)
        {
            if (_database == null)
                return false;

            var query = string.Format(
                "SELECT `id` " +
                "FROM `contacts` " +
                "WHERE `user_id` = {0} AND `contact_id` = {1} AND `blocked` = 0;",
                userId, contactId);
            var reader = _database.ExecuteReader(query);

            var exists = reader.HasRows;
            reader.Close();

            return exists;
        }

        public static bool IsBlocked(uint userId, uint contactId)
        {
            if (_database == null)
                return false;

            var query = string.Format(
                "SELECT `id` " +
                "FROM `contacts` " +
                "WHERE `user_id` = {0} AND `contact_id` = {1} AND `blocked` = 1;",
                userId, contactId);
            var reader = _database.ExecuteReader(query);

            var blocked = reader.HasRows;
            reader.Close();

            return blocked;
        }
    }
}