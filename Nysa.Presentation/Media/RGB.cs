using System;
using Win = System.Windows.Media;

namespace Nysa.Windows.Media
{

    public struct RGB
    {
        public Byte Red     { get; set; }
        public Byte Green   { get; set; }
        public Byte Blue    { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:ColorRGB"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public RGB(Win.Color value)
            : this()
        {
            this.Red   = value.R;
            this.Green = value.G;
            this.Blue  = value.B;
        }


        public RGB(Byte red, Byte green, Byte blue)
            : this()
        {
            this.Red    = red;
            this.Green  = green;
            this.Blue   = blue;
        }

        /// <summary>
        /// Implicit conversion of the specified RGB.
        /// </summary>
        /// <param name="rgb">The RGB.</param>
        /// <returns></returns>
        public static implicit operator Win.Color(RGB rgb)
        {
            Win.Color c = Win.Color.FromArgb(255, rgb.Red, rgb.Green, rgb.Blue);
            return c;
        }

        /// <summary>
        /// Explicit conversion of the specified c.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        public static explicit operator RGB(Win.Color c)
            => new RGB(c);

        public HSL ToHsl()
        {
            const Int32 red     = 0;
            const Int32 green   = 1;
            const Int32 blue    = 2;

            Double[] rgb = new Double[]
                            {   Convert.ToDouble(this.Red)   / 255.0, 
                                Convert.ToDouble(this.Green) / 255.0, 
                                Convert.ToDouble(this.Blue)  / 255.0
                            };

            Int32 maxIdx = 0;
            Int32 minIdx = 0;

            for (Int32 i = red; i < blue; i++)
            {
                maxIdx = (rgb[maxIdx] < rgb[i + 1]) ? i + 1 : maxIdx;
                minIdx = (rgb[minIdx] > rgb[i + 1]) ? i + 1 : minIdx;
            }

            Double l = (rgb[maxIdx] + rgb[minIdx]) / 2.0;
            Double c = (rgb[maxIdx] - rgb[minIdx]);
            Double h = 0.0;
            Double s =   (l == 0.0 || l == 1) ? 0.0
                       : (l <= 0.5)           ? c / (2.0 * l)
                       :                        c / (2.0 - (2.0 * l));

            if (l > 0.0 && c > 0.0)
            {
                if      (maxIdx == red)     h = (((rgb[green] - rgb[blue] ) / c) + (rgb[green] < rgb[blue] ? 6.0 : 0.0));
                else if (maxIdx == green)   h = (((rgb[blue]  - rgb[red]  ) / c) + 2.0);
                else                        h = (((rgb[red]   - rgb[green]) / c) + 4.0);

                h /= 6.0;
            }

            return new HSL(Convert.ToInt16(h * 360.0), s, l);
        }

    }

}
