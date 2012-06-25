using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Drawing
{
    public interface IGraphicsPath
    {
        int VertexCount { get; }
        int IndexCount { get; }

        Vector2[] VertexPositionData { get; }
        Vector2[] VertexTextureData { get; }
        Color[] VertexColorData { get; }
        short[] IndexData { get; }

        Pen Pen { get; }
    }

    public enum PathType
    {
        Open,
        Closed,
    }

    public class GraphicsPath : IGraphicsPath
    {
        private Pen _pen;

        private int _pointCount;
        private int _vertexCount;
        private int _indexCount;

        private Vector2[] _positionData;
        private Vector2[] _textureData;
        private Color[] _colorData;
        private short[] _indexData;

        public GraphicsPath (Pen pen)
        {
            _pen = pen;
        }

        public GraphicsPath (Pen pen, IList<Vector2> points)
            : this(pen, points, 0, points.Count, PathType.Open)
        { }

        public GraphicsPath (Pen pen, IList<Vector2> points, PathType pathType)
            : this(pen, points, 0, points.Count, pathType)
        { }

        public GraphicsPath (Pen pen, IList<Vector2> points, int offset, int count, PathType pathType)
            : this(pen)
        {
            _pointCount = count;

            switch (pathType) {
                case PathType.Open:
                    CompileOpenPath(points, offset, count);
                    break;

                case PathType.Closed:
                    CompileClosedPath(points, offset, count);
                    break;
            }
        }

        #region IGraphicsPath Interface

        public int IndexCount
        {
            get { return _indexCount; }
        }

        public int VertexCount
        {
            get { return _vertexCount; }
        }

        public Vector2[] VertexPositionData
        {
            get { return _positionData; }
        }

        public Vector2[] VertexTextureData
        {
            get { return _textureData; }
        }

        public Color[] VertexColorData
        {
            get { return _colorData; }
        }

        public short[] IndexData
        {
            get { return _indexData; }
        }

        public Pen Pen
        {
            get { return _pen; }
        }

        #endregion

        private void InitializeBuffers (int vertexCount, int indexCount)
        {
            _vertexCount = vertexCount;
            _indexCount = indexCount;

            _indexData = new short[indexCount];
            _positionData = new Vector2[vertexCount];

            if (_pen.Color != null)
                _colorData = new Color[vertexCount];

            if (_pen.Brush != null && _pen.Brush.Texture != null)
                _textureData = new Vector2[vertexCount];
        }

        private void CompileOpenPath (IList<Vector2> points, int offset, int count)
        {
            InitializeBuffers(count * 2, (count - 1) * 6);

            AddStartPoint(0, points[offset + 0], points[offset + 1]);

            for (int i = 0; i < count - 2; i++) {
                AddMiteredJoint(i + 1, points[offset + i], points[offset + i + 1], points[offset + i + 2]);
                AddSegment(i, i + 1);
            }

            AddEndPoint(count - 1, points[offset + count - 2], points[offset + count - 1]);
            AddSegment(count - 2, count - 1);
        }

        private void CompileClosedPath (IList<Vector2> points, int offset, int count)
        {
            InitializeBuffers(count * 2, count * 6);

            AddMiteredJoint(0, points[offset + count - 1], points[offset + 0], points[offset + 1]);

            for (int i = 0; i < count - 2; i++) {
                AddMiteredJoint(i + 1, points[offset + i], points[offset + i + 1], points[offset + i + 2]);
                AddSegment(i, i + 1);
            }

            AddMiteredJoint(count - 1, points[offset + count - 2], points[offset + count - 1], points[offset + 0]);
            AddSegment(count - 2, count - 1);

            AddSegment(count - 1, 0);
        }

        private void AddStartPoint (int pointIndex, Vector2 a, Vector2 b)
        {
            short vIndex = (short)(pointIndex * 2);

            //ComputeFlatStartPoint(a, b, _pen.Width, out _positionData[vIndex + 0], out _positionData[vIndex + 1]);
            _pen.ComputeStartPoint(_positionData, vIndex, a, b);

            if (_colorData != null) {
                _colorData[vIndex + 0] = _pen.Color;
                _colorData[vIndex + 1] = _pen.Color;
            }

            if (_textureData != null) {
                int texWidth = _pen.Brush.Texture.Width;
                int texHeight = _pen.Brush.Texture.Height;

                Vector2 pos0 = _positionData[vIndex + 0];
                Vector2 pos1 = _positionData[vIndex + 1];

                _textureData[vIndex + 0] = new Vector2(pos0.X / texWidth, pos0.Y / texHeight);
                _textureData[vIndex + 1] = new Vector2(pos1.X / texWidth, pos1.Y / texHeight);
            }
        }

        private void AddEndPoint (int pointIndex, Vector2 a, Vector2 b)
        {
            short vIndex = (short)(pointIndex * 2);

            //ComputeFlatEndPoint(a, b, _pen.Width, out _positionData[vIndex + 0], out _positionData[vIndex + 1]);
            _pen.ComputeEndPoint(_positionData, vIndex, a, b);

            if (_colorData != null) {
                _colorData[vIndex + 0] = _pen.Color;
                _colorData[vIndex + 1] = _pen.Color;
            }

            if (_textureData != null) {
                int texWidth = _pen.Brush.Texture.Width;
                int texHeight = _pen.Brush.Texture.Height;

                Vector2 pos0 = _positionData[vIndex + 0];
                Vector2 pos1 = _positionData[vIndex + 1];

                _textureData[vIndex + 0] = new Vector2(pos0.X / texWidth, pos0.Y / texHeight);
                _textureData[vIndex + 1] = new Vector2(pos1.X / texWidth, pos1.Y / texHeight);
            }
        }

        private void AddMiteredJoint (int pointIndex, Vector2 a, Vector2 b, Vector2 c)
        {
            short vIndex = (short)(pointIndex * 2);

            //ComputeMiter(a, b, c, _pen.Width, out _positionData[vIndex + 0], out _positionData[vIndex + 1]);
            _pen.ComputeMiter(_positionData, vIndex, a, b, c);

            if (_colorData != null) {
                _colorData[vIndex + 0] = _pen.Color;
                _colorData[vIndex + 1] = _pen.Color;
            }

            if (_textureData != null) {
                int texWidth = _pen.Brush.Texture.Width;
                int texHeight = _pen.Brush.Texture.Height;

                Vector2 pos0 = _positionData[vIndex + 0];
                Vector2 pos1 = _positionData[vIndex + 1];

                _textureData[vIndex + 0] = new Vector2(pos0.X / texWidth, pos0.Y / texHeight);
                _textureData[vIndex + 1] = new Vector2(pos1.X / texWidth, pos1.Y / texHeight);
            }
        }

        private void AddSegment (int startPoint, int endPoint)
        {
            short vIndexStart = (short)(startPoint * 2);
            short vIndexEnd = (short)(endPoint * 2);
            short iIndex = (short)(startPoint * 6);

            _indexData[iIndex + 0] = (short)(vIndexStart + 0);
            _indexData[iIndex + 1] = (short)(vIndexStart + 1);
            _indexData[iIndex + 2] = (short)(vIndexEnd + 1);
            _indexData[iIndex + 3] = (short)(vIndexStart + 0);
            _indexData[iIndex + 4] = (short)(vIndexEnd + 1);
            _indexData[iIndex + 5] = (short)(vIndexEnd + 0);
        }

        /*private void ComputeMiter (Vector2 a, Vector2 b, Vector2 c, float width, out Vector2 m0, out Vector2 m1)
        {
            float w2 = width / 2;

            Vector2 edgeAB = new Vector2(b.X - a.X, b.Y - a.Y);
            edgeAB.Normalize();
            Vector2 edgeABt = new Vector2(-edgeAB.Y, edgeAB.X);

            Vector2 edgeBC = new Vector2(c.X - b.X, c.Y - b.Y);
            edgeBC.Normalize();
            Vector2 edgeBCt = new Vector2(-edgeBC.Y, edgeBC.X);

            Vector2 point2 = new Vector2(a.X + w2 * edgeABt.X, a.Y + w2 * edgeABt.Y);
            Vector2 point4 = new Vector2(a.X - w2 * edgeABt.X, a.Y - w2 * edgeABt.Y);

            Vector2 point1 = new Vector2(c.X + w2 * edgeBCt.X, c.Y + w2 * edgeBCt.Y);
            Vector2 point3 = new Vector2(c.X - w2 * edgeBCt.X, c.Y - w2 * edgeBCt.Y);

            float offset01 = Vector2.Dot(edgeBCt, point1);
            float t0 = (offset01 - Vector2.Dot(edgeBCt, point2)) / Vector2.Dot(edgeBCt, edgeAB);
            Vector2 point0 = new Vector2(point2.X + t0 * edgeAB.X, point2.Y + t0 * edgeAB.Y);

            float offset35 = Vector2.Dot(edgeBCt, point3);
            float t5 = (offset35 - Vector2.Dot(edgeBCt, point4)) / Vector2.Dot(edgeBCt, edgeAB);
            Vector2 point5 = new Vector2(point4.X + t5 * edgeAB.X, point4.Y + t5 * edgeAB.Y);

            m0 = point0;
            m1 = point5;
        }

        private void ComputeFlatStartPoint (Vector2 a, Vector2 b, float width, out Vector2 m0, out Vector2 m1)
        {
            float w2 = width / 2;

            Vector2 edgeAB = new Vector2(b.X - a.X, b.Y - a.Y);
            edgeAB.Normalize();
            Vector2 edgeABt = new Vector2(-edgeAB.Y, edgeAB.X);

            m0 = new Vector2(a.X + w2 * edgeABt.X, a.Y + w2 * edgeABt.Y);
            m1 = new Vector2(a.X - w2 * edgeABt.X, a.Y - w2 * edgeABt.Y);
        }

        private void ComputeSquareStartPoint (Vector2 a, Vector2 b, float width, out Vector2 m0, out Vector2 m1)
        {
            float w2 = width / 2;

            Vector2 edgeAB = new Vector2(b.X - a.X, b.Y - a.Y);
            edgeAB.Normalize();
            Vector2 edgeABt = new Vector2(-edgeAB.Y, edgeAB.X);

            a = new Vector2(b.X - w2 * edgeAB.X, b.Y - w2 * edgeAB.Y);

            m0 = new Vector2(a.X + w2 * edgeABt.X, a.Y + w2 * edgeABt.Y);
            m1 = new Vector2(a.X - w2 * edgeABt.X, a.Y - w2 * edgeABt.Y);
        }

        private void ComputeFlatEndPoint (Vector2 a, Vector2 b, float width, out Vector2 m0, out Vector2 m1)
        {
            float w2 = width / 2;

            Vector2 edgeAB = new Vector2(b.X - a.X, b.Y - a.Y);
            edgeAB.Normalize();
            Vector2 edgeABt = new Vector2(-edgeAB.Y, edgeAB.X);

            m0 = new Vector2(b.X + w2 * edgeABt.X, b.Y + w2 * edgeABt.Y);
            m1 = new Vector2(b.X - w2 * edgeABt.X, b.Y - w2 * edgeABt.Y);
        }

        private void ComputeSquareEndPoint (Vector2 a, Vector2 b, float width, out Vector2 m0, out Vector2 m1)
        {
            float w2 = width / 2;

            Vector2 edgeAB = new Vector2(b.X - a.X, b.Y - a.Y);
            edgeAB.Normalize();
            Vector2 edgeABt = new Vector2(-edgeAB.Y, edgeAB.X);

            b = new Vector2(b.X + w2 * edgeAB.X, b.Y + w2 * edgeAB.Y);

            m0 = new Vector2(b.X + w2 * edgeABt.X, b.Y + w2 * edgeABt.Y);
            m1 = new Vector2(b.X - w2 * edgeABt.X, b.Y - w2 * edgeABt.Y);
        }*/
    }

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
            int pw = (int)pen.Width;
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
            int pwh = (int)pen.Width >> 1;
            float unitW = pen.Brush.Texture.Width / (float)(length + pen.Width);
            float unitH = pen.Brush.Texture.Height / (float)pen.Width;

            float originX = unitW * pwh;
            float originY = unitH * pwh;

            spriteBatch.Draw(pen.Brush.Texture,
                new Rectangle(location.X, location.Y, length + pwh * 2, (int)pen.Width),
                null,
                Color.White * pen.Brush.Alpha,
                angle,
                new Vector2(originX, originY),
                SpriteEffects.None,
                0);
        }
    }

    
}
