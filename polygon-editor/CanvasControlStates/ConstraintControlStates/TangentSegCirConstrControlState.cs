using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace polygon_editor {
    class TangentSegCirConstrControlState : CanvasControlState {
        Polygon Polygon1;
        int? Edge1;

        public TangentSegCirConstrControlState(CanvasState state) : base(state) {

        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            if(!Edge1.HasValue) {
                (int? activeEdge, Polygon activePolygon) = State.FindAnyEdgeWithinMouse(
                    e.GetPosition(State.Canvas).X, e.GetPosition(State.Canvas).Y
                );

                if (activeEdge == null) return;

                Edge1 = activeEdge;
                Polygon1 = activePolygon;
                State.UpdateCanvas();
            }
            else {
                Circle activeCircle = State.FindAnyCircleCenterWithinMouse(
                    e.GetPosition(State.Canvas).X, e.GetPosition(State.Canvas).Y
                );
                if (activeCircle == null) return;
                State.SetControlState(new DoingNothingControlState(State));
            }
        }

        public override void OnMouseRightButtonUp(MouseButtonEventArgs e) {
            State.SetControlState(new DoingNothingControlState(State));
        }

        public override void DrawStateFeatures() {
            if (Edge1 == null) {
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
                    Polygon1, Edge1.Value
                );
            }
        }
    }
}
