using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace polygon_editor {
    class TwoSegmentsEqLenConstrControlState : TwoSegmentSelectionConstrControlState {
        public TwoSegmentsEqLenConstrControlState(CanvasState state) : base(state) {

        }

        public override void OnSelectionComplete() {
            Constraint constraint = 
                new EqualEdgeLengthsConstraint(Polygon1, Edge1.Value, Polygon2, Edge2.Value);
            Polygon1.Constraints[Edge1.Value] = constraint;
            Polygon2.Constraints[Edge2.Value] = constraint;
            constraint.ForceConstraint();
            State.SetControlState(new DoingNothingControlState(State));
        }
    }
}
