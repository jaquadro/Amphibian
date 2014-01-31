using System.Collections.Generic;
using Amphibian.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Systems.Rendering.Sprites
{
    public class CounterDefinition
    {
        public enum HAlignment
        {
            Left,
            Center,
            Right,
        }

        public enum VAlignment
        {
            Top,
            Center,
            Bottom,
        }

        public static readonly Dictionary<string, int> DigitIndex = new Dictionary<string, int>() {
            { "0", 0 }, { "1", 1 }, { "2", 2 }, { "3", 3 }, { "4", 4 },
            { "5", 5 }, { "6", 6 }, { "7", 7 }, { "8", 8 }, { "9", 9 },
            { "+", 10 }, { "-", 11 }, { ".", 12 },
        };

        private StaticSpriteDefinition[] _digitFrames;

        public CounterDefinition ()
        {
            _digitFrames = new StaticSpriteDefinition[13];
        }

        public Counter CreateCounter ()
        {
            return new Counter(this);
        }

        public StaticSpriteDefinition[] DigitFrames
        {
            get { return _digitFrames; }
        }

        public FPInt DigitSpacing { get; set; }

        public bool LeadingPlus { get; set; }

        public HAlignment HAlign { get; set; }
        public VAlignment VAlign { get; set; }

        public void Draw (SpriteBatch spriteBatch, PointFP position, SpriteInfo spriteData, List<int> digitIndexList)
        {
            FPInt width = -DigitSpacing;
            FPInt height = 0;
            foreach (int digitIndex in digitIndexList) {
                width += (FPInt)(_digitFrames[digitIndex].Width * spriteData.Scale) + DigitSpacing;
                height = FPMath.Max(height, (FPInt)(_digitFrames[digitIndex].Height * spriteData.Scale));
            }

            FPInt x = position.X - width / 2;
            FPInt y = position.Y - height / 2;
            foreach (int digitIndex in digitIndexList) {
                _digitFrames[digitIndex].Draw(spriteBatch, new PointFP(x, y), spriteData);
                x += (FPInt)(_digitFrames[digitIndex].Width * spriteData.Scale) + DigitSpacing;
            }
        }

        internal void ParseValue (int value, List<int> digitIndexList)
        {
            digitIndexList.Clear();
            if (value < 0)
                digitIndexList.Add(DigitIndex["-"]);
            else if (LeadingPlus)
                digitIndexList.Add(DigitIndex["+"]);

            if (value == 0) {
                digitIndexList.Add(0);
                return;
            }

            int start = digitIndexList.Count;
            while (value > 0) {
                int digit = value % 10;
                value /= 10;

                digitIndexList.Add(digit);
            }

            digitIndexList.Reverse(start, digitIndexList.Count - start);
        }

        internal PointFP CalculateSize (List<int> digitList, SpriteInfo spriteInfo)
        {
            FPInt width = 0;
            FPInt height = 0;

            foreach (int digitIndex in digitList) {
                width += (FPInt)(_digitFrames[digitIndex].Width * spriteInfo.Scale);
                height = (FPInt)FPMath.Max(height, (FPInt)(_digitFrames[digitIndex].Height * spriteInfo.Scale));
            }

            if (digitList.Count > 1)
                width += DigitSpacing * (digitList.Count - 1);

            return new PointFP(width, height);
        }
    }
}
