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
using System.IO;
using System.Windows.Forms.Integration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Tesseract;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CameraHelper.IsDisplay = true;
            CameraHelper.SourcePlayer = player;
            CameraHelper.UpdateCameraDevices();
        }
        private void btnOpenCamera_Click(object sender, EventArgs e)
        {
            if (CameraHelper.CameraDevices.Count > 0)
            {
                CameraHelper.SetCameraDevice(0);
            }
        }

        private void btnCloseCamera_Click(object sender, EventArgs e)
        {
            CameraHelper.CloseDevice();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            string fullPath = CameraHelper.CaptureImage(AppDomain.CurrentDomain.BaseDirectory + @"\Capture");

            BitmapImage bit = new BitmapImage();
            bit.BeginInit();
            bit.UriSource = new Uri(fullPath);
            bit.EndInit();
            imgCapture.Source = bit;

            Console.WriteLine(fullPath);

            try
            {
                Bitmap img = new Bitmap(fullPath);
                var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);

                var page = engine.Process(img);

                N.Text = page.GetText().Replace("N29 ", "");
                Console.Write(page.GetText());
                Console.WriteLine(page.GetMeanConfidence());
                Console.ReadKey();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message.ToString());
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CameraHelper.CloseDevice();
        }
        private void btnOcr_Click(object sender, EventArgs e)
        {
            N.Text = "XB";
            try
            {

                Bitmap img = new Bitmap(@"./Capture/sample1.jpg");    //需要识别的图片

                var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
                var page = engine.Process(img);

                N.Text = page.GetText().Replace("N29 ", "");
                Console.Write(page.GetText());
                Console.WriteLine(page.GetMeanConfidence());
                Console.ReadKey();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message.ToString());
            }
        }
    }
}
