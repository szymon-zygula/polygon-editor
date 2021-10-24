using System;
using System.Windows.Input;

namespace polygon_editor {
    public class RemoveConstraintControlState : CanvasControlState {

        public RemoveConstraintControlState(CanvasState state) : base(state) {

        }

        public override void EnterState() {
            State.Canvas.Cursor = CanvasOptions.REMOVE_CURSOR;
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            double x = e.GetPosition(State.Canvas).X;
            double y = e.GetPosition(State.Canvas).Y;
            foreach(Polygon polygon in State.Polygons) {
                for(int i = 0; i < polygon.Points.Length; ++i) {
                    TryRemovePolygonConstraint(polygon, i, x, y);
                }
            }

            foreach(Circle circle in State.Circles) {
                int distX = (int)Math.Abs(circle.X + Constraint.ICON_DISTANCE + PrefabIcons.ICON_SIZE / 2 - x);
                int distY = (int)Math.Abs(circle.Y + Constraint.ICON_DISTANCE + PrefabIcons.ICON_SIZE / 2 - y);
                if(circle.Constraint != null && distX <= PrefabIcons.ICON_SIZE / 2 && distY <= PrefabIcons.ICON_SIZE) {
                    circle.Constraint.Neutralize();
                    State.SetControlState(new DoingNothingControlState(State));
                    return;
                }
            }
        }

        private void TryRemovePolygonConstraint(Polygon polygon, int edge, double x, double y) {
            if(polygon.Constraints[edge] != null) {
                (int, int) midpoint = polygon.EdgeMidpoint(edge);
                int distX = (int)Math.Abs(midpoint.Item1 + Constraint.ICON_DISTANCE + PrefabIcons.ICON_SIZE / 2 - x);
                int distY = (int)Math.Abs(midpoint.Item2 + Constraint.ICON_DISTANCE + PrefabIcons.ICON_SIZE / 2 - y);
                if(distX <= PrefabIcons.ICON_SIZE / 2 && distY <= PrefabIcons.ICON_SIZE) {
                    polygon.Constraints[edge].Neutralize();
                    State.SetControlState(new DoingNothingControlState(State));
                    return;
                }
            }
        }

        public override void OnMouseRightButtonUp(MouseButtonEventArgs e) {
            State.SetControlState(new DoingNothingControlState(State));
        }

        public override void ExitState() {
            State.Canvas.Cursor = CanvasOptions.NORMAL_CURSOR;
        }
    }
}
