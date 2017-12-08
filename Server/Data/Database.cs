using MySql.Data.MySqlClient;
using System.Data;

namespace sTalk.Server.Data
{
    public class Database
    {
        private MySqlConnection _connection;

        public Database(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        public void Open()
        {
            if (_connection.State == ConnectionState.Open)
                return;

            try
            {
                _connection.Open();
            }
            catch
            {
                throw;
            }
        }

        public void Close()
        {
            if (_connection.State != ConnectionState.Open)
                return;

            try
            {
                _connection.Close();
            }
            catch
            {
                throw;
            }
        }

        public int ExecuteNonQuery(string query)
        {
            try
            {
                var cmd = new MySqlCommand(query, _connection);
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }

        public MySqlDataReader ExecuteReader(string query)
        {
            try
            {
                var cmd = new MySqlCommand(query, _connection);
                return cmd.ExecuteReader();
            }
            catch
            {
                throw;
            }
        }

        public object ExecuteScalar(string query)
        {
            try
            {
                var cmd = new MySqlCommand(query, _connection);
                return cmd.ExecuteScalar();
            }
            catch
            {
                throw;
            }
        }
    }

    public static class MySqlClient
    {
        public static string GetString(this MySqlDataReader reader, string column, string defaultValue)
        {
            var index = reader.GetOrdinal(column);

            if (reader.IsDBNull(index))
                return defaultValue;

            return reader.GetString(index);
        }
    }
}