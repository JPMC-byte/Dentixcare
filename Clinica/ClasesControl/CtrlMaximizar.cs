using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.ClasesControl
{
    public static class CtrlMaximizar
    {
        public static bool EsMaximizada { get; set; } = false;
        public static Size EscalaOriginal { get; set; } = Size.Empty;
        public static Point PosicionOriginal { get; set; } = Point.Empty;
    }
}
