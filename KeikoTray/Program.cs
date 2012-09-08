using System;
using System.Drawing;
using System.Windows.Forms;

namespace KeikoTray
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var contextMenu1 = new ContextMenu();
            var menuItem1 = new MenuItem();
            var notifyIcon1 = new NotifyIcon();

            var icon000 = new Icon("000.ico");
            var icon001 = new Icon("001.ico");
            var icon010 = new Icon("010.ico");
            var icon011 = new Icon("011.ico");
            var icon100 = new Icon("100.ico");
            var icon101 = new Icon("101.ico");
            var icon110 = new Icon("110.ico");
            var icon111 = new Icon("111.ico");

            contextMenu1.MenuItems.AddRange(new[] { menuItem1 });

            menuItem1.Index = 0;
            menuItem1.Text = "E&xit";
            menuItem1.Click += (object s, EventArgs e) =>
            {
                notifyIcon1.Visible = false;
                Application.Exit();
            };

            notifyIcon1.ContextMenu = contextMenu1;

            notifyIcon1.Text = "Form1 (NotifyIcon example)";
            notifyIcon1.Visible = true;

            Application.Run();
        }
    }
}
