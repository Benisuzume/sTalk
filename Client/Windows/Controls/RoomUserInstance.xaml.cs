using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sTalk.Client.Windows.Controls
{
    public partial class RoomUserInstance : UserControl
    {
        private static List<Color> _colors;
        private static int _index;

        static RoomUserInstance()
        {
            _colors = new List<Color>();
            _colors.Add(Color.FromRgb(183, 28, 28));
            _colors.Add(Color.FromRgb(74, 20, 140));
            _colors.Add(Color.FromRgb(13, 71, 161));
            _colors.Add(Color.FromRgb(0, 96, 100));
            _colors.Add(Color.FromRgb(51, 105, 30));
            _colors.Add(Color.FromRgb(130, 119, 23));
            _colors.Add(Color.FromRgb(245, 127, 23));
            _colors.Add(Color.FromRgb(191, 54, 12));
            _colors.Add(Color.FromRgb(136, 14, 79));
            _colors.Add(Color.FromRgb(1, 87, 155));
            _colors.Add(Color.FromRgb(0, 77, 64));
            _colors.Add(Color.FromRgb(255, 111, 0));
            _colors.Add(Color.FromRgb(49, 27, 146));
            _colors.Add(Color.FromRgb(27, 94, 32));
            _colors.Add(Color.FromRgb(230, 81, 0));
            _colors.Add(Color.FromRgb(26, 35, 126));

            var random = new Random();
            _index = random.Next(_colors.Count);
        }

        public RoomUserInstance(string username, string status)
        {
            InitializeComponent();
            DataContext = this;

            if (username == null)
            {
                var color = Color.FromRgb(38, 50, 56);
                var brush = new SolidColorBrush(color);

                Brush = brush;
                Initial = null;
                Username = "Me";
                Status = "Joined at " + DateTime.Now.ToString("HH:mm:ss");

                lblInitial.Visibility = Visibility.Collapsed;
            }
            else
            {
                Brush = GetBrush();
                Initial = username.Substring(0, 1).ToUpper();
                Username = username;
                Status = status;
            }
        }

        public Brush Brush { get; private set; }
        public string Initial { get; private set; }
        public string Username { get; private set; }
        public string Status { get; private set; }

        public PublicMessageInstance PublicMessage(string message)
        {
            return new PublicMessageInstance(Brush, Initial, Username, message);
        }

        private Brush GetBrush()
        {
            var brush = new SolidColorBrush(_colors[_index]);

            _index++;
            if (_index == _colors.Count)
                _index = 0;

            return brush;
        }
    }
}