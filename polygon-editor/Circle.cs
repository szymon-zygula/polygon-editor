using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    public class Circle {
        public int X { get; set; }
        public int Y { get; set; }
        public int R { get; set; }
        public UInt32 Color { get; set; }
        public int Index { get; set; }

        public Circle() {

        }

        public override string ToString() {
            return $"Circle {Index}";
        }
    }
}
