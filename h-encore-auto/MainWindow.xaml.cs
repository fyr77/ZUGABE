using System.Windows;
using System.Net;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System;
using System.Windows.Threading;
using System.Diagnostics;
using Essy.Tools.InputBox;
using Microsoft.Win32;

namespace h_encore_auto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (Directory.Exists(temp))
            {
                DeleteDirectory(temp);
            }

            createTemp();
        }

        string ProgramFilesx86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        bool tempCreated = false;
        string temp = Path.GetTempPath() + @"encore_temp\";

        private void dlFile(string url, string filename)
        {
            using (WebClient client = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DownloadFile(url, temp + filename);
            }
        }
        private void createTemp()
        {
            if (tempCreated == false)
            {
                Directory.CreateDirectory(temp);
                tempCreated = true;
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

        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            if (boxPath7z.Text == "")
            {
                MessageBox.Show("A path missing!");
            }
            else if (boxPathEntry.Text == "")
            {
                MessageBox.Show("A path missing!");
            }
            else if (boxPathEnc.Text == "")
            {
                MessageBox.Show("A path missing!");
            }
            else if (boxPathPkg.Text == "")
            {
                MessageBox.Show("A path missing!");
            }
            else if (boxPathPsvimg.Text == "")
            {
                MessageBox.Show("A path missing!");
            }
            else
            {
                MessageBox.Show("This will probably take some time. Sit back and wait for further message boxes.");

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.WorkingDirectory = temp;

                startInfo.Arguments = "/C " + boxPath7z.Text + " x " + boxPathPsvimg.Text;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = "/C " + boxPath7z.Text + " x " + boxPathPkg.Text;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = "/C " + boxPath7z.Text + " x " + boxPathEnc.Text;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = "/C " + temp + "pkg2zip.exe" + " -x " + boxPathEntry.Text;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = "/C xcopy /E /Y /I " + temp + @"app\PCSG90096\ " + temp + @"h-encore\app\ux0_temp_game_PCSG90096_app_PCSG90096\";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = "/C xcopy /E /Y /I " + temp + @"app\PCSG90096\sce_sys\package\temp.bin " + temp + @"h-encore\license\ux0_temp_game_PCSG90096_license_app_PCSG90096\6488b73b912a753a492e2714e9b38bc7.rif*";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                for (; ; )
                {
                    Process[] pname = Process.GetProcessesByName("qcma");
                    if (pname.Length == 0)
                    {
                        if (MessageBox.Show("QCMA not detected. Please start it now. Download it?", "QCMA error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            Process.Start("https://codestation.github.io/qcma/");
                            MessageBox.Show("Retrying...");
                        }
                        else
                            MessageBox.Show("Retrying...");
                    }
                    else
                    {
                        break;
                    }
                }
                for (; ; )
                {
                    MessageBox.Show("In the QCMA settings, set the option \"Use this version for updates\" to \"FW 0.00 (Always up-to-date)\".");
                    MessageBox.Show("Launch Content Manager on your PS Vita and connect it to your computer, where you then need to select PC -> PS Vita System.\nAfter that you select Applications. If you see an error message about System Software, you should simply reboot your device to solve it\n(if this doesn't solve, then put your device into airplane mode and reboot).");
                    if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PS Vita\"))
                    {
                        MessageBox.Show("A folder will open, which should now contain another folder named with 16 characters of jumbled letters and numbers.\nCopy this folder name and insert it on the website, which is going to open as well.\nThe website will give you an even longer text mass, which you must enter in the next step.");
                        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PS Vita\APP\");
                        Process.Start("http://cma.henkaku.xyz/");
                    }
                    else
                    {
                        MessageBox.Show("A folder named after your Account ID was created inside your \"PS Vita\\APP\\\"\nUse the settings of QCMA to find it.\nCopy the folder name and insert it on the website, which is going to open.\nThe website will give you an even longer text mass, which you must enter in the next step.");
                        Process.Start("http://cma.henkaku.xyz/");
                    }
                    if (MessageBox.Show("Do you want to read the steps again?", "Re-read?", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        break;
                    }
                }
                string key = null;
                for (; ; )
                {
                    key = InputBox.ShowInputBox("Enter key with 64 characters from the website.");
                    if (key != null)
                    {
                        break;
                    }
                    else
                    {
                        if (MessageBox.Show("Do you want to cancel?", "Cancel?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            cleanup();
                        }
                    }
                }
                startInfo.WorkingDirectory = temp + "h-encore";

                startInfo.Arguments = @"/C ..\psvimg-create -n app -K " + key + " PCSG90096/app";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = @"/C ..\psvimg-create -n appmeta -K " + key + " PCSG90096/appmeta";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = @"/C ..\psvimg-create -n license -K " + key + " PCSG90096/license";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = @"/C ..\psvimg-create -n savedata -K " + key + " PCSG90096/savedata";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PS Vita\"))
                {
                    MessageBox.Show("Two folders will now open.\nCopy the contained folder called \"PCSG90096\" to the \"PS Vita/APP/xxxxxxxxxxxxxxxx/\" folder.\nThen refresh the databse of QCMA by right-clicking the icon and selecting it.");
                    Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PS Vita\APP\");
                    Process.Start(temp + @"h-encore\");
                }
                else
                {
                    MessageBox.Show("A folder will now open.\nCopy the contained folder called \"PCSG90096\" to your \"PS Vita/APP/xxxxxxxxxxxxxxxx/\" folder.\nThen refresh the databse of QCMA by right-clicking the icon and selecting it.");
                    Process.Start(temp + @"h-encore\");
                }
                MessageBox.Show("Ready? Have you copied the folder and refreshed the database?");
                MessageBox.Show("Now copy h-encore to your Vita using the content manager.");
                MessageBox.Show("Launch h-encore to exploit your device (if a message about trophies appears, simply click yes). \nThe screen should first flash white, then purple, and finally\nopen a menu called h-encore bootstrap menu where you can download VitaShell and install HENkaku.");
                if (MessageBox.Show("For more info and how to remove the trophy warning please visit TheFlow's official page for the exploit.", "Info", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Process.Start("https://codestation.github.io/qcma/");
                }

                cleanup();
            }
        }

        private void buttonZipDL_Click(object sender, RoutedEventArgs e)
        {
            dlFile("https://www.7-zip.org/a/7zr.exe", "7zr.exe");
            dlFile("https://www.7-zip.org/a/7z1805-extra.7z", "7z-extra.7z");

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = temp;

            startInfo.Arguments = "/C 7zr.exe x 7z-extra.7z";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            boxPath7z.Text = temp + "7za.exe";
        }

        private void buttonPsvimgDL_Click(object sender, RoutedEventArgs e)
        {
            dlFile("https://github.com/yifanlu/psvimgtools/releases/download/v0.1/psvimgtools-0.1-win32.zip", "psvimgtools-0.1-win32.zip");

            boxPathPsvimg.Text = temp + "psvimgtools-0.1-win32.zip";
        }

        private void buttonPkgDL_Click(object sender, RoutedEventArgs e)
        {
            dlFile("https://github.com/mmozeiko/pkg2zip/releases/download/v1.8/pkg2zip_32bit.zip", "pkg2zip_32bit.zip");

            boxPathPkg.Text = temp + "pkg2zip_32bit.zip";
        }

        private void buttonEncDL_Click(object sender, RoutedEventArgs e)
        {
            dlFile("https://github.com/TheOfficialFloW/h-encore/releases/download/v1.0/h-encore.zip", "h-encore.zip");

            boxPathEnc.Text = temp + "h-encore.zip";
        }

        private void buttonEntryDL_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This will take a while, please be patient.");

            dlFile("http://ares.dl.playstation.net/cdn/JP0741/PCSG90096_00/xGMrXOkORxWRyqzLMihZPqsXAbAXLzvAdJFqtPJLAZTgOcqJobxQAhLNbgiFydVlcmVOrpZKklOYxizQCRpiLfjeROuWivGXfwgkq.pkg", "xGMrXOkORxWRyqzLMihZPqsXAbAXLzvAdJFqtPJLAZTgOcqJobxQAhLNbgiFydVlcmVOrpZKklOYxizQCRpiLfjeROuWivGXfwgkq.pkg");

            boxPathEntry.Text = temp + "xGMrXOkORxWRyqzLMihZPqsXAbAXLzvAdJFqtPJLAZTgOcqJobxQAhLNbgiFydVlcmVOrpZKklOYxizQCRpiLfjeROuWivGXfwgkq.pkg";
        }

        private void button7zFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".exe";
            dlg.Filter = "Executables (.exe)|*.exe";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string path = dlg.FileName;
                boxPath7z.Text = path;
            }
        }

        private void buttonPsvimgFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".zip";
            dlg.Filter = "ZIP Archives (.zip)|*.zip";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string path = dlg.FileName;
                boxPathPsvimg.Text = path;
            }
        }

        private void buttonPkgFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".zip";
            dlg.Filter = "ZIP Archives (.zip)|*.zip";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string path = dlg.FileName;
                boxPathPkg.Text = path;
            }
        }

        private void buttonEncFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".zip";
            dlg.Filter = "ZIP Archives (.zip)|*.zip";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string path = dlg.FileName;
                boxPathEnc.Text = path;
            }
        }
        private void buttonEntryFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".zip";
            dlg.Filter = "PSVita PKG Archives (.pkg)|*.pkg";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string path = dlg.FileName;
                boxPathEntry.Text = path;
            }
        }
        private void cleanup()
        {
            DeleteDirectory(temp);
            MessageBox.Show("Done.");
            System.Environment.Exit(0);
        }
    }
}
