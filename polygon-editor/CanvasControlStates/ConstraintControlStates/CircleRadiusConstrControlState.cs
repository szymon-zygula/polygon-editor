using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace polygon_editor {
    class CircleRadiusConstrControlState : CanvasControlState {
        public CircleRadiusConstrControlState(CanvasState state) : base(state) {

        }

        public override void OnMouseRightButtonUp(MouseButtonEventArgs e) {
            State.SetControlState(new DoingNothingControlState(State));
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            Circle activeCircle = State.FindAnyCircleCenterWithinMouse(
                e.GetPosition(State.Canvas).X, e.GetPosition(State.Canvas).Y
            );
            if (activeCircle == null) return;
            int newRadius = State.GetConstraintParam();
            if (newRadius == 0) return;
            activeCircle.Constraint = new ConstrantRadiusConstraint(activeCircle, newRadius);
            activeCircle.Constraint.ForceConstraint();
            State.SetControlState(new DoingNothingControlState(State));
        }

        public override void DrawStateFeatures() {
            foreach(Circle circle in State.Circles) {
                State.Plane.MarkCircleCenter(
                    CanvasOptions.ADDING_CONSTRAINT_MARKER_COLOR,
                    CanvasOptions.ACTIVE_EDGE_RADIUS,
                    circle
                );
            }
        }
    }
}
