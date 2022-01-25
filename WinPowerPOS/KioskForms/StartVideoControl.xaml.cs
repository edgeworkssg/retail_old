using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for StartVideoControl.xaml
    /// </summary>
    public partial class StartVideoControl : UserControl
    {
        private bool repeat = false;

        public StartVideoControl()
        {
            InitializeComponent();

            errorMessage.Background = new SolidColorBrush(Colors.Black) { Opacity = 0.6 };
            errorStatus.Background = new SolidColorBrush(Colors.Black) { Opacity = 0.6 };
        }

        public void Play(Uri path, bool repeat)
        {
            mediaElement.Source = path;
            mediaElement.Play();
            this.repeat = repeat;
        }

        public void Stop()
        {
            mediaElement.Stop();
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (repeat)
            {
                mediaElement.Position = TimeSpan.Zero;
                mediaElement.Play();
            }
        }

        public void SetDisabledMode(bool p)
        {
            errorMessage.Visibility = p ? Visibility.Visible : Visibility.Hidden;
        }

        public void SetStatusCode(string code)
        {
            label2.Text = code;

            if (label2.Text.Length < 1)
                errorStatus.Visibility = Visibility.Hidden;
            else
                errorStatus.Visibility = Visibility.Visible;
        }
    }
}
