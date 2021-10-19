using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    class ConstrantRadiusConstraint : Constraint {
        Circle ConstrainedCircle;
        int R;

        public ConstrantRadiusConstraint(Circle circle, int r) {
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

        }

        public override void DrawIcons(DrawingPlane plane) {
            Icon.X = ConstrainedCircle.X + ICON_DISTANCE;
            Icon.Y = ConstrainedCircle.Y + ICON_DISTANCE;
            plane.Draw(Icon);
        }

        public override void ForceConstraint() {
            ConstrainedCircle.R = R;
        }

        public override void ForceConstraintWithInvariant(Shape invariantShape, int[] invariantVertices) {
            ForceConstraint();
        }
    }
}
