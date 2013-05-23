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
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace WpfApplication1
{

    public partial class ColorWindow : Window
    {
        KinectSensor kinect;
        public ColorWindow(KinectSensor sensor) : this()
        {
            kinect = sensor;
        }

        public ColorWindow()
        {
            InitializeComponent();
            Loaded += ColorWindow_Loaded;
            Unloaded += ColorWindow_Unloaded;
        }
        void ColorWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (kinect != null)
            {
                kinect.ColorStream.Disable();
                kinect.ColorFrameReady -= myKinect_ColorFrameReady;
                kinect.Stop();              
            }
        }
        void ColorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (kinect != null)
            {                
                //kinect.ColorStream.Enable();
                kinect.ColorStream.Enable(ColorImageFormat.InfraredResolution640x480Fps30);
                kinect.ColorFrameReady += myKinect_ColorFrameReady;
                kinect.Start();
            }
        }
        void myKinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame frame = e.OpenColorImageFrame())
            {
                if (frame == null)
                    return;

                byte[] pixelData = new byte[frame.PixelDataLength];
                frame.CopyPixelDataTo(pixelData);
                //ColorData.Source = BitmapImage.Create(frame.Width, frame.Height, 96, 96,
                //PixelFormats.Bgr32, null, pixelData,
                //frame.Width * frame.BytesPerPixel);
                ColorData.Source = BitmapImage.Create(frame.Width, frame.Height, 96, 96,
                PixelFormats.Gray16, null, pixelData,
                frame.Width * frame.BytesPerPixel);
            }
        }
    }
}
