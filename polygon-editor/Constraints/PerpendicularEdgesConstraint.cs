using System;
using System.Collections.Generic;

namespace polygon_editor {
    public class PerpendicularEdgesConstraint : Constraint {
        Polygon Polygon1;
        readonly int Vertex1_1;
        readonly int Vertex1_2;

        Polygon Polygon2;
        readonly int Vertex2_1;
        readonly int Vertex2_2;

        readonly Icon Icon1;
        readonly Icon Icon2;

        public PerpendicularEdgesConstraint(Polygon polygon1, int edge1, Polygon polygon2, int edge2) {
            Polygon1 = polygon1;
            Vertex1_1 = edge1;
            Vertex1_2 = (edge1 + 1) % polygon1.Points.Length;

            Polygon2 = polygon2;
            Vertex2_1 = edge2;
            Vertex2_2 = (edge2 + 1) % polygon2.Points.Length;

            Icon1 = new Icon(
                PrefabIcons.PERPENDICULAR_ICON_COLOR,
                PrefabIcons.ICON_SIZE,
                PrefabIcons.PERPENDICULAR_ICON
            );

            Icon2 = new Icon(
                PrefabIcons.PERPENDICULAR_ICON_COLOR,
                PrefabIcons.ICON_SIZE,
                PrefabIcons.PERPENDICULAR_ICON
            );

            UpdateIconsPositions();
        }

        private void UpdateIconsPositions() {
            if (Polygon1 == null || Polygon2 == null) return;
            (int, int) midpoint = Polygon1.EdgeMidpoint(Vertex1_1);
            Icon1.X = midpoint.Item1 + ICON_DISTANCE;
            Icon1.Y = midpoint.Item2 + ICON_DISTANCE;

            midpoint = Polygon2.EdgeMidpoint(Vertex2_1);
            Icon2.X = midpoint.Item1 + ICON_DISTANCE;
            Icon2.Y = midpoint.Item2 + ICON_DISTANCE;
        }

        public override void DrawIcons(DrawingPlane plane) {
            UpdateIconsPositions();
            plane.Draw(Icon1);
            plane.Draw(Icon2);
        }

        public override void ForceConstraint() {
            ForceConstraintWithInvariant(new HashSet<(Shape, int)>());
        }

        public override void ForceConstraintWithInvariant(HashSet<(Shape, int)> invariantIndices) {
            bool switchEdges =
                invariantIndices.Contains((Polygon2, Vertex2_1)) ||
                invariantIndices.Contains((Polygon2, Vertex2_2));

            var vrtx1_1 = Polygon1.Points[Vertex1_1];
            var vrtx1_2 = Polygon1.Points[Vertex1_2];
            var vrtx2_1 = Polygon2.Points[Vertex2_1];
            var vrtx2_2 = Polygon2.Points[Vertex2_2];
            if(switchEdges) {
                (vrtx1_1, vrtx2_1) = (vrtx2_1, vrtx1_1);
                (vrtx1_2, vrtx2_2) = (vrtx2_2, vrtx1_2);
            }

            var polygon = switchEdges ? Polygon1 : Polygon2;
            var midpoint2 = polygon.EdgeMidpoint(switchEdges ? Vertex1_1 : Vertex2_1);

            (double, double) vec1 = (vrtx1_2.Item1 - vrtx1_1.Item1, vrtx1_2.Item2 - vrtx1_1.Item2);
            (double, double) vec_mid2 = (vrtx2_2.Item1 - midpoint2.Item1, vrtx2_2.Item2 - midpoint2.Item2);
            double dotProduct = MathUtils.DotProduct(vec1, vec_mid2);

            double phi = Math.Atan(dotProduct / MathUtils.Determinant(vec1, vec_mid2));

            if (Math.Abs(dotProduct) / Math.PI * 180 <= MAX_ABSOLUTE_ERROR) return;

            polygon.Points[switchEdges ? Vertex1_1 : Vertex2_1] = MathUtils.RoundVector(MathUtils.RotateByCenter(midpoint2, vrtx2_1, phi));
            polygon.Points[switchEdges ? Vertex1_2 : Vertex2_2] = MathUtils.RoundVector(MathUtils.RotateByCenter(midpoint2, vrtx2_2, phi));

            ForceRecurrentContstraint(polygon, switchEdges ? Vertex1_1 : Vertex2_1, switchEdges ? Vertex1_1 : Vertex2_1);
            if(RecurrentApplicationCount == 0) {
                ForceRecurrentContstraint(polygon, switchEdges ? Vertex1_2 : Vertex2_2, switchEdges ? Vertex1_2 : Vertex2_2);
            }
        }

        private void ForceRecurrentContstraint(Polygon poly, int edge, int invVert) {
            RecurrentApplicationCount += 1;
            if(RecurrentApplicationCount < RECURRENT_APPLICATION_LIMIT && poly.Constraints[edge] != null) {
                poly.Constraints[edge].ForceConstraintWithInvariant(
                    new HashSet<(Shape, int)> { (poly, invVert) }
                );
            }
            RecurrentApplicationCount -= 1;
        }

        public override void Neutralize() {
            Polygon1.Constraints[Vertex1_1] = null;
            Polygon1 = null;
            Polygon2.Constraints[Vertex2_1] = null;
            Polygon2 = null;
        }
    }
}
