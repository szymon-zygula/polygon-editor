using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    class ConstantRadiusConstraint : Constraint {
        Circle Circle;
        readonly int R;

        public ConstantRadiusConstraint(Circle circle, int r) {
            R = r;
            Circle = circle;
            Icon = new Icon(
                PrefabIcons.CONST_LENGTH_ICON_COLOR,
                PrefabIcons.ICON_SIZE,
                PrefabIcons.CONST_LENGTH_ICON
            );

            UpdateIconPosition();
        }

        private void UpdateIconPosition() {
            if (Circle == null) return;
            Icon.X = Circle.X + ICON_DISTANCE;
            Icon.Y = Circle.Y + ICON_DISTANCE;
        }

        public override void DrawIcons(DrawingPlane plane) {
            UpdateIconPosition();
            plane.Draw(Icon);
        }

        public override void ForceConstraint() {
            Circle.R = R;
        }

        public override void ForceConstraintWithInvariant(HashSet<(Shape, int)> invariantObjects) {
            ForceConstraint();
        }

        public override void Neutralize() {
            Circle.Constraint = null;
            Circle = null;
        }
    }
}
