using System.Net;

namespace KeikoTray
{
    /// <summary>
    /// VirtualKeikoクライアント
    /// </summary>
    public class Keiko
    {
        /// <summary>
        /// VirtualKeikoのURL
        /// </summary>
        public string Url { get; private set; }
        /// <summary>
        /// Keikoの名前
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">VirtualKeikoのUrl</param>
        /// <param name="name">Keikoの名前</param>
        public Keiko(string url, string name)
        {
            Url = Url;
            Name = name;
        }

        /// <summary>
        /// 現在の状況を取得し、3桁の数字の文字列で返します。
        /// </summary>
        /// <returns></returns>
        public string GetState()
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(Url + Name);
            }
        }
    }
}
