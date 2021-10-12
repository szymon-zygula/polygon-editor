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
    }
}
