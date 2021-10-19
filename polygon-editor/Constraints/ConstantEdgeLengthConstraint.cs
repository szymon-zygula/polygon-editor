using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace polygon_editor {
    class ConstantEdgeLengthConstraint : Constraint {
        Polygon Polygon;
        int Vertex1;
        int Vertex2;
        int Length;

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
            bool isSecondInvariant = invariantIndices.Contains((Polygon, Vertex2));
            int vrt1 = Vertex1;
            int vrt2 = Vertex2;
            if(isSecondInvariant) {
                (vrt1, vrt2) = (vrt2, vrt1);
            }

            (double, double) abVec = (
                Polygon.Points[vrt2].Item1 - Polygon.Points[vrt1].Item1,
                Polygon.Points[vrt2].Item2 - Polygon.Points[vrt1].Item2
            );

            double abVecLen = Math.Sqrt(abVec.Item1 * abVec.Item1 + abVec.Item2 * abVec.Item2);

            if (Math.Abs(abVecLen - Length) <= MAX_ABSOLUTE_ERROR) return;

            (double, double) transVec = (
                abVec.Item1 / abVecLen * Length, abVec.Item2 / abVecLen * Length
            );

            Polygon.Points[vrt2].Item1 = Polygon.Points[vrt1].Item1 + (int)Math.Round(transVec.Item1);
            Polygon.Points[vrt2].Item2 = Polygon.Points[vrt1].Item2 + (int)Math.Round(transVec.Item2);

            int prevEdge = Vertex1 > 0 ? Vertex1 - 1 : Polygon.Points.Length - 1;
            int nextEdge = Vertex2;

            if (Polygon.Constraints[isSecondInvariant ? prevEdge : nextEdge] == null) {
                return;
            }

            RecurrentApplicationCount += 1;
            if(RecurrentApplicationCount < RECURRENT_APPLICATION_LIMIT) {
                Polygon.Constraints[isSecondInvariant ? prevEdge : nextEdge]
                    .ForceConstraintWithInvariant(
                        new HashSet<(Shape, int)> { (Polygon, isSecondInvariant ? Vertex1 : Vertex2) }
                    );
            }
            RecurrentApplicationCount -= 1;
        }
    }
}
