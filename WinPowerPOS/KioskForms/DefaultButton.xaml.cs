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
using System.Drawing;
using System.IO;

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for DefaultButton.xaml
    /// </summary>
    public partial class DefaultButton : UserControl
    {
        public event EventHandler ControlClick;

        private string _buttonColor = "#76B44E";
        private string _buttonHoverColor = "#88ce5b";
        private string _buttonPressColor = "#4e812d";
        
        public string ButtonColor
        {
            get { return _buttonColor; }
            set 
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.Resources["bgColorBrush"] = (SolidColorBrush)(new BrushConverter().ConvertFrom(value));
                    _buttonColor = value;
                }
            }
        }
        public string ButtonHoverColor
        {
            get { return _buttonHoverColor; }
            set 
            {
                if(!string.IsNullOrEmpty(value))
                    _buttonHoverColor = value; 
            }
        }
        public string ButtonPressColor
        {
            get { return _buttonPressColor; }
            set 
            {
                if (!string.IsNullOrEmpty(value))
                    _buttonPressColor = value; 
            }
        }
        public string ButtonText
        {
            set
            {
                this.txtButton.Text = value;
            }
        }
        public string ButtonImage
        {
            set
            {
                //imgButton.Source = ToWpfBitmap(value);
                //imgButton.Source = new BitmapImage(new Uri("Resources/staffIcon.png", UriKind.Relative));
                var uriSource = new Uri(@"/WinPowerPOS;component/"+value, UriKind.Relative);
                imgButton.Source = new BitmapImage(uriSource);
            }
        }

        public DefaultButton()
        {
            InitializeComponent();
        }

        private void UserControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Resources["bgColorBrush"] = (SolidColorBrush)(new BrushConverter().ConvertFrom(_buttonPressColor));
            if (ControlClick != null)
                ControlClick(sender, new EventArgs());
        }

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Resources["bgColorBrush"] = (SolidColorBrush)(new BrushConverter().ConvertFrom(_buttonHoverColor));
            this.Cursor = Cursors.Hand;
        }

        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Resources["bgColorBrush"] = (SolidColorBrush)(new BrushConverter().ConvertFrom(_buttonColor));
            this.Cursor = Cursors.Arrow;
        }

        private void UserControl_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Resources["bgColorBrush"] = (SolidColorBrush)(new BrushConverter().ConvertFrom(_buttonHoverColor));
        }

        private static BitmapSource ToWpfBitmap(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
    }
}
