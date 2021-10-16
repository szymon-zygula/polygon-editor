using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace polygon_editor {
    class DrawingPolygonControlState : CanvasControlState {
        Polygon CurrentlyDrawnPolygon;
        public DrawingPolygonControlState(CanvasState state) : base(state) {
            CurrentlyDrawnPolygon = new Polygon {
                Color = CanvasOptions.ACTIVE_LINE_COLOR
            };
        }

        public override void EnterState() {
            State.Canvas.Cursor = CanvasOptions.DRAW_CURSOR;
        }

        public override void OnMouseLeftButtonUp(MouseButtonEventArgs e) {
            int mouseX = (int)e.GetPosition(State.Canvas).X;
            int mouseY = (int)e.GetPosition(State.Canvas).Y;

            if (CurrentlyDrawnPolygon.Points.Length == 0) {
                CurrentlyDrawnPolygon.AddPoint(mouseX, mouseY);
            }

            CurrentlyDrawnPolygon.AddPoint(mouseX, mouseY);
            State.UpdateCanvas();
        }

        public override void OnMouseRightButtonUp(MouseButtonEventArgs e) {
            State.SetControlState(new DoingNothingControlState(State));
        }

        public override void OnMouseMove(MouseEventArgs e) {
            if (CurrentlyDrawnPolygon.Points.Length == 0) return;
            CurrentlyDrawnPolygon.Points[CurrentlyDrawnPolygon.Points.Length - 1] = (
                (int)e.GetPosition(State.Canvas).X, (int)e.GetPosition(State.Canvas).Y
            );
            State.UpdateCanvas();
        }

        public override void DrawStateFeatures() {
            State.Plane.DrawIncompletePolygon(CurrentlyDrawnPolygon);
        }

        public override void ExitState() {
            State.Canvas.Cursor = CanvasOptions.NORMAL_CURSOR;

            if (CurrentlyDrawnPolygon.Points.Length >= 4) {
                CurrentlyDrawnPolygon.Color = CanvasOptions.NORMAL_LINE_COLOR;
                CurrentlyDrawnPolygon.RemoveLastPoint();
                State.AddPolygon(CurrentlyDrawnPolygon);
            }

            State.UpdateCanvas();
        }
    }
}
