using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace h_encore_auto
{
    /// <summary>
    /// Interaction logic for AutoMode.xaml
    /// </summary>
    public partial class AutoMode : Window
    {
        public AutoMode()
        {
            InitializeComponent();
        }

        string ProgramFilesx86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        bool tempCreated = false;
        string longAID = null;
        string shortAID = null;
        string path7z = Ref.tempDir + "7za.exe";
        string pathPsvimg = Ref.tempDir + "psvimgtools.zip";
        string pathPkg = Ref.tempDir + "pkg2zip.zip";
        string pathEnc = Ref.tempDir + "h-encore.zip";
        string pathEntry = Ref.tempDir + "entryPoint.pkg";
        string pathQcma = Ref.tempDir + "qcma.zip";
        string pathQcmaExtracted = Ref.tempDir + "Qcma\\";
        string pathBackupReg = Ref.tempDir + "backup.reg";
        string pathImportReg = Ref.tempDir + "qcma.reg";
        string pathQcmaRes = Ref.tempDir + "QcmaRes\\";

        private void Window_Closed(object sender, EventArgs e)
        {
            var newForm = new MainWindow();
            newForm.Visibility = Visibility.Visible;
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("qcma");
            if (pname.Length != 0)
            {
                foreach (var proc in pname)
                {
                    proc.Kill();
                }
                MessageBox.Show("QCMA was closed, since this application has to interact with it.");
            }

            // 7ZIP Download and extraction
            Util.dlFile(Ref.url7zr, "7zr.exe");
            Util.dlFile(Ref.url7za, "7z-extra.7z");

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            ProcessStartInfo startInfoOut = new ProcessStartInfo();

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = Ref.tempDir;

            startInfo.Arguments = "/C 7zr.exe x 7z-extra.7z";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            //Rest of the tool downloads
            Util.dlFile(Ref.urlPsvimg, "psvimgtools.zip");
            Util.dlFile(Ref.urlPkg, "pkg2zip.zip");
            Util.dlFile(Ref.urlEnc, "h-encore.zip");
            Util.dlFile(Ref.urlEntry, "entryPoint.pkg");
            Util.dlFile(Ref.urlQcma, "qcma.zip");
            Util.dlFile(Ref.urlReg, "qcma.reg");

            string text = File.ReadAllText(pathImportReg);
            text = text.Replace("REPLACE", pathQcmaRes);
            File.WriteAllText(pathImportReg, text);

            startInfo.Arguments = "/C " + path7z + " x " + pathPsvimg;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = "/C " + path7z + " x " + pathPkg;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = "/C " + path7z + " x " + pathEnc;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = "/C " + path7z + " x " + pathQcma;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = "/C " + Ref.tempDir + "pkg2zip.exe -x " + pathEntry;
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
                MessageBox.Show("Exception: " + ex.Message + "\nYou should tell the developer in a github issue. Include a screenshot if possible!");
                return;
            }

            bool qcmaConfigFound = false;

            startInfoOut.RedirectStandardOutput = true;
            startInfoOut.UseShellExecute = false;
            startInfoOut.Arguments = @"/C reg query HKEY_CURRENT_USER\Software\codestation\qcma & echo 0";
            process.StartInfo = startInfoOut;
            process.Start();
            string stdout = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (stdout == "0")
            {
                qcmaConfigFound = false;

                startInfo.Arguments = @"/C reg import " + pathImportReg;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }

            else
            {
                qcmaConfigFound = true;

                startInfo.Arguments = @"/C reg export HKEY_CURRENT_USER\Software\codestation\qcma " + pathBackupReg;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = @" /C reg import " + pathImportReg;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }

            startInfo.Arguments = @"/C " + pathQcmaExtracted + "qcma.exe";
            process.StartInfo = startInfo;
            process.Start();

            for (; ; )
            {
                var guide = new VitaGuide();
                guide.ShowDialog();

                if (Util.IsDirectoryEmpty(pathQcmaRes + "PSVita\\APP\\"))
                {
                    MessageBox.Show("Required folder not found. \nMake sure you did everything correctly and follow the steps again.");
                }
                else
                {
                    break;
                }
            }

            shortAID = Directory.GetDirectories(pathQcmaRes + "PSVita\\APP\\")[0];


        }
    }
}
