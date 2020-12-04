using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace GPL_Component1
{
    class ColorFactory
    {
        public Color GetColor(string name)
        {
            string checkColor = name.ToLower().Trim();
            switch (checkColor)
            {
                case "red":
                    return Color.Red;
                case "green":
                    return Color.Green;
                case "yellow":
                    return Color.Yellow;
                case "orange":
                    return Color.Orange;
                case "black":
                    return Color.Black;
                default:
                    return Color.White;
            }

        }
    }
}
