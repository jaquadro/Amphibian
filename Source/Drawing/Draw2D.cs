using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Drawing
{
    public static partial class Draw2D
    {
    }

    // FillRectangle
    public static partial class Draw2D
    {
        public static void FillRectangle (SpriteBatch spriteBatch, Rectangle rect, Brush brush)
        {
            spriteBatch.Draw(brush.Texture, rect, Color.White * brush.Alpha);
        }

        public static void FillRectangle (SpriteBatch spriteBatch, Rectangle rect, Brush brush, float angle)
        {
            spriteBatch.Draw(brush.Texture, rect, null, Color.White * brush.Alpha, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public static void FillRectangle (SpriteBatch spriteBatch, Vector2 location, Vector2 size, Brush brush)
        {
            FillRectangle(spriteBatch, location, size, brush, 0f);
        }

        public static void FillRectangle (SpriteBatch spriteBatch, Vector2 location, Vector2 size, Brush brush, float angle)
        {
            spriteBatch.Draw(brush.Texture, 
                location, 
                null, 
                Color.White * brush.Alpha, 
                angle, 
                Vector2.Zero, 
                size,
                SpriteEffects.None, 
                0);
        }
    }

    // DrawRectangle
    public static partial class Draw2D
    {
        private static Dictionary<RectangleInfoKey, RectangleInfo> _rectangleCache = new Dictionary<RectangleInfoKey,RectangleInfo>();

        internal struct RectangleInfoKey
        {
            internal Point Size;
            internal float Angle;

            internal RectangleInfoKey (Point size, float angle)
            {
                Size = size;
                Angle = angle;
            }

            public static bool operator == (RectangleInfoKey key1, RectangleInfoKey key2) {
                return key1.Size == key2.Size && key1.Angle == key2.Angle;
            }

            public static bool operator != (RectangleInfoKey key1, RectangleInfoKey key2)
            {
                return !(key1 == key2);
            }

            public override bool Equals (object obj)
            {
                if (obj is RectangleInfoKey) {
                    return this == (RectangleInfoKey)obj;
                }
                return false;
            }

            public override int GetHashCode ()
            {
                int hash = 23;
                hash = hash * 37 + Size.X;
                hash = hash * 37 + Size.Y;
                hash = hash * 37 + Angle.GetHashCode();
                return hash;
            }

            public override string ToString ()
            {
                return Size.ToString() + ", " + Angle.ToString();
            }
        }

        internal struct RectangleInfo
        {
            internal Point P1;
            internal Point P2;

            internal RectangleInfo (Point p1, Point p2)
            {
                P1 = p1;
                P2 = p2;
            }
        }

        internal static RectangleInfo PrepareDrawRectangle (Rectangle rect, float angle)
        {
            int ox = rect.X;
            int oy = rect.Y;
            rect.Offset(-ox, -oy);

            float cosa = (float)Math.Cos(angle);
            float sina = (float)Math.Sin(angle);

            int p1x = (int)(rect.Right * cosa - rect.Top * sina);
            int p1y = (int)(rect.Right * sina + rect.Top * cosa);
            int p2x = (int)(rect.Left * cosa - rect.Bottom * sina);
            int p2y = (int)(rect.Left * sina + rect.Bottom * cosa);

            return new RectangleInfo(new Point(p1x, p1y), new Point(p2x, p2y));
        }

        public static void DrawRectangle (SpriteBatch spriteBatch, Rectangle rect, Pen pen)
        {
            int pw = pen.Width;
            int pwh = pw >> 1;
            int left = rect.Left - pwh;
            int top = rect.Top - pwh;
            int right = rect.Right - pwh;
            int bottom = rect.Bottom - pwh;
            int width = rect.Width + pwh * 2;
            int height = rect.Height + pwh * 2;

            spriteBatch.Draw(pen.Brush.Texture, new Rectangle(left, top, width, pw), Color.White * pen.Brush.Alpha);
            spriteBatch.Draw(pen.Brush.Texture, new Rectangle(left, bottom, width, pw), Color.White * pen.Brush.Alpha);
            spriteBatch.Draw(pen.Brush.Texture, new Rectangle(left, top, pw, height), Color.White * pen.Brush.Alpha);
            spriteBatch.Draw(pen.Brush.Texture, new Rectangle(right, top, pw, height), Color.White * pen.Brush.Alpha);
        }

        public static void DrawRectangle (SpriteBatch spriteBatch, Rectangle rect, Pen pen, float angle)
        {
            const float angle90 = (float)Math.PI / 2;

            RectangleInfoKey key = new RectangleInfoKey(new Point(rect.Width, rect.Height), angle);
            RectangleInfo info;
            if (!_rectangleCache.TryGetValue(key, out info)) {
                info = PrepareDrawRectangle(rect, angle);
                _rectangleCache[key] = info;
            }

            DrawLine(spriteBatch, new Point(rect.X, rect.Y), rect.Width, angle, pen);
            DrawLine(spriteBatch, new Point(rect.X + info.P2.X, rect.Y + info.P2.Y), rect.Width, angle, pen);
            DrawLine(spriteBatch, new Point(rect.X, rect.Y), rect.Height, angle + angle90, pen);
            DrawLine(spriteBatch, new Point(rect.X + info.P1.X, rect.Y + info.P1.Y), rect.Height, angle + angle90, pen);
        }
    }

    

    // DrawLine
    public static partial class Draw2D
    {
        private static Dictionary<Point, LineInfo> _lineCache = new Dictionary<Point,LineInfo>();

        internal struct LineInfo
        {
            internal int Length;
            internal float Angle;

            internal LineInfo (int length, float angle)
            {
                Length = length;
                Angle = angle;
            }
        }

        internal static LineInfo PrepareDrawLine (Point point1, Point point2)
        {
            float distance = Vector2.Distance(new Vector2(point1.X, point1.Y), new Vector2(point2.X, point2.Y));
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

            return new LineInfo((int)distance, angle);
        }

        public static void DrawLine (SpriteBatch spriteBatch, Point point1, Point point2, Pen pen)
        {
            Point key = new Point(point2.X - point1.X, point2.Y - point1.Y);
            LineInfo info;
            if (!_lineCache.TryGetValue(key, out info)) {
                info = PrepareDrawLine(point1, point2);
                _lineCache[key] = info;
            }

            DrawLine(spriteBatch, point1, info.Length, info.Angle, pen);
        }

        public static void DrawLine (SpriteBatch spriteBatch, Point location, int length, float angle, Pen pen)
        {
            int pwh = pen.Width >> 1;

            spriteBatch.Draw(pen.Brush.Texture,
                new Rectangle(location.X - pwh, location.Y - pwh, length + pwh * 2, pen.Width),
                null,
                Color.White * pen.Brush.Alpha,
                angle,
                new Vector2(pwh, pwh),
                SpriteEffects.None,
                0);
        }
    }

    
}
