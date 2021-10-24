using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace polygon_editor {
    class TangentSegCirConstrControlState : CanvasControlState {
        Polygon Polygon;
        int? Edge;

        public TangentSegCirConstrControlState(CanvasState state) : base(state) {

        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            if(!Edge.HasValue) {
                (int? activeEdge, Polygon activePolygon) = State.FindAnyEdgeWithinMouse(
                    e.GetPosition(State.Canvas).X, e.GetPosition(State.Canvas).Y
                );

                if (activeEdge == null) return;

                Edge = activeEdge;
                Polygon = activePolygon;
                State.UpdateCanvas();
            }
            else {
                Circle activeCircle = State.FindAnyCircleCenterWithinMouse(
                    e.GetPosition(State.Canvas).X, e.GetPosition(State.Canvas).Y
                );
                if (activeCircle == null) return;
                Constraint constraint = new TangentCircleConstraint(activeCircle, Polygon, Edge.Value);
                if (Polygon.Constraints[Edge.Value] != null) Polygon.Constraints[Edge.Value].Neutralize();
                Polygon.Constraints[Edge.Value] = constraint;
                if (activeCircle.Constraint != null) activeCircle.Constraint.Neutralize();
                activeCircle.Constraint = constraint;
                constraint.ForceConstraint();
                State.SetControlState(new DoingNothingControlState(State));
            }
        }

        public override void OnMouseRightButtonUp(MouseButtonEventArgs e) {
            State.SetControlState(new DoingNothingControlState(State));
        }

        public override void DrawStateFeatures() {
            if (Edge == null) {
                foreach (Polygon polygon in State.Polygons) {
                    State.Plane.MarkPolygonEdges(
                        CanvasOptions.ADDING_CONSTRAINT_MARKER_COLOR,
                        CanvasOptions.ACTIVE_EDGE_RADIUS,
                        polygon
                    );
                }
            }
            else {
                foreach (Circle circle in State.Circles) {
                    State.Plane.MarkCircleCenter(
                        CanvasOptions.ADDING_CONSTRAINT_MARKER_COLOR,
                        CanvasOptions.ACTIVE_EDGE_RADIUS,
                        circle
                    );
                }

                State.Plane.MarkPolygonEdge(
                    CanvasOptions.SET_CONSTRAINT_MARKER_COLOR,
                    CanvasOptions.ACTIVE_EDGE_RADIUS,
                    Polygon, Edge.Value
                );
            }
        }
    }
}
