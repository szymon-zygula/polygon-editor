using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    public abstract class Constraint {
        protected static int ICON_DISTANCE = 10;
        protected Icon Icon;
        public abstract void ForceConstraint();
        public abstract void ForceConstraintWithInvariant(Shape invariantShape, int[] invariantIndices);
        public abstract void DrawIcons(DrawingPlane plane);
    }
}
