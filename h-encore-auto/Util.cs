using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Windows.Markup;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

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
                MessageBox.Show("Failed to get the CMA encryption key. Make sure your internet is connected and retry.");
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
        public static void Cleanup()
        {
            Process[] pname = Process.GetProcessesByName("qcma");
            if (pname.Length != 0)
            {
                foreach (var proc in pname)
                {
                    proc.Kill();
                }
            }

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            ProcessStartInfo startInfoOut = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = Ref.tempDir;

            if (Ref.isRegModified == true)
            {
                startInfo.Arguments = @"/C reg delete HKEY_CURRENT_USER\Software\codestation\qcma /f";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }

            if (Ref.isQcmaConfigFound == true)
            {
                startInfo.Arguments = @"/C reg import " + Ref.pathBackupReg;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }

            if (Directory.Exists(Ref.tempDir))
            {
                if (MessageBox.Show("Do you want to keep the downloaded files for future use?\nPressing no will wipe any leftover files of this application off your computer.", "Keep Files?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    File.Create(Ref.tempDir + "keepfile");
                else
                    DeleteDirectory(Ref.tempDir);
            }

            Environment.Exit(0);
        }
        public static bool IsDirectoryEmpty(string path)
        {
            IEnumerable<string> items = Directory.EnumerateFileSystemEntries(path);
            using (IEnumerator<string> en = items.GetEnumerator())
            {
                return !en.MoveNext();
            }
        }

        public static string GetLang()
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;

            return ci.TwoLetterISOLanguageName;
        }
    }
}