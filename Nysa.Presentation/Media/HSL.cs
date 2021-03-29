using System;
using Win = System.Windows.Media;

namespace Nysa.Windows.Media
{

    public struct HSL
    {
        public Int16    Hue         { get; set; }           // 0 to 360 degrees { Red = 0, Green = 120, Blue = 260, Red = 360 }
        public Double   Saturation  { get; set; }           // 0 to 1
        public Double   Lightness   { get; set; }           // 0 to 1           { Black = 0, White = 1 }

        public HSL(Int16 hue, Double saturation, Double lightness)
            : this()
        {
            this.Hue        = hue;
            this.Saturation = saturation;
            this.Lightness  = lightness;
        }

        public RGB ToRgb()
        {
            const Int32 red     = 0;
            const Int32 green   = 1;
            const Int32 blue    = 2;

            Double[] rgb = new Double[]
                            {   this.Lightness, 
                                this.Lightness, 
                                this.Lightness
                            };

            if (this.Saturation != 0.0)
            {
                Double c = ((1.0 - Math.Abs((2.0 * this.Lightness) - 1.0)) * this.Saturation);
                Double p = this.Hue / 60.0;
                Double x = c * (1.0 - Math.Abs((p % 2.0) - 1.0));

                if      (0.0 <= p && p < 1.0) rgb = new Double[] { c, x, 0.0 };
                else if (1.0 <= p && p < 2.0) rgb = new Double[] { x, c, 0.0 };
                else if (2.0 <= p && p < 3.0) rgb = new Double[] { 0.0, c, x };
                else if (3.0 <= p && p < 4.0) rgb = new Double[] { 0.0, x, c };
                else if (4.0 <= p && p < 5.0) rgb = new Double[] { x, 0.0, c };
                else if (5.0 <= p && p < 6.0) rgb = new Double[] { c, 0.0, x };
                else                          rgb = new Double[] { 0.0, 0.0, 0.0 };

                Double m = this.Lightness - (c / 2.0);

                rgb[red]    += m;
                rgb[green]  += m;
                rgb[blue]   += m;
            }

            return new RGB(Convert.ToByte(rgb[0] * 255.0), Convert.ToByte(rgb[1] * 255.0), Convert.ToByte(rgb[2] * 255.0));
        }

    }

}
