using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPL_Component1
{
    class ColorProvider
    {

        public Pen pickPen(string color, int size)
        {
            ColorFactory cf = new ColorFactory();
            Color getColor = cf.GetColor(color);
            return new Pen(getColor, size);
        }
    }
}
