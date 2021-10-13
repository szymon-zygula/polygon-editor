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
        static UInt32 CANVAS_ACTIVE_EDGE_COLOR = 0xFF0000FF;

        static int CANVAS_ACTIVE_VERTEX_RADIUS = 5;
        static int CANVAS_ACTIVE_EDGE_RADIUS = CANVAS_ACTIVE_VERTEX_RADIUS;

        UInt32[] CanvasData;
        int CanvasWidth;
        int CanvasHeight;

        static Cursor CANVAS_NORMAL_CURSOR = Cursors.Arrow;
        static Cursor CANVAS_DRAW_CURSOR = Cursors.Cross;

        int TotalPolygonCount = 0;
        int TotalCircleCount = 0;

        Polygon ActivePolygon;
        int? CurrentlyDrawnVertex = null;
        int? CurrentlyDrawnEdge = null;

        List<Polygon> Polygons;
        Polygon CurrentlyDrawnPolygon;

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
                BitmapHelper.MarkPolygonVertices(
                    CanvasData,
                    CanvasWidth,
                    CANVAS_ACTIVE_VERTEX_COLOR,
                    CANVAS_ACTIVE_VERTEX_RADIUS,
                    ActivePolygon
                );

                BitmapHelper.MarkPolygonEdges(
                    CanvasData,
                    CanvasWidth,
                    CANVAS_ACTIVE_EDGE_COLOR,
                    CANVAS_ACTIVE_VERTEX_RADIUS,
                    ActivePolygon
                );
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
            CurrentlyDrawnPolygon = new Polygon();
            CurrentlyDrawnPolygon.Color = CANVAS_ACTIVE_LINE_COLOR;

            if(ActivePolygon != null)
            {
                ActivePolygon.Color = CANVAS_NORMAL_LINE_COLOR;
                DeactivateShapes();
                UpdateCanvasImage();
            }
        }

        private void ButtonDrawCircle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CanvasImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(CurrentlyDrawnPolygon != null)
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
            else if(CurrentlyDrawnVertex != null)
            {
                CurrentlyDrawnVertex = null;
            }
            else if(CurrentlyDrawnEdge != null)
            {
                CurrentlyDrawnEdge = null;
            }
        }

        private void CanvasImage_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(CurrentlyDrawnPolygon != null)
            {
                StopDrawingPolygon();
            }
            else if(ActivePolygon != null && CurrentlyDrawnVertex == null)
            {
                double mouseX = e.GetPosition(CanvasImage).X;
                double mouseY = e.GetPosition(CanvasImage).Y;
                int? activeVertex = ActivePolygon.FindVertexWithinRadius(
                    mouseX, mouseY, CANVAS_ACTIVE_VERTEX_RADIUS);
                if (activeVertex == null)
                {
                    return;
                }

                ActivePolygon.RemoveNthPoint(activeVertex.Value);
                if (ActivePolygon.Points.Length < 3)
                {
                    Polygons.Remove(ActivePolygon);
                    ShapeList.Items.RemoveAt(ShapeList.SelectedIndex);
                    DeactivateShapes();
                }
                UpdateCanvasImage();
            }
        }

        private void DeactivateShapes()
        {
            ShapeList.SelectedItems.Clear();
            ActivePolygon = null;
        }

        private void StopDrawingPolygon()
        {
            CanvasImage.Cursor = CANVAS_NORMAL_CURSOR;

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
            int mouseX = (int)e.GetPosition(CanvasImage).X;
            int mouseY = (int)e.GetPosition(CanvasImage).Y;
            if(CurrentlyDrawnPolygon != null && CurrentlyDrawnPolygon.Points.Length > 0)
            {
                CurrentlyDrawnPolygon.Points[CurrentlyDrawnPolygon.Points.Length - 1] = (mouseX, mouseY);
                UpdateCanvasImage();
            }
            else if(CurrentlyDrawnVertex != null)
            {
                ActivePolygon.Points[CurrentlyDrawnVertex.Value] = (mouseX, mouseY);
                UpdateCanvasImage();
            }
            else if(CurrentlyDrawnEdge != null)
            {
                (int, int) mid = ActivePolygon.EdgeMidpoint(CurrentlyDrawnEdge.Value);
                int deltaX = mouseX - mid.Item1;
                int deltaY = mouseY - mid.Item2;
                int idx1 = CurrentlyDrawnEdge.Value;
                int idx2 = CurrentlyDrawnEdge.Value == ActivePolygon.Points.Length - 1
                    ? 0
                    : CurrentlyDrawnEdge.Value + 1;
                ActivePolygon.Points[idx1].Item1 += deltaX;
                ActivePolygon.Points[idx1].Item2 += deltaY;
                ActivePolygon.Points[idx2].Item1 += deltaX;
                ActivePolygon.Points[idx2].Item2 += deltaY;
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

        private void CanvasImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(ActivePolygon != null)
            {
                double mouseX = e.GetPosition(CanvasImage).X;
                double mouseY = e.GetPosition(CanvasImage).Y;
                int? activeVertex = ActivePolygon.FindVertexWithinRadius(
                    mouseX, mouseY, CANVAS_ACTIVE_VERTEX_RADIUS);

                if(activeVertex != null)
                {
                    CurrentlyDrawnVertex = activeVertex.Value;
                    return;
                }

                int? activeEdge = ActivePolygon.FindEdgeWithinSquareRadius(
                    mouseX, mouseY, CANVAS_ACTIVE_EDGE_RADIUS);

                if(activeEdge != null)
                {
                    CurrentlyDrawnEdge = activeEdge.Value;
                    return;
                }
            }
        }

        private void CanvasImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Middle && ActivePolygon != null)
            {
                double mouseX = e.GetPosition(CanvasImage).X;
                double mouseY = e.GetPosition(CanvasImage).Y;
                int? edge = ActivePolygon.FindEdgeWithinSquareRadius(mouseX, mouseY, CANVAS_ACTIVE_EDGE_RADIUS);
                if (edge == null) return;
                (int, int) midpoint = ActivePolygon.EdgeMidpoint(edge.Value);
                ActivePolygon.InsertPointAt(midpoint.Item1, midpoint.Item2, edge.Value + 1);
                UpdateCanvasImage();
            }
        }
    }
}
