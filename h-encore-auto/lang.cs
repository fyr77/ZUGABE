using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace h_encore_auto
{
    class lang
    {
        public static string[] GuideText(string lang)
        {
            if (lang == "en")
                return guideEN;
            else
                return guideEN;
        }

        private static readonly string[] guideEN= new string[] {
            "1. On your Vita, open the Content Manager. Make sure your Vita and PC are in the same Network!",
            "2. Select \"Copy Content\"",
            "3. If it tries to connect, cancel it.",
            "4. Select \"PC\"",
            "5. Select \"Wi-Fi\"",
            "6. Select \"Register Device\"",
            "7. Your Computer should show up. Select it.",
            "8. Enter the code shown or your computer.",
            "8. Enter the code shown or your computer.",
            "8. Enter the code shown or your computer.",
            "9. It should tell you that the device was registered successfully.",
            "10. After clicking next, please wait.",
            "11. Select \"PC -> PS Vita System\"",
            "12. Select \"Applications\"",
            "1. On your Vita, open the Content Manager. Make sure your Vita and PC are in the same Network!",
            "2. Select \"Copy Content\"",
            "3. Wait for it to connect.",
            "4. Select \"PC -> PS Vita System\"",
            "5. Select \"Applications\"",
            "6",
            "7",
            "8",
            "9",
            "10",
            "Done. Keep in mind that you have to launch h-encore every time you reboot the Vita. \nWhen launched, just press Exit again. This reapplies the exploit."
        };
    }
}
