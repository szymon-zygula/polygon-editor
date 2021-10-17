using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace polygon_editor {
    abstract class TwoSegmentSelectionConstrControlState : CanvasControlState {
        protected int? Edge1;
        protected Polygon Polygon1;
        protected int? Edge2;
        protected Polygon Polygon2;

        public TwoSegmentSelectionConstrControlState(CanvasState state) : base(state) {

        }

        public abstract void OnSelectionComplete();

        public override void OnMouseRightButtonUp(MouseButtonEventArgs e) {
            State.SetControlState(new DoingNothingControlState(State));
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            (int? activeEdge, Polygon activePolygon) = State.FindAnyEdgeWithinMouse(
                e.GetPosition(State.Canvas).X, e.GetPosition(State.Canvas).Y
            );

            if (activeEdge == null || activeEdge == Edge1) return;
            if(!Edge1.HasValue) {
                Edge1 = activeEdge;
                Polygon1 = activePolygon;
                State.UpdateCanvas();
            }
            else {
                Edge2 = activeEdge;
                Polygon2 = activePolygon;
                OnSelectionComplete();
            }
        }

        public override void DrawStateFeatures() {
            foreach(Polygon polygon in State.Polygons) {
                State.Plane.MarkPolygonEdges(
                    CanvasOptions.ADDING_CONSTRAINT_MARKER_COLOR,
                    CanvasOptions.ACTIVE_EDGE_RADIUS,
                    polygon
                );
            }

            if(Edge1 != null) {
                State.Plane.MarkPolygonEdge(
                    CanvasOptions.SET_CONSTRAINT_MARKER_COLOR,
                    CanvasOptions.ACTIVE_EDGE_RADIUS,
                    Polygon1, Edge1.Value);
            }
        }
    }
}
