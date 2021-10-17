using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace polygon_editor {
    class ResizingCircleControlState : CanvasControlState {
        Circle ResizedCircle;
        public ResizingCircleControlState(CanvasState state, Circle circle) : base(state) {
            ResizedCircle = circle;
        }

        public override void EnterState() {
            ResizedCircle.Color = CanvasOptions.CHANGING_LINE_COLOR;
        }

        public override void OnMouseMove(MouseEventArgs e) {
            ResizedCircle.R = Math.Abs(ResizedCircle.Y - (int)e.GetPosition(State.Canvas).Y);
            State.UpdateCanvas();
        }

        public override void OnMouseLeftButtonUp(MouseButtonEventArgs e) {
            State.SetControlState(new ActiveCircleControlState(State, ResizedCircle));
        }

        public override void ExitState() {
            ResizedCircle.Color = CanvasOptions.NORMAL_LINE_COLOR;
        }
    }
}
