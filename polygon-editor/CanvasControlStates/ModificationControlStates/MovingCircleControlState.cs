using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace polygon_editor {
    class MovingCircleControlState : CanvasControlState {
        Circle MovedCircle;
        public MovingCircleControlState(CanvasState state, Circle circle) : base(state) {
            MovedCircle = circle;
        }

        public override void EnterState() {
            MovedCircle.Color = CanvasOptions.CHANGING_LINE_COLOR;
        }

        public override void OnMouseMove(MouseEventArgs e) {
            MovedCircle.X = (int)e.GetPosition(State.Canvas).X;
            MovedCircle.Y = (int)e.GetPosition(State.Canvas).Y;
            MovedCircle.ForceConstraintWithInvariant();
            State.UpdateCanvas();
        }

        public override void OnMouseLeftButtonUp(MouseButtonEventArgs e) {
            MovedCircle.ForceConstraintWithInvariant();
            State.SetControlState(new ActiveCircleControlState(State, MovedCircle));
        }

        public override void ExitState() {
            MovedCircle.Color = CanvasOptions.NORMAL_LINE_COLOR;
        }
    }
}
