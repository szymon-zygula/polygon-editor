using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace polygon_editor {
    public class DrawingPlane {
        public int Width { get; }
        public int Height { get; }
        private UInt32[] Pixels;

        public DrawingPlane(int width, int height) {
            Width = width;
            Height = height;
            Pixels = new UInt32[width * height];
        }

        public void SetPixel(int x, int y, UInt32 color) {
            if(x < 0 || x >= Width || y < 0 || y >= Height) {
                return;
            }

            Pixels[x + y * Width] = color;
        }

        public void Fill(UInt32 color) {
            for (int i = 0; i < Width; ++i) {
                for (int j = 0; j < Height; ++j) {
                    Pixels[i + j * Width] = color;
                }
            }
        }

        public void DrawIncompletePolygon(Polygon polygon) {
            for (int i = 1; i < polygon.Points.Length; ++i) {
                BresenhamDrawer.Line(
                    this,
                    polygon.Color,
                    polygon.Points[i - 1].Item1, polygon.Points[i - 1].Item2,
                    polygon.Points[i].Item1, polygon.Points[i].Item2
                );
            }
        }

        public void DrawPolygon(Polygon polygon) {
            DrawIncompletePolygon(polygon);
            BresenhamDrawer.Line(
                this,
                polygon.Color,
                polygon.Points.Last().Item1, polygon.Points.Last().Item2,
                polygon.Points.First().Item1, polygon.Points.First().Item2
            );
        }

        public void DrawCircle(Circle circle) {
            BresenhamDrawer.Circle(
                this,
                circle.Color,
                circle.R,
                circle.X,
                circle.Y
            );
        }

        private Polygon CreateSquare(UInt32 color, int x, int y, int r) {
            Polygon square = new Polygon();
            square.AddPoint(x + r, y + r);
            square.AddPoint(x - r, y + r);
            square.AddPoint(x - r, y - r);
            square.AddPoint(x + r, y - r);
            square.Color = color;
            return square;
        }

        public void MarkPolygonEdges(UInt32 color, int r, Polygon polygon) {
            for (int i = 0; i < polygon.Points.Length; ++i) {
                MarkPolygonEdge(color, r, polygon, i);
            }
        }

        public void MarkPolygonEdge(UInt32 color, int r, Polygon polygon, int edge) {
            (int, int) mid = polygon.EdgeMidpoint(edge);
            Polygon square = CreateSquare(color, mid.Item1, mid.Item2, r);
            DrawPolygon(square);
        }

        public void MarkPolygonVertices(UInt32 color, int r, Polygon polygon) {
            foreach ((int, int) point in polygon.Points) {
                BresenhamDrawer.Circle(
                    this,
                    color,
                    r,
                    point.Item1,
                    point.Item2
                );
            }
        }

        public void MarkPolygonCenter(UInt32 color, int r, Polygon polygon) {
            (int, int) center = polygon.GetCenter();
            Polygon square = CreateSquare(color, center.Item1, center.Item2, r);
            DrawPolygon(square);
        }

        public void MarkCircleCenter(UInt32 color, int r, Circle circle) {
            Polygon square = CreateSquare(color, circle.X, circle.Y, r);
            DrawPolygon(square);
        }

        public void MarkCircleTop(UInt32 color, int r, Circle circle) {
            BresenhamDrawer.Circle(
                this,
                color,
                r,
                circle.X,
                circle.Y - circle.R
            );
        }

        public BitmapSource CreateBitmapSource() {
            return BitmapSource.Create(
                Width,
                Height,
                96,
                96,
                PixelFormats.Bgra32,
                null,
                Pixels,
                Width * 4
            );
        }
    }
}
