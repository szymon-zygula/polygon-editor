using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    public abstract class Constraint {
        public static int ICON_DISTANCE = 5;
        protected Icon Icon;

        protected const int RECURRENT_APPLICATION_LIMIT = 25;
        protected int RecurrentApplicationCount = 0;

        protected const int MAX_ABSOLUTE_ERROR = 2;

        public abstract void ForceConstraint();
        public abstract void ForceConstraintWithInvariant(HashSet<(Shape, int)> invariantIndices);
        public abstract void DrawIcons(DrawingPlane plane);
        public abstract void Neutralize();
    }
}
