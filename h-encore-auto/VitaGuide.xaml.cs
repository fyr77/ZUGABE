using System;
using System.Collections.Generic;
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
    /// Interaction logic for VitaGuide.xaml
    /// </summary>
    public partial class VitaGuide : Window
    {
        int currImg = 1;

        public VitaGuide()
        {
            InitializeComponent();
            imgFrame.Source = new BitmapImage(new Uri("/img/1.png", UriKind.Relative));
            currImg = 1;
            buttonDone.Visibility = Visibility.Hidden;
        }

        private void buttonFwd_Click(object sender, RoutedEventArgs e)
        {
            currImg++;
            imgFrame.Source = new BitmapImage(new Uri("/img/" + currImg + ".png", UriKind.Relative));
            buttonBck.IsEnabled = true;

            if (currImg == 14)
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

            if (currImg == 1)
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
