﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    public class EqualEdgeLengthsConstraint : Constraint {
        Polygon Polygon1;
        int Vertex1_1;
        int Vertex1_2;

        Polygon Polygon2;
        int Vertex2_1;
        int Vertex2_2;

        Icon Icon1;
        Icon Icon2;

        public EqualEdgeLengthsConstraint(Polygon polygon1, int edge1, Polygon polygon2, int edge2) {
            Polygon1 = polygon1;
            Vertex1_1 = edge1;
            Vertex1_2 = edge1 == polygon1.Points.Length - 1 ? 0 : edge1 + 1;

            Polygon2 = polygon2;
            Vertex2_1 = edge2;
            Vertex2_2 = edge2 == polygon2.Points.Length - 1 ? 0 : edge2 + 1;

            Icon1 = new Icon(
                PrefabIcons.EQ_LENGTH_ICON_COLOR,
                PrefabIcons.ICON_SIZE,
                PrefabIcons.EQ_LENGTH_ICON
            );

            Icon2 = new Icon(
                PrefabIcons.EQ_LENGTH_ICON_COLOR,
                PrefabIcons.ICON_SIZE,
                PrefabIcons.EQ_LENGTH_ICON
            );

            UpdateIconsPositions();
        }

        private void UpdateIconsPositions() {
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
            int vertex1 = Vertex2_1;
            int vertex2 = Vertex2_2;
            Polygon polygon = Polygon2;
            int length = Polygon1.EdgeLength(Vertex1_1);
            if(invariantIndices.Contains((Polygon2, Vertex2_1)) || invariantIndices.Contains((Polygon2, Vertex2_2))) {
                vertex1 = Vertex1_1;
                vertex2 = Vertex1_2;
                polygon = Polygon1;
                length = Polygon2.EdgeLength(Vertex2_1);
            }

            if (Math.Abs(polygon.EdgeLength(vertex1) - length) <= MAX_ABSOLUTE_ERROR) return;

            bool isSecondInvariant = invariantIndices.Contains((polygon, vertex2));
            int vertex = isSecondInvariant ? vertex1 : vertex2;

            polygon.MoveVertexToEdgeLength(vertex, vertex1, length);

            int prevEdge = vertex1 > 0 ? vertex1 - 1 : polygon.Points.Length - 1;
            int nextEdge = vertex2;

            if (polygon.Constraints[isSecondInvariant ? prevEdge : nextEdge] == null) {
                return;
            }

            ForceRecurrentContstraint(
                polygon,
                isSecondInvariant ? prevEdge : nextEdge,
                isSecondInvariant ? vertex1 : vertex2
            );
        }

        private void ForceRecurrentContstraint(Polygon poly, int edge, int invVert) {
            RecurrentApplicationCount += 1;
            if(RecurrentApplicationCount < RECURRENT_APPLICATION_LIMIT) {
                poly.Constraints[edge] .ForceConstraintWithInvariant(
                    new HashSet<(Shape, int)> { (poly, invVert) }
                );
            }
            RecurrentApplicationCount -= 1;
        }
    }
}
