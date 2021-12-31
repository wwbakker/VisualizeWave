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
using System.IO;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WAV_Tools_C_Sharp;
using System.Media;
using Path = System.IO.Path;

namespace VisualizeWave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly String waveFilePath;
        private readonly WAV_file wavFile;
        private UInt16[] samples;
        public MainWindow()
        {
            waveFilePath = @"E:\Dev\Repos\sampleaudio\output.wav";
            wavFile = new WAV_file(Path.GetDirectoryName(waveFilePath), Path.GetFileName(waveFilePath));
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            wavFile.loadFile();
            wavFile.getBuffer_16_bits_mono(out samples);
            LoadBitmap();
        }

        WriteableBitmap writeableBitmap;
        protected void LoadBitmap()
        {

            RenderOptions.SetBitmapScalingMode(WaveImage, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(WaveImage, EdgeMode.Aliased);

            writeableBitmap = new WriteableBitmap(
                (int)ActualWidth,
                (int)ActualHeight,
                96,
                96,
                PixelFormats.Bgr32,
                null);

            WaveImage.Source = writeableBitmap;

            /*WaveImage.Stretch = Stretch.Fill;*/
            WaveImage.HorizontalAlignment = HorizontalAlignment.Left;
            WaveImage.VerticalAlignment = VerticalAlignment.Top;

            try
            {
                writeableBitmap.Lock();
                if (samples != null && WaveImage.ActualWidth > 0)
                {
                    var scaleWidth = samples.Length / WaveImage.ActualWidth;
                    var sampleResolution = wavFile.BitsPerSample == WAV_file.BITS_PER_SAMPLE.BPS_8_BITS ? Byte.MaxValue : UInt16.MaxValue;
                    var scaleHeight = WaveImage.ActualHeight / sampleResolution;

                    unsafe
                    {
                        // Get a pointer to the back buffer.
                        IntPtr pBackBuffer = writeableBitmap.BackBuffer;
                        var pPixel = (int x, int y) => pBackBuffer + (y * writeableBitmap.BackBufferStride) + (x * 4);
                        int color_data = 0x00ff00;
                        
                        for (int i = 0; i < WaveImage.ActualWidth; i++)
                        {
                            var x = i;
                            var y = (int)(samples[(int)Math.Min(i * scaleWidth, samples.Length - 1)] * scaleHeight);
                            * ((int*)pPixel(x, y)) = color_data;
                        }

                    }

                    writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, (int)WaveImage.ActualWidth, (int)WaveImage.ActualHeight));
                }
            }
            finally
            {
                // Release the back buffer and make it available for display.
                writeableBitmap.Unlock();
            }

        }


        private void WaveImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LoadBitmap();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                var soundPlayer = new SoundPlayer(waveFilePath);
                soundPlayer.Play();
            }
        }
    }
}
