using System;
using System.Windows.Input;

namespace polygon_editor {
    public static class CanvasOptions {
        public static readonly UInt32 BACKGROUND_COLOR = 0xFFE0E0E0;
        public static readonly UInt32 NORMAL_LINE_COLOR = 0xFF0000FF;
        public static readonly UInt32 ACTIVE_LINE_COLOR = 0xFF00BB00;
        public static readonly UInt32 CHANGING_LINE_COLOR = 0xFFFF0000;
        public static readonly UInt32 ACTIVE_VERTEX_COLOR = 0xFFFF0000;
        public static readonly UInt32 ACTIVE_EDGE_COLOR = 0xFF0000FF;
        public static readonly UInt32 ACTIVE_CENTER_COLOR = 0xFFFF00FF;

        public static readonly int ACTIVE_VERTEX_RADIUS = 5;
        public static readonly int ACTIVE_EDGE_RADIUS = ACTIVE_VERTEX_RADIUS;

        public static readonly Cursor NORMAL_CURSOR = Cursors.Arrow;
        public static readonly Cursor DRAW_CURSOR = Cursors.Cross;
    }
}
