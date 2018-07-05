using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private void Window_Closed(object sender, EventArgs e)
        {
            var newForm = new MainWindow();
            newForm.Visibility = Visibility.Visible;
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            // 7ZIP Download and extraction
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

            //Rest of the tool downloads
            Util.dlFile(Ref.urlPsvimg, "psvimgtools.zip");
            Util.dlFile(Ref.urlPkg, "pkg2zip.zip");
            Util.dlFile(Ref.urlEnc, "h-encore.zip");
            Util.dlFile(Ref.urlEntry, "entryPoint.pkg");
        }
    }
}
