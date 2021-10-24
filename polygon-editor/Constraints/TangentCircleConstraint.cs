using System;
using System.Collections.Generic;

namespace polygon_editor {
    public class TangentCircleConstraint : Constraint {
        readonly Circle Circle;
        readonly Polygon Polygon;
        readonly int Edge;

        readonly Icon Icon1;
        readonly Icon Icon2;

        public TangentCircleConstraint(Circle circle, Polygon polygon, int edge) {
            Circle = circle;
            Polygon = polygon;
            Edge = edge;

            Icon1 = new Icon(
                PrefabIcons.TANGENT_CIRCLE_ICON_COLOR,
                PrefabIcons.ICON_SIZE,
                PrefabIcons.TANGENT_CIRCLE_ICON
            );

            Icon2 = new Icon(
                PrefabIcons.TANGENT_CIRCLE_ICON_COLOR,
                PrefabIcons.ICON_SIZE,
                PrefabIcons.TANGENT_CIRCLE_ICON
            );
        }

        public override void DrawIcons(DrawingPlane plane) {
            UpdateIconsPositions();
            plane.Draw(Icon1);
            plane.Draw(Icon2);
        }

        private void UpdateIconsPositions() {
            (int, int) midpoint = Polygon.EdgeMidpoint(Edge);
            Icon1.X = midpoint.Item1 + ICON_DISTANCE;
            Icon1.Y = midpoint.Item2 + ICON_DISTANCE;
            Icon2.X = Circle.X + ICON_DISTANCE;
            Icon2.Y = Circle.Y + ICON_DISTANCE;
        }

        public override void ForceConstraint() {
            var vrt1 = Polygon.Points[Edge];
            var vrt2 = Polygon.Points[(Edge + 1) % Polygon.Points.Length];
            double A = vrt1.Item2 - vrt2.Item2;
            double B = vrt2.Item1 - vrt1.Item1;
            double C = -vrt1.Item2 * B - A * vrt1.Item1;
            double d = Math.Abs(A * Circle.X + B * Circle.Y + C) / Math.Sqrt(A * A + B * B);
            Circle.R = (int)d;
        }

        public override void ForceConstraintWithInvariant(HashSet<(Shape, int)> invariantIndices) {
            ForceConstraint();
        }
    }
}
