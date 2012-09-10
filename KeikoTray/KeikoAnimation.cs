using System;
using System.Drawing;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using KeikoTray.Properties;

namespace KeikoTray
{
    public class KeikoAnimation
    {
        Action<Icon> Action;
        Icon[] Icons = new[]
            {
                Resources._000,
                Resources._001,
                Resources._010,
                Resources._011,
                Resources._100,
                Resources._101,
                Resources._110,
                Resources._111,
            };
        IDisposable Current;

        public KeikoAnimation(Action<Icon> action)
        {
            Action = action;
        }

        public void SetState(string state)
        {
            if (!Regex.IsMatch(state, "^[012]{3}"))
            {
                throw new ArgumentException("[012]のどれかからなる3桁の文字列が必要です", "state");
            }

            Console.WriteLine(state);

            if (Current != null)
            {
                Current.Dispose();
                Current = null;
            }

            if (!Animated(state))
            {
                Action(Icons[GetIndex(state)]);
            }
            else
            {
                var icon1 = Icons[GetIndex(state.Replace('2', '0'))];
                var icon2 = Icons[GetIndex(state.Replace('2', '1'))];
                Current = Observable.Interval(TimeSpan.FromSeconds(0.8))
                    .Select((_, i) => i % 2 == 0 ? icon1 : icon2)
                    .Subscribe(Action);
            }
        }

        static bool Animated(string state)
        {
            return state.IndexOf('2') != -1;
        }

        static int GetIndex(string state)
        {
            return Convert.ToInt32(state.Substring(0, 3), 2);
        }
    }
}
