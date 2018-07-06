using System;
using System.IO;

namespace h_encore_auto
{
    public class Ref
    {
        public static readonly string urlCma = "http://cma.henkaku.xyz/?aid=";
        public static readonly string urlPsvimg = "https://github.com/yifanlu/psvimgtools/releases/download/v0.1/psvimgtools-0.1-win32.zip";
        public static readonly string urlPkg = "https://github.com/mmozeiko/pkg2zip/releases/download/v1.8/pkg2zip_32bit.zip";
        public static readonly string urlEnc = "https://github.com/TheOfficialFloW/h-encore/releases/download/v1.0/h-encore.zip";
        public static readonly string urlEntry = "http://ares.dl.playstation.net/cdn/JP0741/PCSG90096_00/xGMrXOkORxWRyqzLMihZPqsXAbAXLzvAdJFqtPJLAZTgOcqJobxQAhLNbgiFydVlcmVOrpZKklOYxizQCRpiLfjeROuWivGXfwgkq.pkg";
        public static readonly string url7zr = "https://www.7-zip.org/a/7zr.exe";
        public static readonly string url7za = "https://www.7-zip.org/a/7z1805-extra.7z";
        public static readonly string urlQcma = "https://raw.githubusercontent.com/fyr77/ZUGABE/master/download-resources/Qcma.zip";
        public static readonly string urlReg = "https://raw.githubusercontent.com/fyr77/ZUGABE/master/download-resources/qcma.reg";
        public static readonly string urlCreateBat = "https://raw.githubusercontent.com/fyr77/ZUGABE/master/download-resources/create.bat";

        public static readonly string pathCurrPic = "img/1.png";

        public static readonly string tempDir = Path.GetTempPath() + @"encore_temp\";

        public static readonly string[] trims = new string[] {
            "movie\\",
            "image\\bg\\",
            "image\\ev\\",
            "image\\icon\\",
            "image\\stitle\\",
            "image\\tachie\\",
            "sound\\bgm\\",
            "sound\\se\\",
            "sound\\sec\\",
            "sound\\voice\\",
            "text\\01\\"
        };

        public static readonly string[] downloads = new string[] {
            "7zr.exe",
            "7z-extra.7z",
            "entryPoint.pkg",
            "h-encore.zip",
            "pkg2zip.zip",
            "psvimgtools.zip",
            "qcma.reg",
            "qcma.zip"
        };

        public static bool isSecondGuide = false;

        public static bool isQcmaConfigFound = false;

        public static readonly string ProgramFilesx86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        public static readonly string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        public static string longAID = null;
        public static string shortAID = null;
        public static readonly string path7z = tempDir + "7za.exe";
        public static readonly string pathPsvimg = tempDir + "psvimgtools.zip";
        public static readonly string pathPkg = tempDir + "pkg2zip.zip";
        public static readonly string pathEnc = tempDir + "h-encore.zip";
        public static readonly string pathEntry = tempDir + "entryPoint.pkg";
        public static readonly string pathQcma = tempDir + "qcma.zip";
        public static readonly string pathQcmaExtracted = tempDir + "Qcma\\";
        public static readonly string pathBackupReg = tempDir + "backup.reg";
        public static readonly string pathImportReg = tempDir + "qcma.reg";
        public static readonly string pathQcmaRes = tempDir + "QcmaRes\\";
        public static bool areFilesKept = false;

        public static bool isRegModified = false;
    }
}
