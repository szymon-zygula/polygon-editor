using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace polygon_editor {
    public class ActiveCircleControlState : CanvasControlState {
        Circle ActiveCircle;
        public ActiveCircleControlState(CanvasState state, Circle circle) : base(state) {
            ActiveCircle = circle;
        }

        public override void EnterState() {
            ActiveCircle.Color = CanvasOptions.ACTIVE_LINE_COLOR;
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            double mouseX = e.GetPosition(State.Canvas).X;
            double mouseY = e.GetPosition(State.Canvas).Y;
            if (TryEnterMovingCircleState(mouseX, mouseY)) return;
            if (TryEnterResizingCircleState(mouseX, mouseY)) return;
        }

        private bool TryEnterMovingCircleState(double mouseX, double mouseY) {
            if (!State.IsWithinCircleCenterMouse(ActiveCircle, mouseX, mouseY)) return false;
            State.SetControlState(new MovingCircleControlState(State, ActiveCircle));
            return true;
        }
        
        private bool TryEnterResizingCircleState(double mouseX, double mouseY) {
            double x = ActiveCircle.X - mouseX;
            double y = ActiveCircle.Y - ActiveCircle.R - mouseY;
            if (Math.Sqrt(x * x + y * y) > CanvasOptions.ACTIVE_VERTEX_RADIUS) return false;
            State.SetControlState(new ResizingCircleControlState(State, ActiveCircle));
            return true;
        }

        public override void OnMouseRightButtonUp(MouseButtonEventArgs e) {
            double mouseX = e.GetPosition(State.Canvas).X;
            double mouseY = e.GetPosition(State.Canvas).Y;
            TryRemoveActiveCircle(mouseX, mouseY);
        }

        private bool TryRemoveActiveCircle(double mouseX, double mouseY) {
            bool isWithinXRange = Math.Abs(ActiveCircle.X - mouseX) <= CanvasOptions.ACTIVE_VERTEX_RADIUS;
            bool isWithinYRange = Math.Abs(ActiveCircle.Y - mouseY) <= CanvasOptions.ACTIVE_VERTEX_RADIUS;
            if (!isWithinXRange || !isWithinYRange) return false;
            RemoveActiveCircle();
            return true;
        }

        private void RemoveActiveCircle() {
            State.Circles.Remove(ActiveCircle);
            State.ShapeList.Items.Remove(ActiveCircle);
            State.SetControlState(new DoingNothingControlState(State));
            State.UpdateCanvas();
        }

        public override void DrawStateFeatures() {
            State.Plane.MarkCircleCenter(
                CanvasOptions.ACTIVE_CENTER_COLOR,
                CanvasOptions.ACTIVE_VERTEX_RADIUS,
                ActiveCircle
            );

            State.Plane.MarkCircleTop(
                CanvasOptions.ACTIVE_EDGE_COLOR,
                CanvasOptions.ACTIVE_VERTEX_RADIUS,
                ActiveCircle
            );
        }

        public override void ExitState() {
            ActiveCircle.Color = CanvasOptions.NORMAL_LINE_COLOR;
        }
    }
}
