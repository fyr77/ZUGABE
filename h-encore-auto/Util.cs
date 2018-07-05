using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Windows.Markup;
using System.Windows;

namespace h_encore_auto
{
    public class Util
    {
        private static WebClient web = new WebClient();
        private static HttpClient http = new HttpClient();

        public static string GetEncKey(string aid)
        //Thanks to noahc3 (https://github.com/noahc3/) for this. 
        {
            try
            {
                string page = http.GetStringAsync(Ref.urlCma + aid).Result;
                return page.Substring(page.Length - 65, 64);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to get the CMA encryption key. Make sure your internet is connected and/or retry.");
                return "";
            }
        }

        public static void DeleteDirectory(string path)
        {
            foreach (string directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }
            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                Directory.Delete(path, true);
            }
        }

        public static void dlFile(string url, string filename)
        {
            using (WebClient client = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DownloadFile(url, Ref.tempDir + filename);
            }
        }
        public static void cleanup()
        {
            Util.DeleteDirectory(Ref.tempDir);
            System.Environment.Exit(0);
        }
    }
}