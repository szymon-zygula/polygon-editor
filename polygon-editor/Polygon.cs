using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor
{
    public class Polygon
    {
        public (int, int)[] Points { get; set; }
        public UInt32 Color { get; set; }
        public int Index { get; set; }

        public Polygon()
        {
            Points = new (int, int)[0];
        }

        public void AddPoint(int x, int y)
        {
            (int, int)[] newPoints = new (int, int)[Points.Length + 1];
            Array.Copy(Points, newPoints, Points.Length);
            newPoints[newPoints.Length - 1] = (x, y);
            Points = newPoints;
        }

        public void RemoveLastPoint()
        {
            (int, int)[] newPoints = new (int, int)[Points.Length - 1];
            Array.Copy(Points, newPoints, Points.Length - 1);
            Points = newPoints;
        }

        public override string ToString()
        {
            return $"Polygon {Index}";
        }
    }
}
