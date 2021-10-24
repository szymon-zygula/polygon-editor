using System;
using System.Collections.Generic;

namespace polygon_editor {
    class ConstantEdgeLengthConstraint : Constraint {
        Polygon Polygon;
        int Vertex1;
        int Vertex2;
        readonly int Length;

        public ConstantEdgeLengthConstraint(Polygon polygon, int edge, int length) {
            Polygon = polygon;
            Vertex1 = edge;
            Vertex2 = edge == polygon.Points.Length - 1 ? 0 : edge + 1;
            Length = length;

            Icon = new Icon(
                PrefabIcons.CONST_LENGTH_ICON_COLOR,
                PrefabIcons.ICON_SIZE,
                PrefabIcons.CONST_LENGTH_ICON
            );

            UpdateIconPosition();
        }

        private void UpdateIconPosition() {
            if (Polygon == null) return;
            (int, int) midpoint = Polygon.EdgeMidpoint(Vertex1);
            Icon.X = midpoint.Item1 + ICON_DISTANCE;
            Icon.Y = midpoint.Item2 + ICON_DISTANCE;
        }

        public override void DrawIcons(DrawingPlane plane) {
            UpdateIconPosition();
            plane.Draw(Icon);
        }

        public override void ForceConstraint() {
            ForceConstraintWithInvariant(new HashSet<(Shape, int)>());
        }

        public override void ForceConstraintWithInvariant(HashSet<(Shape, int)> invariantIndices) {
            if (Math.Abs(Polygon.EdgeLength(Vertex1) - Length) <= MAX_ABSOLUTE_ERROR) return;

            bool isSecondInvariant = invariantIndices.Contains((Polygon, Vertex2));
            int vertex = isSecondInvariant ? Vertex1 : Vertex2;

            Polygon.MoveVertexToEdgeLength(vertex, Vertex1, Length);

            int prevEdge = Vertex1 > 0 ? Vertex1 - 1 : Polygon.Points.Length - 1;
            int nextEdge = Vertex2;

            if (Polygon.Constraints[isSecondInvariant ? prevEdge : nextEdge] == null) {
                return;
            }

            ForceRecurrentContstraint(
                isSecondInvariant ? prevEdge : nextEdge,
                isSecondInvariant ? Vertex1 : Vertex2
            );
        }

        private void ForceRecurrentContstraint(int edge, int invVert) {
            RecurrentApplicationCount += 1;
            if(RecurrentApplicationCount < RECURRENT_APPLICATION_LIMIT && Polygon.Constraints[edge] != null) {
                Polygon.Constraints[edge] .ForceConstraintWithInvariant(
                    new HashSet<(Shape, int)> { (Polygon, invVert) }
                );
            }
            RecurrentApplicationCount -= 1;
        }

        public override void Neutralize() {
            Polygon.Constraints[Vertex1] = null;
            Polygon = null;
        }

        public override void MoveVertexPolygonForward(Polygon polygon, int edge) {
            if(polygon == Polygon && edge == Vertex1) {
                Vertex1 = (Vertex1 + 1) % Polygon.Points.Length;
                Vertex2 = (Vertex2 + 1) % Polygon.Points.Length;
            }
        }
    }
}
