using sTalk.Server.Windows;
using System;
using System.Windows.Forms;

namespace sTalk.Server
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new Main());
        }
    }
}