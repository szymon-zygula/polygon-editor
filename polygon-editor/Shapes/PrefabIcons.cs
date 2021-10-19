using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    static class PrefabIcons {
        private static UInt32 TR_COL = 0x00000000;
        private static UInt32 PU_COL = 0xFF800080;


        public static int ICON_SIZE = 12;

        private static UInt32 CL_COL = PU_COL;
        public static UInt32 CONST_LENGTH_ICON_COLOR = CL_COL;
        public static UInt32[] CONST_LENGTH_ICON = {
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
        };
    }
}
