using System;
using System.Reactive.Linq;
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

            contextMenu1.MenuItems.AddRange(new[] { menuItem1 });

            menuItem1.Index = 0;
            menuItem1.Text = "E&xit";
            menuItem1.Click += (object s, EventArgs e) =>
            {
                notifyIcon1.Visible = false;
                Application.Exit();
            };

            notifyIcon1.ContextMenu = contextMenu1;

            notifyIcon1.Text = "[test]";
            notifyIcon1.Visible = true;

            var keiko = new Keiko("http://virtualkeiko.herokuapp.com/", "test");
            var anim = new KeikoAnimation(icon => notifyIcon1.Icon = icon);

            Observable.Interval(TimeSpan.FromSeconds(3.0))
                .Select(_ => keiko.GetState())
                .DistinctUntilChanged()
                .Subscribe(anim.SetState);

            Application.Run();
        }
    }
}
