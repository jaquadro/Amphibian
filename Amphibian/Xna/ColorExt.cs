using System;
using Microsoft.Xna.Framework;

namespace Amphibian.Xna
{
    public static class ColorExt
    {
        public static string ToRgbHex (this Color color)
        {
            return "#" + ByteToHex2(color.R) + ByteToHex2(color.G) + ByteToHex2(color.B);
        }

        public static string ToArgbHex (this Color color)
        {
            return "#" + ByteToHex2(color.A) + ByteToHex2(color.R) + ByteToHex2(color.G) + ByteToHex2(color.B);
        }

        public static Color ParseRgbHex (string hex)
        {
            if ((hex.Length != 6 && hex.Length != 7) ||
                (hex.Length == 7 && hex[0] != '#'))
                throw new ArgumentException("Invalid ARGB hex string.", "hex");

            if (hex[0] == '#')
                hex = hex.Substring(1);

            try {
                byte r = Convert.ToByte(hex.Substring(2, 2), 16);
                byte g = Convert.ToByte(hex.Substring(4, 2), 16);
                byte b = Convert.ToByte(hex.Substring(6, 2), 16);

                return new Color()
                {
                    A = 255,
                    R = r,
                    G = g,
                    B = b
                };
            }
            catch (FormatException) {
                throw new ArgumentException("Invalid ARGB hex string.", "hex");
            }
        }

        public static Color ParseArgbHex (string hex)
        {
            if ((hex.Length != 8 && hex.Length != 9) ||
                (hex.Length == 9 && hex[0] != '#'))
                throw new ArgumentException("Invalid ARGB hex string.", "hex");

            if (hex[0] == '#')
                hex = hex.Substring(1);

            try {
                byte a = Convert.ToByte(hex.Substring(0, 2), 16);
                byte r = Convert.ToByte(hex.Substring(2, 2), 16);
                byte g = Convert.ToByte(hex.Substring(4, 2), 16);
                byte b = Convert.ToByte(hex.Substring(6, 2), 16);

                return new Color()
                {
                    A = a,
                    R = r,
                    G = g,
                    B = b
                };
            }
            catch (FormatException) {
                throw new ArgumentException("Invalid ARGB hex string.", "hex");
            }
        }

        private static string ByteToHex2 (byte value)
        {
            String hex = Convert.ToString(value, 16);
            if (hex.Length >= 2)
                return hex;
            else
                return "0" + hex;
        }
    }
}
