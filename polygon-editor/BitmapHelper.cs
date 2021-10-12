using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor
{
    public static class BitmapHelper
    {
        public static void Fill(UInt32[] pixels, UInt32 color, int width, int height)
        {
            for(int i = 0; i < width; ++i)
            {
                for(int j = 0; j < height; ++j)
                {
                    pixels[i + j * width] = color;
                }
            }
        }

        public static void DrawIncompletePolygon(UInt32[] pixels, int width, Polygon polygon)
        {
            for(int i = 1; i < polygon.Points.Length; ++i)
            {
                BresenhamDrawer.Line(
                    pixels,
                    polygon.Color,
                    width,
                    polygon.Points[i - 1].Item1, polygon.Points[i - 1].Item2,
                    polygon.Points[i].Item1, polygon.Points[i].Item2
                );
            }
        }

        public static void DrawPolygon(UInt32[] pixels, int width, Polygon polygon)
        {
            DrawIncompletePolygon(pixels, width, polygon);
            BresenhamDrawer.Line(
                pixels,
                polygon.Color,
                width,
                polygon.Points.Last().Item1, polygon.Points.Last().Item2,
                polygon.Points.First().Item1, polygon.Points.First().Item2
            );
        }

        public static void MarkPolygonVertices(UInt32[] pixels, int width, UInt32 color, int r, Polygon polygon)
        {
            foreach((int, int) point in polygon.Points)
            {
                BresenhamDrawer.Circle(
                    pixels,
                    color,
                    width,
                    r,
                    point.Item1,
                    point.Item2
                );
            }
        }
    }
}
