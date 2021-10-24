using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_editor {
    static class PrefabIcons {
        private static readonly UInt32 TR_COL = 0x00000000;
        private static readonly UInt32 PU_COL = 0xFF800080;
        private static readonly UInt32 CY_COL = 0xFF00FFFF;
        private static readonly UInt32 OR_COL = 0xFFFFA500;
        private static readonly UInt32 LI_COL = 0xFFBFFF00;

        public static readonly int ICON_SIZE = 12;

        private static readonly UInt32 CL_COL = PU_COL;
        public static readonly UInt32 CONST_LENGTH_ICON_COLOR = CL_COL;
        public static readonly UInt32[] CONST_LENGTH_ICON = {
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, PU_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, PU_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, PU_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, PU_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, PU_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
        };

        private static readonly UInt32 EL_COL = CY_COL;
        public static readonly UInt32 EQ_LENGTH_ICON_COLOR = EL_COL;
        public static readonly UInt32[] EQ_LENGTH_ICON = {
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, EL_COL, EL_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, EL_COL, EL_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, EL_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
        };

        private static readonly UInt32 TC_COL = OR_COL;
        public static readonly UInt32 TANGENT_CIRCLE_ICON_COLOR = TC_COL;
        public static readonly UInt32[] TANGENT_CIRCLE_ICON = {
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TC_COL, TC_COL, TC_COL, TC_COL, TC_COL, TC_COL, TC_COL, TC_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TC_COL, TC_COL, TC_COL, TC_COL, TC_COL, TC_COL, TC_COL, TC_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TC_COL, TC_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TC_COL, TC_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TC_COL, TC_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TC_COL, TC_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TC_COL, TC_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TC_COL, TC_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
        };

        private static readonly UInt32 PP_COL = LI_COL;
        public static readonly UInt32 PERPENDICULAR_ICON_COLOR = PP_COL;
        public static readonly UInt32[] PERPENDICULAR_ICON = {
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PP_COL, PP_COL, PP_COL, PP_COL, PP_COL, PP_COL, PP_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PP_COL, PP_COL, PP_COL, PP_COL, PP_COL, PP_COL, PP_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PP_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, PP_COL, PP_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PP_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, PP_COL, PP_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PP_COL, PP_COL, PP_COL, PP_COL, PP_COL, PP_COL, PP_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PP_COL, PP_COL, PP_COL, PP_COL, PP_COL, PP_COL, PP_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PP_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, PP_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
            TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL, TR_COL,
        };
    }
}
