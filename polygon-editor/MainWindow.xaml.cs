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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace polygon_editor
{

    public partial class MainWindow : Window
    {
        static UInt32 CANVAS_BACKGROUND_COLOR = 0xFFE0E0E0;
        static UInt32 CANVAS_LINE_COLOR = 0xFF0000FF;

        public MainWindow()
        {
            InitializeComponent();
            int width = (int)CanvasImage.Width;
            int height = (int)CanvasImage.Height;

            UInt32[] pixels = new UInt32[height * width];
            BitmapHelper.Fill(pixels, CANVAS_BACKGROUND_COLOR, width, height);
            BresenhamDrawer.Line(pixels, CANVAS_LINE_COLOR, width, 0, 0, 30, 200);

            BitmapSource bitmap = BitmapSource.Create(
                width,
                height,
                96,
                96,
                PixelFormats.Bgra32,
                null,
                pixels,
                width * 4
            );

            CanvasImage.Source = bitmap;
        }

        private void ButtonDrawPolygon_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDrawCircle_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
