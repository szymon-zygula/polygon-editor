using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    public class Polygon {
        public (int, int)[] Points { get; set; }
        public UInt32 Color { get; set; }
        public int Index { get; set; }

        public Polygon() {
            Points = new (int, int)[0];
        }

        public (int, int) EdgeMidpoint(int n) {
            int idx1 = n;
            int idx2 = n == Points.Length - 1 ? 0 : n + 1;
            return ((Points[idx1].Item1 + Points[idx2].Item1) / 2, (Points[idx1].Item2 + Points[idx2].Item2) / 2);
        }

        public void AddPoint(int x, int y) {
            InsertPointAt(x, y, Points.Length);
        }

        public void InsertPointAt(int x, int y, int n) {
            (int, int)[] newPoints = new (int, int)[Points.Length + 1];
            Array.Copy(Points, newPoints, n);
            Array.Copy(Points, n, newPoints, n + 1, Points.Length - n);
            newPoints[n] = (x, y);
            Points = newPoints;
        }

        public void RemoveNthPoint(int n) {
            (int, int)[] newPoints = new (int, int)[Points.Length - 1];
            Array.Copy(Points, newPoints, n);
            Array.Copy(Points, n + 1, newPoints, n, Points.Length - n - 1);
            Points = newPoints;
        }

        public void RemoveLastPoint() {
            RemoveNthPoint(Points.Length - 1);
        }

        public int? FindVertexWithinRadius(double x0, double y0, int radius) {
            for (int i = 0; i < Points.Length; ++i) {
                double x = Points[i].Item1 - x0;
                double y = Points[i].Item2 - y0;
                if (x * x + y * y < radius * radius) {
                    return i;
                }
            }

            return null;
        }

        public int EdgeLength(int n) {
            int idx1 = n;
            int idx2 = n == Points.Length - 1 ? 0 : n + 1;
            int x = Points[idx1].Item1 - Points[idx2].Item1;
            int y = Points[idx1].Item2 - Points[idx2].Item2;
            return (int)Math.Sqrt(x * x + y * y);
        }

        public int? FindEdgeWithinSquareRadius(double x0, double y0, int radius) {
            for (int i = 0; i < Points.Length; ++i) {
                (int, int) mid = EdgeMidpoint(i);
                if (Math.Abs(mid.Item1 - x0) <= radius && Math.Abs(mid.Item2 - y0) <= radius) {
                    return i;
                }
            }

            return null;
        }

        public (int, int) GetCenter() {
            int avgX = 0;
            int avgY = 0;
            int perimeter = 0;

            for (int i = 0; i < Points.Length; ++i) {
                int len = EdgeLength(i);
                (int, int) mid = EdgeMidpoint(i);
                avgX += mid.Item1 * len;
                avgY += mid.Item2 * len;
                perimeter += len;
            }

            avgX /= perimeter;
            avgY /= perimeter;

            return (avgX, avgY);
        }

        public override string ToString() {
            return $"Polygon {Index}";
        }
    }
}
