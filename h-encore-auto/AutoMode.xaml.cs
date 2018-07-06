using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Threading;

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

            if (File.Exists(Ref.tempDir + "keepfile"))
            {
                Ref.areFilesKept = true;
                Util.keepFilesCleanup();
            }
            else
            {
                Ref.areFilesKept = false;
                if (Directory.Exists(Ref.tempDir))
                    Util.DeleteDirectory(Ref.tempDir);
            }

            InitTimer();
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(Ref.tempDir);
            buttonStart.IsEnabled = false;
            buttonClose.IsEnabled = false;
            barWorking.Visibility = Visibility.Visible;

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                Process[] pname = Process.GetProcessesByName("qcma");
                if (pname.Length != 0)
                {
                    foreach (var proc in pname)
                    {
                        proc.Kill();
                    }
                    MessageBox.Show("QCMA was closed, since this application has to interact with it.");
                }

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                ProcessStartInfo startInfoOut = new ProcessStartInfo();

                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.WorkingDirectory = Ref.tempDir;

                startInfoOut.WindowStyle = ProcessWindowStyle.Hidden;
                startInfoOut.FileName = "cmd.exe";
                startInfoOut.WorkingDirectory = Ref.tempDir;

                if (Ref.areFilesKept == false)
                {
                    // 7ZIP Download and extraction
                    Util.dlFile(Ref.url7zr, "7zr.exe");
                    Util.dlFile(Ref.url7za, "7z-extra.7z");

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

                    string text = File.ReadAllText(Ref.pathImportReg);
                    text = text.Replace("REPLACE", Ref.pathQcmaRes);
                    File.WriteAllText(Ref.pathImportReg, text);
                    text = text.Replace("\\", "/");
                    File.WriteAllText(Ref.pathImportReg, text);
                    text = text.Replace("HKEY_CURRENT_USER/Software/codestation/qcma", @"HKEY_CURRENT_USER\Software\codestation\qcma");
                    File.WriteAllText(Ref.pathImportReg, text);
                }

                startInfo.Arguments = "/C " + Ref.path7z + " x " + Ref.pathPsvimg;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = "/C " + Ref.path7z + " x " + Ref.pathPkg;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = "/C " + Ref.path7z + " x " + Ref.pathEnc;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                Util.dlFile(Ref.urlCreateBat, "h-encore\\create.bat");

                startInfo.Arguments = "/C " + Ref.path7z + " x " + Ref.pathQcma;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                startInfo.Arguments = "/C " + Ref.tempDir + "pkg2zip.exe -x " + Ref.pathEntry;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                Util.CopyDir(Ref.tempDir + "app\\PCSG90096\\", Ref.tempDir + "h-encore\\app\\ux0_temp_game_PCSG90096_app_PCSG90096\\",true);
                File.Copy(Ref.tempDir + "app\\PCSG90096\\sce_sys\\package\\temp.bin", Ref.tempDir + @"h-encore\license\ux0_temp_game_PCSG90096_license_app_PCSG90096\6488b73b912a753a492e2714e9b38bc7.rif");

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

                startInfoOut.RedirectStandardOutput = true;
                startInfoOut.UseShellExecute = false;
                startInfoOut.Arguments = @"/C reg query HKEY_CURRENT_USER\Software\codestation\qcma & echo 0";
                process.StartInfo = startInfoOut;
                process.Start();
                string stdout = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (stdout == "0")
                {
                    Ref.isQcmaConfigFound = false;

                    startInfo.Arguments = @"/C reg import " + Ref.pathImportReg;
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }

                else
                {
                    Ref.isQcmaConfigFound = true;

                    startInfo.Arguments = @"/C reg export HKEY_CURRENT_USER\Software\codestation\qcma " + Ref.pathBackupReg;
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();

                    startInfo.Arguments = @" /C reg import " + Ref.pathImportReg;
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }

                Ref.isRegModified = true;

                startInfo.Arguments = @"/C " + Ref.pathQcmaExtracted + "qcma.exe";
                process.StartInfo = startInfo;
                process.Start();

                stage = 1;

            }).Start();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Util.Cleanup();
        }

        private int stage = 0;
        private System.Windows.Forms.Timer timer1;
        public void InitTimer()
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var guide = new VitaGuide();
            if (stage == 1)
            {
                for (; ; )
                {
                    stage = 0;
                    Ref.isSecondGuide = false;
                    guide.ShowDialog();
                    Ref.isSecondGuide = true;

                    if (Util.IsDirectoryEmpty(Ref.pathQcmaRes + "PSVita\\APP\\"))
                    {
                        MessageBox.Show("Required folder not found. \nMake sure you did everything correctly and follow the steps again.");
                    }
                    else
                    {
                        stage = 2;
                        break;
                    }
                }
            }
            if (stage == 2)
            {
                stage = 0;
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;

                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    ProcessStartInfo startInfoOut = new ProcessStartInfo();

                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.WorkingDirectory = Ref.tempDir + "h-encore";

                    Ref.shortAID = new DirectoryInfo(Directory.GetDirectories(Ref.pathQcmaRes + "PSVita\\APP\\")[0]).Name;

                    Ref.longAID = Util.GetEncKey(Ref.shortAID);

                    startInfo.Arguments = "/C create.bat " + Ref.longAID;
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();

                    Util.CopyDir(Ref.tempDir + "h-encore\\PCSG90096\\", Ref.pathQcmaRes + "PSVita\\APP\\" + Ref.shortAID + "\\PCSG90096\\", true);

                    stage = 3;
                }).Start();
            }
            if (stage == 3)
            {
                stage = 0;
                guide.ShowDialog();

                MessageBox.Show("If not already done, wait until your Vita has copied over the exploit, then press OK.");

                Util.Cleanup();
            }
        }
    }
}
