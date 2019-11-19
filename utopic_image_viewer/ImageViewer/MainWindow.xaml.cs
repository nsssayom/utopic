using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using ImageViewer;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(params string[] args)
        {
          InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            if (args.Length > 0)
            {
                string pathToPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures).ToString();
                string filename = args[0];
                imageContainer.Source = new BitmapImage(new Uri(filename)); 
            }
            else
            {
                imageContainer.Source = new BitmapImage(new Uri("image.jpg", UriKind.Relative));
            }
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private double zoomValue = 1.0;
        private void imageContainer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                zoomValue += 0.1;
            }
            else
            {
                if (zoomValue > .2)
                {
                    zoomValue -= 0.1;
                }
            }
            
            int intZoomVal = (int)(zoomValue * 100);
            string zoomText = intZoomVal.ToString();
            zoomText += "%";
            ZoomLabel.Content = zoomText;

            #region Animate ZoomLabel
            ZoomLabel.Visibility = Visibility.Visible;

            var a = new DoubleAnimation
            {
                From = 1,
                To = 0.0,
                FillBehavior = FillBehavior.Stop,
                BeginTime = TimeSpan.FromSeconds(.5),
                Duration = new Duration(TimeSpan.FromSeconds(1.5))
            };
            var storyboard = new Storyboard();

            storyboard.Children.Add(a);
            Storyboard.SetTarget(a, ZoomLabel);
            Storyboard.SetTargetProperty(a, new PropertyPath(OpacityProperty));
            storyboard.Completed += delegate { ZoomLabel.Visibility = System.Windows.Visibility.Hidden; };
            storyboard.Begin();
            #endregion

            GC.Collect();
        }
    }
}
