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
        static readonly UInt32 CANVAS_BACKGROUND_COLOR = 0xFFE0E0E0;
        static readonly UInt32 CANVAS_NORMAL_LINE_COLOR = 0xFF0000FF;
        static readonly UInt32 CANVAS_ACTIVE_LINE_COLOR = 0xFF00BB00;
        static readonly UInt32 CANVAS_ACTIVE_VERTEX_COLOR = 0xFFFF0000;
        static readonly UInt32 CANVAS_ACTIVE_EDGE_COLOR = 0xFF0000FF;
        static readonly UInt32 CANVAS_ACTIVE_CENTER_COLOR = 0xFFFF00FF;

        static readonly int CANVAS_ACTIVE_VERTEX_RADIUS = 5;
        static readonly int CANVAS_ACTIVE_EDGE_RADIUS = CANVAS_ACTIVE_VERTEX_RADIUS;

        readonly UInt32[] CanvasData;
        readonly int CanvasWidth;
        readonly int CanvasHeight;

        static readonly Cursor CANVAS_NORMAL_CURSOR = Cursors.Arrow;
        static readonly Cursor CANVAS_DRAW_CURSOR = Cursors.Cross;

        int TotalPolygonCount = 0;
        int TotalCircleCount = 0;

        Polygon ActivePolygon;
        int? CurrentlyDrawnVertex = null;
        int? CurrentlyDrawnEdge = null;

        bool IsMovingShape;
        bool IsResizingCircle;

        readonly List<Polygon> Polygons;
        Polygon CurrentlyDrawnPolygon;

        readonly List<Circle> Circles;
        Circle CurrentlyDrawnCircle;

        Circle ActiveCircle;

        public MainWindow()
        {
            Polygons = new List<Polygon>();
            Circles = new List<Circle>();
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

            if(CurrentlyDrawnCircle != null && CurrentlyDrawnCircle.R > 0)
            {
                BitmapHelper.DrawCircle(CanvasData, CanvasWidth, CurrentlyDrawnCircle);
            }

            foreach(Polygon polygon in Polygons)
            {
                BitmapHelper.DrawPolygon(CanvasData, CanvasWidth, polygon);
            }

            foreach(Circle circle in Circles)
            {
                BitmapHelper.DrawCircle(CanvasData, CanvasWidth, circle);
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

                BitmapHelper.MarkPolygonCenter(
                    CanvasData,
                    CanvasWidth,
                    CANVAS_ACTIVE_CENTER_COLOR,
                    CANVAS_ACTIVE_VERTEX_RADIUS,
                    ActivePolygon
                );
            }

            if(ActiveCircle != null)
            {
                BitmapHelper.MarkCircleCenter(
                    CanvasData,
                    CanvasWidth,
                    CANVAS_ACTIVE_CENTER_COLOR,
                    CANVAS_ACTIVE_VERTEX_RADIUS,
                    ActiveCircle
                );

                BitmapHelper.MarkCircleTop(
                    CanvasData,
                    CanvasWidth,
                    CANVAS_ACTIVE_EDGE_COLOR,
                    CANVAS_ACTIVE_VERTEX_RADIUS,
                    ActiveCircle
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
            CurrentlyDrawnPolygon = new Polygon
            {
                Color = CANVAS_ACTIVE_LINE_COLOR
            };
            if (IsAnyShapeActive()) DeactivateShapes();
        }

        private void ButtonDrawCircle_Click(object sender, RoutedEventArgs e)
        {
            CanvasImage.Cursor = CANVAS_DRAW_CURSOR;
            CurrentlyDrawnCircle = new Circle
            {
                Color = CANVAS_ACTIVE_LINE_COLOR,
                R = -1
            };
            if (IsAnyShapeActive()) DeactivateShapes();
        }

        private void CanvasImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int mouseX = (int)e.GetPosition(CanvasImage).X;
            int mouseY = (int)e.GetPosition(CanvasImage).Y;
            if(CurrentlyDrawnPolygon != null)
            {
                if(CurrentlyDrawnPolygon.Points.Length == 0)
                {
                    CurrentlyDrawnPolygon.AddPoint(mouseX, mouseY);
                }

                CurrentlyDrawnPolygon.AddPoint(mouseX, mouseY);
                UpdateCanvasImage();
            }
            else if(CurrentlyDrawnCircle != null)
            {
                CurrentlyDrawnCircle.X = mouseX;
                CurrentlyDrawnCircle.Y = mouseY;
                CurrentlyDrawnCircle.R = 0;
            }
            else if(CurrentlyDrawnVertex != null)
            {
                CurrentlyDrawnVertex = null;
            }
            else if(CurrentlyDrawnEdge != null)
            {
                CurrentlyDrawnEdge = null;
            }
            else if(IsMovingShape)
            {
                IsMovingShape = false;
            }
            else if(ActiveCircle != null && IsResizingCircle)
            {
                IsResizingCircle = false;
            }
        }

        private void CanvasImage_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            double mouseX = e.GetPosition(CanvasImage).X;
            double mouseY = e.GetPosition(CanvasImage).Y;
            if(CurrentlyDrawnPolygon != null) StopDrawingPolygon();
            else if(CurrentlyDrawnCircle != null)
            {
                int x = CurrentlyDrawnCircle.X - (int)mouseX;
                int y = CurrentlyDrawnCircle.Y - (int)mouseY;
                CurrentlyDrawnCircle.R = (int)Math.Sqrt(x * x + y * y);
                StopDrawingCircle();
            }
            else if(ActivePolygon != null && CurrentlyDrawnVertex == null)
            {
                int? activeVertex = ActivePolygon.FindVertexWithinRadius(
                    mouseX, mouseY, CANVAS_ACTIVE_VERTEX_RADIUS);
                if (activeVertex != null)
                {
                    ActivePolygon.RemoveNthPoint(activeVertex.Value);
                    if (ActivePolygon.Points.Length < 3) RemoveActivePolygon();
                    UpdateCanvasImage();
                }

                (int, int) center = ActivePolygon.GetCenter();
                if (
                    Math.Abs(center.Item1 - mouseX) <= CANVAS_ACTIVE_VERTEX_RADIUS &&
                    Math.Abs(center.Item2 - mouseY) <= CANVAS_ACTIVE_VERTEX_RADIUS)
                {
                    RemoveActivePolygon();
                    UpdateCanvasImage();
                }
            }
            if(
                ActiveCircle != null &&
                Math.Abs(ActiveCircle.X - mouseX) <= CANVAS_ACTIVE_VERTEX_RADIUS &&
                Math.Abs(ActiveCircle.Y - mouseY) <= CANVAS_ACTIVE_VERTEX_RADIUS)
            {
                Circles.Remove(ActiveCircle);
                ShapeList.Items.RemoveAt(ShapeList.SelectedIndex);
                DeactivateShapes();
                UpdateCanvasImage();
            }
        }

        private void RemoveActivePolygon()
        {
            Polygons.Remove(ActivePolygon);
            ShapeList.Items.RemoveAt(ShapeList.SelectedIndex);
            DeactivateShapes();
        }

        private bool IsAnyShapeActive()
        {
            return ActivePolygon != null || ActiveCircle != null;
        }

        private void DeactivateShapesWithoutCleaningList()
        {
            if(ActivePolygon != null)
            {
                ActivePolygon.Color = CANVAS_NORMAL_LINE_COLOR;
                ActivePolygon = null;
            }
            else if(ActiveCircle != null)
            {
                ActiveCircle.Color = CANVAS_NORMAL_LINE_COLOR;
                ActiveCircle = null;
            }

            UpdateCanvasImage();
        }

        private void DeactivateShapes()
        {
            ShapeList.SelectedItems.Clear();
            DeactivateShapesWithoutCleaningList();
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
        private void StopDrawingCircle()
        {
            CanvasImage.Cursor = CANVAS_NORMAL_CURSOR;
            Circles.Add(CurrentlyDrawnCircle);
            TotalCircleCount += 1;
            CurrentlyDrawnCircle.Index = TotalCircleCount;
            CurrentlyDrawnCircle.Color = CANVAS_NORMAL_LINE_COLOR;
            ShapeList.Items.Add(CurrentlyDrawnCircle);
            CurrentlyDrawnCircle = null;
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
            else if(CurrentlyDrawnCircle != null && CurrentlyDrawnCircle.R != -1)
            {
                int x = mouseX - CurrentlyDrawnCircle.X;
                int y = mouseY - CurrentlyDrawnCircle.Y;
                CurrentlyDrawnCircle.R = (int)Math.Sqrt(x * x + y * y);
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
            else if(IsMovingShape)
            {
                if(ActivePolygon != null)
                {
                    (int, int) center = ActivePolygon.GetCenter();
                    int deltaX = mouseX - center.Item1;
                    int deltaY = mouseY - center.Item2;
                    for(int i = 0; i < ActivePolygon.Points.Length; ++i)
                    {
                        ActivePolygon.Points[i].Item1 += deltaX;
                        ActivePolygon.Points[i].Item2 += deltaY;
                    }
                }
                else if(ActiveCircle != null)
                {
                    ActiveCircle.X = mouseX;
                    ActiveCircle.Y = mouseY;
                }
                UpdateCanvasImage();
            }
            else if(ActiveCircle != null && IsResizingCircle)
            {
                ActiveCircle.R = Math.Abs(ActiveCircle.Y - mouseY);
                UpdateCanvasImage();
            }
        }

        private void ShapeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsAnyShapeActive()) DeactivateShapesWithoutCleaningList();

            if(ShapeList.SelectedItem is Polygon)
            {
                ActivePolygon = ShapeList.SelectedItem as Polygon;
                if(ActivePolygon == null) return;
                ActivePolygon.Color = CANVAS_ACTIVE_LINE_COLOR;
            }
            else if(ShapeList.SelectedItem is Circle)
            {
                ActiveCircle = ShapeList.SelectedItem as Circle;
                if(ActiveCircle == null) return;
                ActiveCircle.Color = CANVAS_ACTIVE_LINE_COLOR;
            }

            UpdateCanvasImage();
        }

        private void CanvasImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ActivePolygon != null)
            {
                double mouseX = e.GetPosition(CanvasImage).X;
                double mouseY = e.GetPosition(CanvasImage).Y;
                (int, int) center = ActivePolygon.GetCenter();
                if(
                    Math.Abs(center.Item1 - mouseX) <= CANVAS_ACTIVE_VERTEX_RADIUS &&
                    Math.Abs(center.Item2 - mouseY) <= CANVAS_ACTIVE_VERTEX_RADIUS)
                {
                    IsMovingShape = true;
                    return;
                }

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
            else if(ActiveCircle != null)
            {
                double mouseX = e.GetPosition(CanvasImage).X;
                double mouseY = e.GetPosition(CanvasImage).Y;
                if(
                    Math.Abs(ActiveCircle.X - mouseX) <= CANVAS_ACTIVE_VERTEX_RADIUS &&
                    Math.Abs(ActiveCircle.Y - mouseY) <= CANVAS_ACTIVE_VERTEX_RADIUS)
                {
                    IsMovingShape = true;
                    return;
                }

                int x = ActiveCircle.X - (int)mouseX;
                int y = ActiveCircle.Y - ActiveCircle.R - (int)mouseY;
                if(Math.Sqrt(x * x + y * y) <= CANVAS_ACTIVE_VERTEX_RADIUS)
                {
                    IsResizingCircle = true;
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
