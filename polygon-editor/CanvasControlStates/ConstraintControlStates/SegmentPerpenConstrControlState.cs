using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace polygon_editor {
    class SegmentPerpenConstrControlState : TwoSegmentSelectionConstrControlState {

        public SegmentPerpenConstrControlState(CanvasState state) : base(state) {

        }

        public override void OnSelectionComplete() {
            MessageBox.Show("segs set perpen");
            State.SetControlState(new DoingNothingControlState(State));
        }
    }
}
