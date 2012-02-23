using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Amphibian
{
    enum HorizontalClamp
    {
        FrameLeft,
        FrameRight,
        FrameBoth,
        WindowLeft,
        WindowRight,
        WindowBoth
    }

    enum VerticalClamp
    {
        FrameTop,
        FrameBottom,
        FrameBoth,
        WindowTop,
        WindowBottom,
        WindowBoth
    }

    


    public class Parallax
    {
        internal enum DimClamp
        {
            FrameNeg,
            FramePos,
            FrameBoth,
            WindowNeg,
            WindowPos,
            WindowBoth
        }

        private class DimProperties
        {
            public int Length;

            public bool Scroll;
            public bool Repeat;
            public bool AutoScroll;

            public float ScrollCoef = 1.0f;
            public float AutoSpeed = 0.0f;
            public float AutoPosition = 0.0f;

            public DimClamp Clamp;
            public int ClampOffset;

            public DimProperties ()
            {
                Clamp = DimClamp.FrameNeg;
                ClampOffset = 0;
            }

            public int Origin (int winPos, int winLength, int frameLength)
            {
                if (Scroll) {
                    if (Repeat) {
                        switch (Clamp) {
                            case DimClamp.FrameNeg:
                                return winPos - (((int)(winPos * ScrollCoef) + ClampOffset + (int)AutoPosition) % Length);
                            case DimClamp.FramePos:
                                // Don't really care if this is correct or not...
                                return (winPos + winLength) - ((winLength + ((frameLength - (winPos + winLength) - ClampOffset + (int)AutoPosition) % Length) + Length - 1) / Length);
                            case DimClamp.WindowNeg:
                                return winPos - (Length - (ClampOffset % Length));
                            case DimClamp.WindowPos:
                                return (winPos + winLength) - Length - ClampOffset;
                            default: return 0;
                        }
                    }
                    else {
                        switch (Clamp) {
                            default:
                                return winPos - (int)((1.0 - ((frameLength - winLength) - winPos) / (float)(frameLength - winLength)) * (float)(Length - winLength));
                        }
                    }
                }
                else {
                    switch (Clamp) {
                        case DimClamp.FrameNeg:
                        case DimClamp.FrameBoth:
                            return ClampOffset;
                        case DimClamp.FramePos:
                            return frameLength - Length - ClampOffset;
                        default: return 0;
                    }
                }
            }
        }

        DimProperties dimX = new DimProperties();
        DimProperties dimY = new DimProperties();

        Texture2D _spriteTexture;

        #region Properties

        public bool ScrollX
        {
            get { return dimX.Scroll; }
            set { dimX.Scroll = value; }
        }

        public bool ScrollY
        {
            get { return dimY.Scroll; }
            set { dimY.Scroll = value; }
        }

        public bool RepeatX
        {
            get { return dimX.Repeat; }
            set { dimX.Repeat = value; }
        }

        public bool RepeatY
        {
            get { return dimY.Repeat; }
            set { dimY.Repeat = value; }
        }

        public bool AutoScrollX
        {
            get { return dimX.AutoScroll; }
            set { dimX.AutoScroll = value; }
        }

        public bool AutoScrollY
        {
            get { return dimY.AutoScroll; }
            set { dimY.AutoScroll = value; }
        }

        #endregion

        public void LoadContent (ContentManager contentManager, string assetName)
        {
            _spriteTexture = contentManager.Load<Texture2D>(assetName);

            dimX.Length = _spriteTexture.Width * 2;
            dimY.Length = _spriteTexture.Height * 2;

            dimX.Scroll = true;
            dimY.Scroll = true;
            //dimX.Repeat = true;
            //dimX.AutoScroll = true;
            //dimX.AutoSpeed = 120f;
        }

        public void Update (GameTime gameTime)
        {
            if (dimX.AutoScroll) {
                dimX.AutoPosition += dimX.AutoSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (dimY.AutoScroll) {
                dimY.AutoPosition += dimY.AutoSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Draw (SpriteBatch spriteBatch, Frame frame, Rectangle window)
        {
            int originX = dimX.Origin(window.X, window.Width, frame.Width);
            int originY = dimY.Origin(window.Y, window.Height, frame.Height);

            int tileX = 1;
            int tileY = 1;

            if (dimX.Repeat) {
                tileX = (window.X + window.Width - originX + dimX.Length) / dimX.Length;
            }
            if (dimY.Repeat) {
                tileY = (window.Y + window.Height - originY + dimY.Length) / dimY.Length;
            }

            for (int x = 0; x < tileX; x++) {
                int xStart = originX + (x * dimX.Length);
                for (int y = 0; y < tileY; y++) {
                    int yStart = originY + (y * dimY.Length);
                    spriteBatch.Draw(_spriteTexture,
                        new Rectangle(xStart, yStart, dimX.Length, dimY.Length),
                        new Rectangle(0, 0, _spriteTexture.Width, _spriteTexture.Height),
                        Color.White);
                }
            }
        }
    }
}
