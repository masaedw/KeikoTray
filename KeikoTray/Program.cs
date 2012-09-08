using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
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

            var icon1 = new Icon("tray.ico");
            var icon2 = new Icon("Icon1.ico");

            contextMenu1.MenuItems.AddRange(new[] { menuItem1 });

            menuItem1.Index = 0;
            menuItem1.Text = "E&xit";
            menuItem1.Click += (object s, EventArgs e) =>
            {
                notifyIcon1.Visible = false;
                Application.Exit();
            };

            notifyIcon1.Icon = icon1;

            notifyIcon1.ContextMenu = contextMenu1;

            notifyIcon1.Text = "Form1 (NotifyIcon example)";
            notifyIcon1.Visible = true;

            var t = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    notifyIcon1.Icon = icon1;
                    Thread.Sleep(1000);
                    notifyIcon1.Icon = icon2;
                    Thread.Sleep(1000);
                    notifyIcon1.BalloonTipText = "hogehoge";
                }
            });

            Application.Run();
        }
    }
}
