using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    class ConstantRadiusConstraint : Constraint {
        Circle ConstrainedCircle;
        int R;

        public ConstantRadiusConstraint(Circle circle, int r) {
            R = r;
            ConstrainedCircle = circle;
            Icon = new Icon(
                PrefabIcons.CONST_LENGTH_ICON_COLOR,
                PrefabIcons.ICON_SIZE,
                PrefabIcons.CONST_LENGTH_ICON
            );

            UpdateIconPosition();
        }

        private void UpdateIconPosition() {
            Icon.X = ConstrainedCircle.X + ICON_DISTANCE;
            Icon.Y = ConstrainedCircle.Y + ICON_DISTANCE;
        }

        public override void DrawIcons(DrawingPlane plane) {
            UpdateIconPosition();
            plane.Draw(Icon);
        }

        public override void ForceConstraint() {
            ConstrainedCircle.R = R;
        }

        public override void ForceConstraintWithInvariant(HashSet<(Shape, int)> invariantObjects) {
            ForceConstraint();
        }
    }
}
