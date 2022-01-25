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

using PowerPOS;

namespace WinPowerPOS.KioskForms
{
    /// <summary>
    /// Interaction logic for VideoControl.xaml
    /// </summary>
    public partial class VideoControl : UserControl
    {
        private bool repeat = false;

        public String SubTitle { get { return subtitle.Text; } set { subtitle.Text = value; } }

        public double FontSize { get { return subtitle.FontSize; } set { subtitle.FontSize = value; } }

        //DisctMediaElementCol mediaList;

        Dictionary<string, string> mediaList = new Dictionary<string, string>();

        public VideoControl()
        {
            InitializeComponent();
        }

        public void AddMediaToGrid(string videoType, Uri path)
        {
            //
            hideAllMediaElement();
            string res = "";
            if (mediaList.TryGetValue(videoType, out res))
            {
                
                //Show the media List
                showMediaElement(res);
                return;
            }
            
            //Add the Media Element to the Container
            MediaElement mediaToAdd = new MediaElement();
            mediaToAdd.Source = path;
            mediaToAdd.Uid = Guid.NewGuid().ToString() ;
            mediaToAdd.LoadedBehavior = MediaState.Manual;
            mediaToAdd.Stretch = Stretch.Fill;
            mediaToAdd.MediaEnded += mediaElement_MediaEnded;
            grid.Children.Add(mediaToAdd);
            mediaToAdd.Play();
            mediaList.Add(videoType, mediaToAdd.Uid);
            showMediaElement(mediaToAdd.Uid);
            
        }

        private void hideAllMediaElement()
        {
            foreach(UIElement element in grid.Children)
            {
                if (element.GetType() == typeof(MediaElement))
                {
                    element.Visibility = Visibility.Hidden;
                }
            }
        }

        private void showMediaElement(string key)
        {
            foreach (UIElement element in grid.Children)
            {
                if (element.Uid == key)
                {
                    element.Visibility = Visibility.Visible;
                    //block.
                }
            }
        }

        public void Play(Uri path, bool repeat)
        {
            mediaElement.Source = path;
            mediaElement.Play();
            this.repeat = repeat;
        }

        public void Play(Uri path, bool repeat, string subtitle)
        {
            Play(path, repeat, subtitle, 36, null);
        }

        public void Play(Uri path, bool repeat, string content, double fontSize)
        {
            Play(path, repeat, content, fontSize, null);
        }

        //private 

        public void Play(Uri path, bool repeat, string content, double fontSize, Brush brush)
        {
            this.SubTitle = content;
            this.FontSize = fontSize;

            if (SubTitle.Trim() == "")
            {
                block.Background = null;
            }
            else
            {
                block.Background = Brushes.Black;
            }

            if (brush == null)
                this.subtitle.Foreground = Brushes.LightGray;
            else
                this.subtitle.Foreground = brush;

            Play(path, repeat);
            //Change to show / hide based on array
            //AddMediaToGrid(path,)
        }

        public void Play(Uri path, bool repeat, string content, double fontSize, Brush brush, string videoType)
        {
            this.SubTitle = content;
            this.FontSize = fontSize;

            if (SubTitle.Trim() == "")
            {
                block.Background = null;
            }
            else
            {
                block.Background = Brushes.Black;
            }

            if (brush == null)
                this.subtitle.Foreground = Brushes.LightGray;
            else
                this.subtitle.Foreground = brush;

            //Play(path, repeat);
            //Change to show / hide based on array
            AddMediaToGrid(videoType, path);
        }

        public void Stop()
        {
            this.SubTitle = "";
            this.FontSize = 36;

            mediaElement.Stop();
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            //if (repeat)
            //{
                ((MediaElement)sender).Position = TimeSpan.Zero;
                ((MediaElement)sender).Play();
            //}
        }
    }
}
