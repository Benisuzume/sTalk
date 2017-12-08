using sTalk.Libraries.Communication.Packet.Data;

namespace sTalk.Server.Data
{
    public static class Accounts
    {
        private static Database _database;

        static Accounts()
        {
            _database = null;
        }

        public static void Initialize(Database database)
        {
            if (_database == null)
                _database = database;
        }

        public static Account Get(string username)
        {
            if (_database == null)
                return null;

            var query = string.Format(
                "SELECT * " +
                "FROM `users` " +
                "WHERE `username` = '{0}';",
                username);
            var reader = _database.ExecuteReader(query);

            if (!reader.Read())
            {
                reader.Close();
                return null;
            }

            var account = new Account()
            {
                Id = reader.GetUInt32("id"),
                Username = reader.GetString("username"),
                Password = reader.GetString("password"),
                Email = reader.GetString("email"),
                IsBanned = reader.GetBoolean("banned"),
                Nickname = reader.GetString("nickname", null)
            };

            reader.Close();
            return account;
        }
    }
}