using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace h_encore_auto
{
    /// <summary>
    /// Interaction logic for VitaGuide.xaml
    /// </summary>
    public partial class VitaGuide : Window
    {
        int currImg;

        string[] currText = lang.GuideText(Util.GetLang());

        public VitaGuide()
        {
            InitializeComponent();

            if (Ref.isSecondGuide == true)
                currImg = 15;
            else
                currImg = 1;

            imgFrame.Source = new BitmapImage(new Uri("/img/" + currImg + ".png", UriKind.Relative));
            textField.Text = currText[currImg - 1];
            buttonDone.Visibility = Visibility.Hidden;
        }

        private void buttonFwd_Click(object sender, RoutedEventArgs e)
        {
            currImg++;
            imgFrame.Source = new BitmapImage(new Uri("/img/" + currImg + ".png", UriKind.Relative));
            textField.Text = currText[currImg - 1];
            buttonBck.IsEnabled = true;

            if (currImg == 14 || currImg == 26)
            {
                buttonFwd.IsEnabled = false;
                buttonDone.Visibility = Visibility.Visible;
            }
            else
            {
                buttonFwd.IsEnabled = true;
            }
        }

        private void buttonBck_Click(object sender, RoutedEventArgs e)
        {
            currImg--;
            imgFrame.Source = new BitmapImage(new Uri("/img/" + currImg + ".png", UriKind.Relative));
            buttonFwd.IsEnabled = true;
            textField.Text = currText[currImg];

            if (currImg == 1 || currImg == 15)
            {
                buttonBck.IsEnabled = false;
            }
            else
            {
                buttonBck.IsEnabled = true;
            }
        }
    }
}
