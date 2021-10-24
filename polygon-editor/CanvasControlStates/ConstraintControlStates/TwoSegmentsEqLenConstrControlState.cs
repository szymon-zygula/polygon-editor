namespace polygon_editor {
    class TwoSegmentsEqLenConstrControlState : TwoSegmentSelectionConstrControlState {
        public TwoSegmentsEqLenConstrControlState(CanvasState state) : base(state) {

        }

        public override void OnSelectionComplete() {
            Constraint constraint = 
                new EqualEdgeLengthsConstraint(Polygon1, Edge1.Value, Polygon2, Edge2.Value);
            if (Polygon1.Constraints[Edge1.Value] != null) Polygon1.Constraints[Edge1.Value].Neutralize();
            if (Polygon2.Constraints[Edge2.Value] != null) Polygon2.Constraints[Edge2.Value].Neutralize();
            Polygon1.Constraints[Edge1.Value] = constraint;
            Polygon2.Constraints[Edge2.Value] = constraint;
            constraint.ForceConstraint();
            State.SetControlState(new DoingNothingControlState(State));
        }
    }
}
