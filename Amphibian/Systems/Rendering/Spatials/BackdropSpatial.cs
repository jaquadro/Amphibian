using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.EntitySystem;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Amphibian.Components;

namespace Amphibian.Systems.Rendering.Spatials
{
    public enum HorizontalClamp
    {
        FrameLeft,
        FrameRight,
        FrameBoth,
        WindowLeft,
        WindowRight,
        WindowBoth
    }

    public enum VerticalClamp
    {
        FrameTop,
        FrameBottom,
        FrameBoth,
        WindowTop,
        WindowBottom,
        WindowBoth
    }

    public class BackdropSpatial : Spatial
    {
        #region Property Data

        internal enum DimClamp
        {
            FrameNeg,
            FramePos,
            FrameBoth,
            WindowNeg,
            WindowPos,
            WindowBoth
        }

        #region Clamping Conversions

        private DimClamp HorizontalToDimClamp (HorizontalClamp clamp)
        {
            switch (clamp) {
                case HorizontalClamp.FrameBoth: return DimClamp.FrameBoth;
                case HorizontalClamp.FrameLeft: return DimClamp.FrameNeg;
                case HorizontalClamp.FrameRight: return DimClamp.FramePos;
                case HorizontalClamp.WindowBoth: return DimClamp.WindowBoth;
                case HorizontalClamp.WindowLeft: return DimClamp.WindowNeg;
                case HorizontalClamp.WindowRight: return DimClamp.WindowPos;
                default: return DimClamp.FrameNeg;
            }
        }

        private DimClamp VerticalToDimClamp (VerticalClamp clamp)
        {
            switch (clamp) {
                case VerticalClamp.FrameBoth: return DimClamp.FrameBoth;
                case VerticalClamp.FrameTop: return DimClamp.FrameNeg;
                case VerticalClamp.FrameBottom: return DimClamp.FramePos;
                case VerticalClamp.WindowBoth: return DimClamp.WindowBoth;
                case VerticalClamp.WindowTop: return DimClamp.WindowNeg;
                case VerticalClamp.WindowBottom: return DimClamp.WindowPos;
                default: return DimClamp.FrameNeg;
            }
        }

        private HorizontalClamp DimToHorizontalClamp (DimClamp clamp)
        {
            switch (clamp) {
                case DimClamp.FrameBoth: return HorizontalClamp.FrameBoth;
                case DimClamp.FrameNeg: return HorizontalClamp.FrameLeft;
                case DimClamp.FramePos: return HorizontalClamp.FrameRight;
                case DimClamp.WindowBoth: return HorizontalClamp.WindowBoth;
                case DimClamp.WindowNeg: return HorizontalClamp.WindowLeft;
                case DimClamp.WindowPos: return HorizontalClamp.WindowRight;
                default: return HorizontalClamp.FrameLeft;
            }
        }

        private VerticalClamp DimToVerticalClamp (DimClamp clamp)
        {
            switch (clamp) {
                case DimClamp.FrameBoth: return VerticalClamp.FrameBoth;
                case DimClamp.FrameNeg: return VerticalClamp.FrameTop;
                case DimClamp.FramePos: return VerticalClamp.FrameBottom;
                case DimClamp.WindowBoth: return VerticalClamp.WindowBoth;
                case DimClamp.WindowNeg: return VerticalClamp.WindowTop;
                case DimClamp.WindowPos: return VerticalClamp.WindowBottom;
                default: return VerticalClamp.FrameTop;
            }
        }

        #endregion

        private class DimProperties
        {
            public int OriginalLength;
            public float Scale;

            public int Length
            {
                get { return (int)(OriginalLength * Scale); }
            }

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
                Scale = 1;
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

        #endregion

        private DimProperties _dimX = new DimProperties();
        private DimProperties _dimY = new DimProperties();

        private Texture2D _spriteTexture;

        public BackdropSpatial (EntityWorld world, ContentManager contentManager, string asset)
            : base(world)
        {
            _spriteTexture = contentManager.Load<Texture2D>(asset);
            _dimX.OriginalLength = _spriteTexture.Width;
            _dimY.OriginalLength = _spriteTexture.Height;
        }

        #region Properties

        public bool ScrollX
        {
            get { return _dimX.Scroll; }
            set { _dimX.Scroll = value; }
        }

        public bool ScrollY
        {
            get { return _dimY.Scroll; }
            set { _dimY.Scroll = value; }
        }

        public float ScrollXCoefficient
        {
            get { return _dimX.ScrollCoef; }
            set { _dimX.ScrollCoef = value; }
        }

        public float ScrollYCoefficient
        {
            get { return _dimY.ScrollCoef; }
            set { _dimY.ScrollCoef = value; }
        }

        public bool RepeatX
        {
            get { return _dimX.Repeat; }
            set { _dimX.Repeat = value; }
        }

        public bool RepeatY
        {
            get { return _dimY.Repeat; }
            set { _dimY.Repeat = value; }
        }

        public bool AutoScrollX
        {
            get { return _dimX.AutoScroll; }
            set { _dimX.AutoScroll = value; }
        }

        public bool AutoScrollY
        {
            get { return _dimY.AutoScroll; }
            set { _dimY.AutoScroll = value; }
        }

        public float AutoScrollXSpeed
        {
            get { return _dimX.AutoSpeed; }
            set { _dimX.AutoSpeed = value; }
        }

        public float AutoScrollYSpeed
        {
            get { return _dimY.AutoSpeed; }
            set { _dimY.AutoSpeed = value; }
        }

        public float AutoScrollXPosition
        {
            get { return _dimX.AutoPosition; }
            set { _dimX.AutoPosition = value; }
        }

        public float AutoScrollYPosition
        {
            get { return _dimY.AutoPosition; }
            set { _dimY.AutoPosition = value; }
        }

        public float ScaleX
        {
            get { return _dimX.Scale; }
            set { _dimX.Scale = value; }
        }

        public float ScaleY
        {
            get { return _dimY.Scale; }
            set { _dimY.Scale = value; }
        }

        public HorizontalClamp HorizontalClamp
        {
            get { return DimToHorizontalClamp(_dimX.Clamp); }
            set { _dimX.Clamp = HorizontalToDimClamp(value); }
        }

        public int HorizontalClampOffset
        {
            get { return _dimX.ClampOffset; }
            set { _dimX.ClampOffset = value; }
        }

        public VerticalClamp VerticalClamp
        {
            get { return DimToVerticalClamp(_dimY.Clamp); }
            set { _dimY.Clamp = VerticalToDimClamp(value); }
        }

        public int VerticalClampOffset
        {
            get { return _dimY.ClampOffset; }
            set { _dimY.ClampOffset = value; }
        }

        #endregion

        public override void Update ()
        {
            if (_dimX.AutoScroll) {
                _dimX.AutoPosition += _dimX.AutoSpeed * (float)World.GameTime.ElapsedGameTime.TotalSeconds;
            }
            if (_dimY.AutoScroll) {
                _dimY.AutoPosition += _dimY.AutoSpeed * (float)World.GameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public override void Render (SpriteBatch spriteBatch, Entity entity, Renderable position)
        {
            if (_spriteTexture == null)
                return;

            CameraSystem camera = World.SystemManager.GetSystem(typeof(CameraSystem)) as CameraSystem;
            Rectangle window = (camera != null)
                ? camera.Bounds : World.Frame.Engine.GraphicsDevice.Viewport.Bounds;

            int originX = _dimX.Origin(window.X, window.Width, World.Frame.Width);
            int originY = _dimY.Origin(window.Y, window.Height, World.Frame.Height);

            int tileX = 1;
            int tileY = 1;

            if (_dimX.Repeat) {
                tileX = (window.X + window.Width - originX + _dimX.Length) / _dimX.Length;
            }
            if (_dimY.Repeat) {
                tileY = (window.Y + window.Height - originY + _dimY.Length) / _dimY.Length;
            }

            for (int x = 0; x < tileX; x++) {
                int xStart = originX + (x * _dimX.Length);
                for (int y = 0; y < tileY; y++) {
                    int yStart = originY + (y * _dimY.Length);
                    spriteBatch.Draw(_spriteTexture,
                        new Rectangle(xStart, yStart, _dimX.Length, _dimY.Length),
                        new Rectangle(0, 0, _spriteTexture.Width, _spriteTexture.Height),
                        Color.White);
                }
            }
        }
    }
}
