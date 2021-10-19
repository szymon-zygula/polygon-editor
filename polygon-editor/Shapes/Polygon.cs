using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace polygon_editor {
    public class Polygon : Shape {
        public (int, int)[] Points { get; set; }
        public UInt32 Color { get; set; }

        public Constraint[] Constraints;

        public Polygon() {
            Constraints = new Constraint[0];
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
            Points = InsertElemAt((x, y), Points, n);
            Constraints = InsertElemAt(null, Constraints, n);
        }

        private T[] InsertElemAt<T>(T elem, T[] array, int n) {
            T[] newElems = new T[array.Length + 1];
            Array.Copy(array, newElems, n);
            Array.Copy(array, n, newElems, n + 1, array.Length - n);
            newElems[n] = elem;
            return newElems;
        }

        public void RemoveNthPoint(int n) {
            Points = RemoveNthElem(n, Points);
            Constraints = RemoveNthElem(n, Constraints);
        }

        private T[] RemoveNthElem<T>(int n, T[] array) {
            T[] newElems = new T[array.Length - 1];
            Array.Copy(array, newElems, n);
            Array.Copy(array, n + 1, newElems, n, array.Length - n - 1);
            return newElems;
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

        public void DrawIncompleteOn(DrawingPlane plane) {
            for (int i = 1; i < Points.Length; ++i) {
                BresenhamDrawer.Line(
                    plane, Color,
                    Points[i - 1].Item1, Points[i - 1].Item2,
                    Points[i].Item1, Points[i].Item2
                );
            }
        }

        public override void DrawOn(DrawingPlane plane) {
            DrawIncompleteOn(plane);
            BresenhamDrawer.Line(
                plane, Color,
                Points.Last().Item1, Points.Last().Item2,
                Points.First().Item1, Points.First().Item2
            );

            for(int i = 0; i < Constraints.Length; ++i) {
                if (Constraints[i] != null) Constraints[i].DrawIcons(plane);
            }
        }

        public void ForceConstraintWithInvariantVertex(int vertex) {
            if (Constraints[vertex] != null) {
                Constraints[vertex].ForceConstraintWithInvariant(
                        new HashSet<(Shape, int)> { (this, vertex) }
                );
            }

            int prevEdge = vertex > 0 ? vertex - 1 : Points.Length - 1;

            if (Constraints[prevEdge] != null) {
                Constraints[prevEdge].ForceConstraintWithInvariant(
                        new HashSet<(Shape, int)> { (this, vertex) }
                );
            }
        }

        public void ForceConstraintWithInvariantEdge(int edge) {
            int prevEdge = edge > 0 ? edge - 1 : Points.Length - 1;
            int nextEdge = edge == Points.Length - 1 ? 0 : edge + 1;
            HashSet<(Shape, int)> invariant = new HashSet<(Shape, int)> {
                (this, edge), (this, edge == Points.Length - 1 ? 0 : edge + 1)
            };

            if (Constraints[edge] != null) {
                Constraints[edge].ForceConstraintWithInvariant(invariant);
            }

            if (Constraints[prevEdge] != null) {
                Constraints[prevEdge].ForceConstraintWithInvariant(invariant);
            }

            if (Constraints[nextEdge] != null) {
                Constraints[nextEdge].ForceConstraintWithInvariant(invariant);
            }
        }
    }
}
