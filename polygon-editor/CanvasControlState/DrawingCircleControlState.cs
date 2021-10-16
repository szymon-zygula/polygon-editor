using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace polygon_editor {
    class DrawingCircleControlState : CanvasControlState {
        static readonly int CIRCLE_RADIUS_NOT_SET = -1;

        Circle CurrentlyDrawnCircle;
        public DrawingCircleControlState(CanvasState state) : base(state) {
            CurrentlyDrawnCircle = new Circle {
                Color = CanvasOptions.ACTIVE_LINE_COLOR,
                R = CIRCLE_RADIUS_NOT_SET
            };
        }

        public override void EnterState() {
            State.Canvas.Cursor = CanvasOptions.DRAW_CURSOR;
        }

        public override void OnMouseLeftButtonUp(MouseButtonEventArgs e) {
            CurrentlyDrawnCircle.X = (int)e.GetPosition(State.Canvas).X;
            CurrentlyDrawnCircle.Y = (int)e.GetPosition(State.Canvas).Y;
            CurrentlyDrawnCircle.R = 0;
        }

        public override void OnMouseRightButtonUp(MouseButtonEventArgs e) {
            State.SetControlState(new DoingNothingControlState(State));
        }

        public override void OnMouseMove(MouseEventArgs e) {
            if (CurrentlyDrawnCircle.R == CIRCLE_RADIUS_NOT_SET) return;
            int x = (int)e.GetPosition(State.Canvas).X - CurrentlyDrawnCircle.X;
            int y = (int)e.GetPosition(State.Canvas).Y - CurrentlyDrawnCircle.Y;
            CurrentlyDrawnCircle.R = (int)Math.Sqrt(x * x + y * y);
            State.UpdateCanvas();
        }

        public override void DrawStateFeatures() {
            if (CurrentlyDrawnCircle.R == CIRCLE_RADIUS_NOT_SET) return;
            State.Plane.DrawCircle(CurrentlyDrawnCircle);
        }

        public override void ExitState() {
            State.Canvas.Cursor = CanvasOptions.NORMAL_CURSOR;
            if (CurrentlyDrawnCircle.R == CIRCLE_RADIUS_NOT_SET) return;
            CurrentlyDrawnCircle.Color = CanvasOptions.NORMAL_LINE_COLOR;
            State.AddCircle(CurrentlyDrawnCircle);
            State.UpdateCanvas();
        }
    }
}
