using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    public class Circle : Shape {
        public int X { get; set; }
        public int Y { get; set; }
        public int R { get; set; }
        public UInt32 Color { get; set; }
        public Constraint Constraint;

        public Circle() {

        }

        public override string ToString() {
            return $"Circle {Index}";
        }

        public override void DrawOn(DrawingPlane plane) {
            BresenhamDrawer.Circle(plane, Color, R, X, Y);
            if (Constraint != null) Constraint.DrawIcons(plane);
        }

        public void ForceConstraintWithInvariant() {
            if (Constraint != null) Constraint.ForceConstraintWithInvariant(new HashSet<(Shape, int)> { (this, 0) });
        }
    }
}
