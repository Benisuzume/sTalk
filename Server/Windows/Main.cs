using Server.Properties;
using sTalk.Server.Data;
using System;
using System.Windows.Forms;

namespace sTalk.Server.Windows
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            var database = new Database(Settings.Default.ConnectionString);

            try
            {
                database.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "sTalk Messenger", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            Accounts.Initialize(database);
            Contacts.Initialize(database);
            Rooms.Initialize(database);

            try
            {
                var server = new Communication.Server(Settings.Default.PrivateKey, Settings.Default.ServerPort);
            }
            catch (Exception ex)
            {
                database.Close();

                MessageBox.Show(ex.Message, "sTalk Messenger", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}