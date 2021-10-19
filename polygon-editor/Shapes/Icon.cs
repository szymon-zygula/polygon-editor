using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    public class Icon : Shape {
        int Size;
        UInt32[] Inside;
        Polygon Box;

        private int _X;

        public int X {
            get {
                return _X;
            }
            set {
                _X = value;
                UpdateBorder();
            }
        }

        private int _Y;
        public int Y {
            get {
                return _Y;
            }
            set {
                _Y = value;
                UpdateBorder();
            }
        }

        public Icon(UInt32 borderColor, int size, UInt32[] inside) {
            Inside = inside;
            Size = size;
            Box = new Polygon();
            Box.AddPoint(0, 0);
            Box.AddPoint(Size + 1, 0);
            Box.AddPoint(Size + 1, Size + 1);
            Box.AddPoint(0, Size + 1);
            Box.Color = borderColor;
        }

        private void UpdateBorder() {
            Box.Points[0] = (X, Y);
            Box.Points[1] = (X + Size + 1, Y);
            Box.Points[2] = (X + Size + 1, Y + Size + 1);
            Box.Points[3] = (X, Y + Size + 1);
        }

        public override void DrawOn(DrawingPlane plane) {
            plane.Draw(Box);
            for(int i = 0; i < Size; ++i) {
                for (int j = 0; j < Size; ++j) {
                    plane.SetPixel(X + i + 1, Y + j + 1, Inside[i + j * Size]);
                }
            }
        }
    }
}
