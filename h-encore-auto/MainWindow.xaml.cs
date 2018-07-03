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

            if (Directory.Exists(Ref.tempDir))
            {
                Util.DeleteDirectory(Ref.tempDir);
            }

            createTemp();
        }

        public void createTemp()
        {
            if (tempCreated == false)
            {
                Directory.CreateDirectory(Ref.tempDir);
                tempCreated = true;
            }
        }

        string ProgramFilesx86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        bool tempCreated = false;
        string longAID = null;
        string shortAID = null;

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
                startInfo.WorkingDirectory = Ref.tempDir;

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

                startInfo.Arguments = "/C " + Ref.tempDir + "pkg2zip.exe" + " -x " + boxPathEntry.Text;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = "/C xcopy /E /Y /I " + Ref.tempDir + @"app\PCSG90096\ " + Ref.tempDir + @"h-encore\app\ux0_temp_game_PCSG90096_app_PCSG90096\";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = "/C xcopy /E /Y /I " + Ref.tempDir + @"app\PCSG90096\sce_sys\package\temp.bin " + Ref.tempDir + @"h-encore\license\ux0_temp_game_PCSG90096_license_app_PCSG90096\6488b73b912a753a492e2714e9b38bc7.rif*";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                try
                {
                    string path = Ref.tempDir + "app\\PCSG90096\\resource\\";
                    foreach (string k in Ref.trims)
                    {
                        Util.DeleteDirectory(path + k);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected Exception: " + ex.Message);
                    return;
                }

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
                        MessageBox.Show("A folder will open, which should now contain another folder named with 16 characters of jumbled letters and numbers.\nCopy this folder name and insert it in the next step.");
                        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PS Vita\APP\");
                        shortAID = InputBox.ShowInputBox("Enter the 16-character folder name.");
                    }
                    else
                    {
                        MessageBox.Show("A folder named after your Account ID was created inside your \"PS Vita\\APP\\\"\nUse the settings of QCMA to find it.\nCopy the folder name and insert it in the next step.");
                        shortAID = InputBox.ShowInputBox("Enter the 16-character folder name.");
                    }
                    if (MessageBox.Show("Press yes to continue, no to redo the last step.", "Continue?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        break;
                    }
                }

                longAID = Util.GetEncKey(shortAID);

                startInfo.WorkingDirectory = Ref.tempDir + "h-encore";

                startInfo.Arguments = @"/C ..\psvimg-create -n app -K " + longAID + " PCSG90096/app";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = @"/C ..\psvimg-create -n appmeta -K " + longAID + " PCSG90096/appmeta";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = @"/C ..\psvimg-create -n license -K " + longAID + " PCSG90096/license";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = @"/C ..\psvimg-create -n savedata -K " + longAID + " PCSG90096/savedata";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PS Vita\"))
                {
                    MessageBox.Show("Two folders will now open.\nCopy the contained folder called \"PCSG90096\" to the \"PS Vita/APP/xxxxxxxxxxxxxxxx/\" folder.\nThen refresh the databse of QCMA by right-clicking the icon and selecting it.");
                    Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PS Vita\APP\");
                    Process.Start(Ref.tempDir + @"h-encore\");
                }
                else
                {
                    MessageBox.Show("A folder will now open.\nCopy the contained folder called \"PCSG90096\" to your \"PS Vita/APP/xxxxxxxxxxxxxxxx/\" folder.\nThen refresh the databse of QCMA by right-clicking the icon and selecting it.");
                    Process.Start(Ref.tempDir + @"h-encore\");
                }
                MessageBox.Show("Ready? Have you copied the folder and refreshed the database?");
                MessageBox.Show("Now copy h-encore to your Vita using the content manager.");
                MessageBox.Show("Launch h-encore to exploit your device (if a message about trophies appears, simply click yes). \nThe screen should first flash white, then purple, and finally\nopen a menu called h-encore bootstrap menu where you can download VitaShell and install HENkaku.");
                if (MessageBox.Show("For more info and how to remove the trophy warning please visit TheFlow's official page for the exploit.", "Info", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Process.Start("https://codestation.github.io/qcma/");
                }

                Util.cleanup();
            }
        }

        private void buttonZipDL_Click(object sender, RoutedEventArgs e)
        {
            Util.dlFile(Ref.url7zr, "7zr.exe");
            Util.dlFile(Ref.url7za, "7z-extra.7z");

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = Ref.tempDir;

            startInfo.Arguments = "/C 7zr.exe x 7z-extra.7z";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            boxPath7z.Text = Ref.tempDir + "7za.exe";
        }

        private void buttonPsvimgDL_Click(object sender, RoutedEventArgs e)
        {
            Util.dlFile(Ref.urlPsvimg, "psvimgtools.zip");

            boxPathPsvimg.Text = Ref.tempDir + "psvimgtools.zip";
        }

        private void buttonPkgDL_Click(object sender, RoutedEventArgs e)
        {
            Util.dlFile(Ref.urlPkg, "pkg2zip.zip");

            boxPathPkg.Text = Ref.tempDir + "pkg2zip.zip";
        }

        private void buttonEncDL_Click(object sender, RoutedEventArgs e)
        {
            Util.dlFile(Ref.urlEnc, "h-encore.zip");

            boxPathEnc.Text = Ref.tempDir + "h-encore.zip";
        }

        private void buttonEntryDL_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This will take a while, please be patient.");

            Util.dlFile(Ref.urlEntry, "entryPoint.pkg");

            boxPathEntry.Text = Ref.tempDir + "entryPoint.pkg";
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
    }
}
