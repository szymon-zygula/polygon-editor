using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    public static class MathUtils {
        public static double VectorLength((double, double) vec) {
            return Math.Sqrt(vec.Item1 * vec.Item1 + vec.Item2 * vec.Item2);
        }

        public static (double, double) MulVector((double, double) vec, double scal) {
            return (
                vec.Item1 * scal, vec.Item2 * scal
            );
        }

        public static void MovePoint(ref (int, int) point, (int, int) newRoot, (double, double) vec) {
            point.Item1 = newRoot.Item1 + (int)Math.Round(vec.Item1);
            point.Item2 = newRoot.Item2 + (int)Math.Round(vec.Item2);
        }
    }
}
