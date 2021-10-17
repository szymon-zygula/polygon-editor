using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace polygon_editor {
    class SegmentLenConstrControlState : CanvasControlState {
        public SegmentLenConstrControlState(CanvasState state) : base(state) {
            
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            (int? activeEdge, Polygon activePolygon) = State.FindAnyEdgeWithinMouse(
                e.GetPosition(State.Canvas).X, e.GetPosition(State.Canvas).Y
            );

            if (activeEdge == null) return;
            State.SetControlState(new DoingNothingControlState(State));
        }

        public override void OnMouseRightButtonUp(MouseButtonEventArgs e) {
            State.SetControlState(new DoingNothingControlState(State));
        }

        public override void DrawStateFeatures() {
            foreach(Polygon polygon in State.Polygons) {
                State.Plane.MarkPolygonEdges(
                    CanvasOptions.ADDING_CONSTRAINT_MARKER_COLOR,
                    CanvasOptions.ACTIVE_EDGE_RADIUS,
                    polygon
                );
            }
        }
    }
}
