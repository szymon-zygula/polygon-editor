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
        static UInt32 CANVAS_NORMAL_LINE_COLOR = 0xFF0000FF;
        static UInt32 CANVAS_ACTIVE_LINE_COLOR = 0xFF00BB00;
        static UInt32 CANVAS_ACTIVE_VERTEX_COLOR = 0xFFFF0000;

        static int CANVAS_ACTIVE_VERTEX_RADIUS = 5;

        UInt32[] CanvasData;
        int CanvasWidth;
        int CanvasHeight;

        static Cursor CANVAS_NORMAL_CURSOR = Cursors.Arrow;
        static Cursor CANVAS_DRAW_CURSOR = Cursors.Cross;

        int TotalPolygonCount = 0;
        int TotalCircleCount = 0;

        // TODO: Just use CurrentlyDrawnPolygon != null
        bool IsDrawingPolygon = false;

        Polygon CurrentlyDrawnPolygon;
        Polygon ActivePolygon;
        List<Polygon> Polygons;

        public MainWindow()
        {
            Polygons = new List<Polygon>();
            InitializeComponent();
            CanvasWidth = (int)CanvasImage.Width;
            CanvasHeight = (int)CanvasImage.Height;
            CanvasData = new UInt32[CanvasHeight * CanvasWidth];

            BitmapHelper.Fill(CanvasData, CANVAS_BACKGROUND_COLOR, CanvasWidth, CanvasHeight);

            BitmapSource bitmap = BitmapSource.Create(
                CanvasWidth,
                CanvasHeight,
                96,
                96,
                PixelFormats.Bgra32,
                null,
                CanvasData,
                CanvasWidth * 4
            );

            CanvasImage.Source = bitmap;
        }

        private void UpdateCanvasImage()
        {
            BitmapHelper.Fill(CanvasData, CANVAS_BACKGROUND_COLOR, CanvasWidth, CanvasHeight);
            if(CurrentlyDrawnPolygon != null)
            {
                BitmapHelper.DrawIncompletePolygon(CanvasData, CanvasWidth, CurrentlyDrawnPolygon);
            }

            foreach(Polygon polygon in Polygons)
            {
                BitmapHelper.DrawPolygon(CanvasData, CanvasWidth, polygon);
            }

            if(ActivePolygon != null)
            {
                BitmapHelper.MarkPolygonVertices(CanvasData, CanvasWidth, CANVAS_ACTIVE_VERTEX_COLOR, CANVAS_ACTIVE_VERTEX_RADIUS, ActivePolygon);
            }

            BitmapSource bitmap = BitmapSource.Create(
                CanvasWidth,
                CanvasHeight,
                96,
                96,
                PixelFormats.Bgra32,
                null,
                CanvasData,
                CanvasWidth * 4
            );

            CanvasImage.Source = bitmap;
        }

        private void ButtonDrawPolygon_Click(object sender, RoutedEventArgs e)
        {
            CanvasImage.Cursor = CANVAS_DRAW_CURSOR;
            IsDrawingPolygon = true;
            CurrentlyDrawnPolygon = new Polygon();
            CurrentlyDrawnPolygon.Color = CANVAS_ACTIVE_LINE_COLOR;

            if(ActivePolygon != null)
            {
                ActivePolygon.Color = CANVAS_NORMAL_LINE_COLOR;
                ActivePolygon = null;
                ShapeList.SelectedItems.Clear();
                UpdateCanvasImage();
            }
        }

        private void ButtonDrawCircle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CanvasImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(IsDrawingPolygon)
            {
                if(CurrentlyDrawnPolygon.Points.Length == 0)
                {
                    CurrentlyDrawnPolygon.AddPoint(
                        (int)e.GetPosition(CanvasImage).X,
                        (int)e.GetPosition(CanvasImage).Y
                    );
                }

                CurrentlyDrawnPolygon.AddPoint(
                    (int)e.GetPosition(CanvasImage).X,
                    (int)e.GetPosition(CanvasImage).Y
                );

                UpdateCanvasImage();
            }
        }

        private void CanvasImage_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(IsDrawingPolygon)
            {
                StopDrawingPolygon();
            }

            if(ActivePolygon != null)
            {
                double mouseX = e.GetPosition(CanvasImage).X;
                double mouseY = e.GetPosition(CanvasImage).Y;
                for(int i = 0; i < ActivePolygon.Points.Length; ++i)
                {
                    double x = (double)ActivePolygon.Points[i].Item1 - mouseX;
                    double y = (double)ActivePolygon.Points[i].Item2 - mouseY;
                    if(x * x + y * y < CANVAS_ACTIVE_VERTEX_RADIUS * CANVAS_ACTIVE_VERTEX_RADIUS)
                    {
                        ActivePolygon.RemoveNthPoint(i);
                        if(ActivePolygon.Points.Length < 3)
                        {
                            Polygons.Remove(ActivePolygon);
                            ShapeList.Items.RemoveAt(ShapeList.SelectedIndex);
                            ShapeList.SelectedItems.Clear();
                            ActivePolygon = null;
                        }
                        UpdateCanvasImage();
                        return;
                    }
                }
            }
        }

        private void StopDrawingPolygon()
        {
            CanvasImage.Cursor = CANVAS_NORMAL_CURSOR;
            IsDrawingPolygon = false;

            if(CurrentlyDrawnPolygon.Points.Length >= 4)
            {
                Polygons.Add(CurrentlyDrawnPolygon);
                TotalPolygonCount += 1;
                CurrentlyDrawnPolygon.Index = TotalPolygonCount;
                CurrentlyDrawnPolygon.Color = CANVAS_NORMAL_LINE_COLOR;
                CurrentlyDrawnPolygon.RemoveLastPoint();
                ShapeList.Items.Add(CurrentlyDrawnPolygon);
            }

            CurrentlyDrawnPolygon = null;
            UpdateCanvasImage();
        }

        private void CanvasImage_MouseMove(object sender, MouseEventArgs e)
        {
            if(IsDrawingPolygon && CurrentlyDrawnPolygon.Points.Length > 0)
            {
                CurrentlyDrawnPolygon.Points[CurrentlyDrawnPolygon.Points.Length - 1] = (
                    (int)e.GetPosition(CanvasImage).X,
                    (int)e.GetPosition(CanvasImage).Y
                );

                UpdateCanvasImage();
            }
        }

        private void ShapeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ActivePolygon != null)
            {
                ActivePolygon.Color = CANVAS_NORMAL_LINE_COLOR;
            }

            ActivePolygon = ShapeList.SelectedItem as Polygon;

            if(ActivePolygon == null)
            {
                return;
            }

            ActivePolygon.Color = CANVAS_ACTIVE_LINE_COLOR;
            UpdateCanvasImage();
        }
    }
}
