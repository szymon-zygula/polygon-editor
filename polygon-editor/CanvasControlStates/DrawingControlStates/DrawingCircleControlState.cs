using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace polygon_editor {
    class DrawingCircleControlState : CanvasControlState {
        static readonly int CIRCLE_RADIUS_NOT_SET = -1;

        Circle DrawnCircle;
        public DrawingCircleControlState(CanvasState state) : base(state) {
            DrawnCircle = new Circle {
                Color = CanvasOptions.ACTIVE_LINE_COLOR,
                R = CIRCLE_RADIUS_NOT_SET
            };
        }

        public override void EnterState() {
            State.Canvas.Cursor = CanvasOptions.DRAW_CURSOR;
        }

        public override void OnMouseLeftButtonUp(MouseButtonEventArgs e) {
            DrawnCircle.X = (int)e.GetPosition(State.Canvas).X;
            DrawnCircle.Y = (int)e.GetPosition(State.Canvas).Y;
            DrawnCircle.R = 0;
        }

        public override void OnMouseRightButtonUp(MouseButtonEventArgs e) {
            State.SetControlState(new DoingNothingControlState(State));
        }

        public override void OnMouseMove(MouseEventArgs e) {
            if (DrawnCircle.R == CIRCLE_RADIUS_NOT_SET) return;
            int x = (int)e.GetPosition(State.Canvas).X - DrawnCircle.X;
            int y = (int)e.GetPosition(State.Canvas).Y - DrawnCircle.Y;
            DrawnCircle.R = (int)Math.Sqrt(x * x + y * y);
            State.UpdateCanvas();
        }

        public override void DrawStateFeatures() {
            if (DrawnCircle.R == CIRCLE_RADIUS_NOT_SET) return;
            State.Plane.Draw(DrawnCircle);
        }

        public override void ExitState() {
            State.Canvas.Cursor = CanvasOptions.NORMAL_CURSOR;
            if (DrawnCircle.R == CIRCLE_RADIUS_NOT_SET) return;
            DrawnCircle.Color = CanvasOptions.NORMAL_LINE_COLOR;
            State.AddCircle(DrawnCircle);
            State.UpdateCanvas();
        }
    }
}
