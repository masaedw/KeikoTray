using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace KeikoTray
{
    public class KeikoIcon
    {
        Keiko keiko_;
        ContextMenu menu_;
        IDisposable handle_;
        NotifyIcon icon_;

        public KeikoIcon(Keiko keiko, ContextMenu menu)
        {
            keiko_ = keiko;
            menu_ = menu;
            icon_ = new NotifyIcon();
            icon_.ContextMenu = menu_;
            icon_.Text = String.Format("[{0}]", keiko_.Name);
            icon_.Visible = true;
        }

        public void Start()
        {
            var anim = new KeikoAnimation(icon => icon_.Icon = icon);

            handle_ = Observable.Interval(TimeSpan.FromSeconds(3.0))
                .Select(_ => keiko_.GetState())
                .DistinctUntilChanged()
                .Subscribe(state =>
                {
                    if (state.StartsWith("220"))
                    {
                        icon_.ShowBalloonTip(1000, keiko_.Name, "Error", ToolTipIcon.Error);
                    }
                    anim.SetState(state);
                }, (Exception e) =>
                {
                    if (e is WebException)
                    {
                        // ignore web excetption (ex: 503 error)
                        Trace.WriteLine(e);
                    }
                    else
                    {
                        End();
                        throw new Exception(String.Format("unknown exception in {0}", keiko_.Name), e);
                    }
                });
        }

        public void End()
        {
            icon_.Visible = false;
            if (handle_ != null)
            {
                handle_.Dispose();
            }
        }
    }

    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: KeikoTray.exe keikoName [keikoName...]");
                Environment.Exit(1);
            }

            var contextMenu1 = new ContextMenu();
            var menuItem1 = new MenuItem();
            contextMenu1.MenuItems.AddRange(new[] { menuItem1 });

            menuItem1.Index = 0;
            menuItem1.Text = "E&xit";

            var url = "http://virtualkeiko.herokuapp.com/";

            var keikos = args
                .Select(name => new Keiko(url, name))
                .Where(keiko => keiko.Exists())
                .Select(keiko => new KeikoIcon(keiko, contextMenu1))
                .ToArray();

            if (!keikos.Any())
            {
                Trace.WriteLine("No keikos. bye.");
                Environment.Exit(1);
            }

            menuItem1.Click += (object s, EventArgs e) =>
            {
                foreach (var tray in keikos)
                {
                    tray.End();
                }
                Application.Exit();
            };

            foreach (var tray in keikos)
            {
                tray.Start();
            }

            Application.Run();
        }
    }
}
