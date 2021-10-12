using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor
{
    public static class BresenhamDrawer
    {
        private static void LineLow(UInt32[] pixels, UInt32 color, int width, int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int yi = 1;
            if(dy < 0)
            {
                y1 = -1;
                dy = -dy;
            }

            int d = 2 * dy - dx;
            int y = y0;

            for(int x = x0; x <= x1; ++x)
            {
                pixels[x + y * width] = color;
                if(d > 0)
                {
                    y += yi;
                    d += (2 * (dy - dx));
                }
                else
                {
                    d += 2 * dy;
                }
            }
        }

        private static void LineHigh(UInt32[] pixels, UInt32 color, int width, int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int xi = 1;
            if(dx < 0) {
                xi = -1;
                dx = -dx;
            }

            int d = 2 * dx - dy;
            int x = x0;

            for(int y = y0; y <= y1; ++y)
            {
                pixels[x + y * width] = color;
                if(d > 0)
                {
                    x += xi;
                    d += 2 * (dx - dy);
                }
                else
                {
                    d += 2 * dx;
                }
            }
        }

        public static void Line(UInt32[] pixels, UInt32 color, int width, int x0, int y0, int x1, int y1)
        {
            if(Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
            {
                if(x0 > x1)
                {
                    BresenhamDrawer.LineLow(pixels, color, width, x1, y1, x0, y0);
                }
                else
                {
                    BresenhamDrawer.LineLow(pixels, color, width, x0, y0, x1, y1);
                }
            }
            else
            {
                if(y0 > y1)
                {
                    BresenhamDrawer.LineHigh(pixels, color, width, x1, y1, x0, y0);
                }
                else
                {
                    BresenhamDrawer.LineHigh(pixels, color, width, x0, y0, x1, y1);
                }
            }
        }
    }
}
