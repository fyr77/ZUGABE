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

        public static readonly string tempDir = Path.GetTempPath() + @"encore_temp\";
    }
}
