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
            MessageBox.Show("segs set eq len");
            State.SetControlState(new DoingNothingControlState(State));
        }
    }
}
