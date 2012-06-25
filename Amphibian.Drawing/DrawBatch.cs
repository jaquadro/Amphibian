﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Amphibian.Drawing
{
    public class DrawBatch
    {
        private struct DrawingInfo
        {
            public Texture2D Texture;
            public int IndexCount;
            public int VertexCount;
        }

        private GraphicsDevice _device;
        private bool _inDraw;

        private DrawingInfo[] _infoBuffer;
        private short[] _indexBuffer;
        private VertexPositionColorTexture[] _vertexBuffer;

        private Vector2[] _computeBuffer;
        private Vector2[] _geometryBuffer;

        private int _infoBufferIndex;
        private int _indexBufferIndex;
        private int _vertexBufferIndex;

        private static BasicEffect _effect;

        public DrawBatch (GraphicsDevice device)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            _device = device;

            _infoBuffer = new DrawingInfo[2048];
            _indexBuffer = new short[32768];
            _vertexBuffer = new VertexPositionColorTexture[8192];
            _computeBuffer = new Vector2[64];
            _geometryBuffer = new Vector2[256];

            _effect = new BasicEffect(device);
            _effect.TextureEnabled = true;
            _effect.VertexColorEnabled = true;
        }

        public void Begin ()
        {
            _inDraw = true;

            _infoBufferIndex = 0;
            _indexBufferIndex = 0;
            _vertexBufferIndex = 0;
        }

        public void End ()
        {
            FlushBuffer();

            _inDraw = false;
        }

        public void DrawRectangle (Rectangle rect, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(8, 24);

            AddInfo(8, 24, pen.Brush);

            Vector2 a = new Vector2(rect.Left, rect.Top);
            Vector2 b = new Vector2(rect.Right, rect.Top);
            Vector2 c = new Vector2(rect.Right, rect.Bottom);
            Vector2 d = new Vector2(rect.Left, rect.Bottom);

            int baseVertexIndex = _vertexBufferIndex;

            AddMiteredJoint(a, b, c, pen);
            AddMiteredJoint(b, c, d, pen);
            AddMiteredJoint(c, d, a, pen);
            AddMiteredJoint(d, a, b, pen);

            AddSegment(baseVertexIndex + 0, baseVertexIndex + 2);
            AddSegment(baseVertexIndex + 2, baseVertexIndex + 4);
            AddSegment(baseVertexIndex + 4, baseVertexIndex + 6);
            AddSegment(baseVertexIndex + 6, baseVertexIndex + 0);
        }

        public void DrawPoint (Point point, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(4, 6);

            AddInfo(4, 6, pen.Brush);

            int baseVertexIndex = _vertexBufferIndex;

            float w2 = pen.Width / 2;
            AddVertex(new Vector2(point.X - w2, point.Y - w2), pen);
            AddVertex(new Vector2(point.X + w2, point.Y - w2), pen);
            AddVertex(new Vector2(point.X - w2, point.Y + w2), pen);
            AddVertex(new Vector2(point.X + w2, point.Y + w2), pen);

            AddSegment(baseVertexIndex + 0, baseVertexIndex + 2);
        }

        public void DrawLine (Point p0, Point p1, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(4, 6);

            AddInfo(4, 6, pen.Brush);

            int baseVertexIndex = _vertexBufferIndex;

            AddStartPoint(new Vector2(p0.X, p0.Y), new Vector2(p1.X, p1.Y), pen);
            AddEndPoint(new Vector2(p0.X, p0.Y), new Vector2(p1.X, p1.Y), pen);

            AddSegment(baseVertexIndex + 0, baseVertexIndex + 2);
        }

        public void DrawPath (GraphicsPath path)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(path.VertexCount, path.IndexCount);

            AddInfo(path.VertexCount, path.IndexCount, path.Pen.Brush);

            for (int i = 0; i < path.VertexCount; i++) {
                _vertexBuffer[_vertexBufferIndex + i] = new VertexPositionColorTexture(
                    new Vector3(path.VertexPositionData[i], 0),
                    path.VertexColorData[i],
                    path.VertexTextureData[i]);
            }

            for (int i = 0; i < path.IndexCount; i++) {
                _indexBuffer[_indexBufferIndex + i] = (short)(path.IndexData[i] + _vertexBufferIndex);
            }

            _vertexBufferIndex += path.VertexCount;
            _indexBufferIndex += path.IndexCount;
        }

        public void DrawCircle (Point center, float radius, Pen pen)
        {
            DrawCircle(center, radius, (int)Math.Ceiling(radius / 1.5), pen);
        }

        public void DrawCircle (Point center, float radius, int subdivisions, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            List<Vector2> unitCircle = CalculateCircleSubdivisions(subdivisions);

            if (_geometryBuffer.Length < subdivisions) {
                Array.Resize(ref _geometryBuffer, subdivisions * 2);
            }

            for (int i = 0; i < subdivisions; i++) {
                _geometryBuffer[i] = new Vector2(center.X + radius * unitCircle[i].X, center.Y + radius * unitCircle[i].Y);
            }

            AddClosedPath(_geometryBuffer, 0, subdivisions, pen);
        }

        public void FillRectangle (Rectangle rect, Brush brush)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(4, 6);

            AddInfo(4, 6, brush);

            int baseVertexIndex = _vertexBufferIndex;

            AddVertex(new Vector2(rect.Left, rect.Top), brush);
            AddVertex(new Vector2(rect.Right, rect.Top), brush);
            AddVertex(new Vector2(rect.Left, rect.Bottom), brush);
            AddVertex(new Vector2(rect.Right, rect.Bottom), brush);

            AddTriangle(baseVertexIndex + 0, baseVertexIndex + 1, baseVertexIndex + 2);
            AddTriangle(baseVertexIndex + 1, baseVertexIndex + 3, baseVertexIndex + 2);
        }

        private void SetRenderState ()
        {
            _device.BlendState = BlendState.AlphaBlend;
            _device.DepthStencilState = DepthStencilState.None;
            _device.RasterizerState = RasterizerState.CullCounterClockwise;
            _device.SamplerStates[0] = SamplerState.PointWrap;

            _effect.Projection = Matrix.CreateOrthographicOffCenter(0, _device.Viewport.Width, _device.Viewport.Height, 0, -1, 1);
            _effect.CurrentTechnique.Passes[0].Apply();
        }

        private void AddMiteredJoint (Vector2 a, Vector2 b, Vector2 c, Pen pen)
        {
            pen.ComputeMiter(_computeBuffer, 0, a, b, c);

            AddVertex(_computeBuffer[0], pen);
            AddVertex(_computeBuffer[1], pen);
        }

        private void AddStartPoint (Vector2 a, Vector2 b, Pen pen)
        {
            pen.ComputeStartPoint(_computeBuffer, 0, a, b);

            AddVertex(_computeBuffer[0], pen);
            AddVertex(_computeBuffer[1], pen);
        }

        private void AddEndPoint (Vector2 a, Vector2 b, Pen pen)
        {
            pen.ComputeEndPoint(_computeBuffer, 0, a, b);

            AddVertex(_computeBuffer[0], pen);
            AddVertex(_computeBuffer[1], pen);
        }

        private void AddInfo (int vertexCount, int indexCount, Brush brush)
        {
            _infoBuffer[_infoBufferIndex].Texture = brush != null ? brush.Texture : null;
            _infoBuffer[_infoBufferIndex].IndexCount = indexCount;
            _infoBuffer[_infoBufferIndex].VertexCount = vertexCount;
            _infoBufferIndex++;
        }

        private void AddInfo (int vertexCount, int indexCount, Texture2D texture)
        {
            _infoBuffer[_infoBufferIndex].Texture = texture;
            _infoBuffer[_infoBufferIndex].IndexCount = indexCount;
            _infoBuffer[_infoBufferIndex].VertexCount = vertexCount;
            _infoBufferIndex++;
        }

        private void AddClosedPath (Vector2[] points, int offset, int count, Pen pen)
        {
            RequestBufferSpace(count * 2, count * 6);

            AddInfo(count * 2, count * 6, pen.Brush);

            int baseVertexIndex = _vertexBufferIndex;

            for (int i = 0; i < count - 2; i++) {
                AddMiteredJoint(points[offset + i], points[offset + i + 1], points[offset + i + 2], pen);
            }

            AddMiteredJoint(points[offset + count - 2], points[offset + count - 1], points[offset + 0], pen);
            AddMiteredJoint(points[offset + count - 1], points[offset + 0], points[offset + 1], pen);

            for (int i = 0; i < count - 1; i++) {
                AddSegment(baseVertexIndex + i * 2, baseVertexIndex + (i + 1) * 2);
            }

            AddSegment(baseVertexIndex + (count - 1) * 2, baseVertexIndex + 0);
        }

        private void AddVertex (Vector2 position, Pen pen)
        {
            VertexPositionColorTexture vertex = new VertexPositionColorTexture();
            vertex.Position = new Vector3(position, 0);
            vertex.Color = pen.Color;

            if (pen.Brush != null && pen.Brush.Texture != null) {
                Texture2D tex = pen.Brush.Texture;
                vertex.TextureCoordinate = new Vector2(position.X / tex.Width, position.Y / tex.Height);
            }

            _vertexBuffer[_vertexBufferIndex++] = vertex;
        }

        private void AddVertex (Vector2 position, Brush brush)
        {
            VertexPositionColorTexture vertex = new VertexPositionColorTexture();
            vertex.Position = new Vector3(position, 0);
            vertex.Color = Color.White;

            if (brush != null && brush.Texture != null) {
                Texture2D tex = brush.Texture;
                vertex.TextureCoordinate = new Vector2(position.X / tex.Width, position.Y / tex.Height);
            }

            _vertexBuffer[_vertexBufferIndex++] = vertex;
        }

        private void AddSegment (int startVertexIndex, int endVertexIndex)
        {
            _indexBuffer[_indexBufferIndex++] = (short)(startVertexIndex + 0);
            _indexBuffer[_indexBufferIndex++] = (short)(startVertexIndex + 1);
            _indexBuffer[_indexBufferIndex++] = (short)(endVertexIndex + 1);
            _indexBuffer[_indexBufferIndex++] = (short)(startVertexIndex + 0);
            _indexBuffer[_indexBufferIndex++] = (short)(endVertexIndex + 1);
            _indexBuffer[_indexBufferIndex++] = (short)(endVertexIndex + 0);
        }

        private void AddTriangle (int a, int b, int c)
        {
            _indexBuffer[_indexBufferIndex++] = (short)a;
            _indexBuffer[_indexBufferIndex++] = (short)b;
            _indexBuffer[_indexBufferIndex++] = (short)c;
        }

        private void FlushBuffer ()
        {
            SetRenderState();

            int vertexOffset = 0;
            int indexOffset = 0;
            int vertexCount = 0;
            int indexCount = 0;
            Texture2D texture = null;

            for (int i = 0; i < _infoBufferIndex; i++) {
                if (texture != _infoBuffer[i].Texture) {
                    if (indexCount > 0) {
                        for (int j = 0; j < indexCount; j++) {
                            _indexBuffer[indexOffset + j] -= (short)vertexOffset;
                        }

                        RenderBatch(indexOffset, indexCount, vertexOffset, vertexCount, texture);
                    }

                    vertexOffset += vertexCount;
                    indexOffset += indexCount;
                    vertexCount = 0;
                    indexCount = 0;
                    texture = _infoBuffer[i].Texture;
                }

                vertexCount += _infoBuffer[i].VertexCount;
                indexCount += _infoBuffer[i].IndexCount;
            }

            if (indexCount > 0) {
                for (int j = 0; j < indexCount; j++) {
                    _indexBuffer[indexOffset + j] -= (short)vertexOffset;
                }

                RenderBatch(indexOffset, indexCount, vertexOffset, vertexCount, texture);
            }

            ClearInfoBuffer();

            _infoBufferIndex = 0;
            _indexBufferIndex = 0;
            _vertexBufferIndex = 0;
        }

        private void RenderBatch (int indexOffset, int indexCount, int vertexOffset, int vertexCount, Texture2D texture)
        {
            _device.Textures[0] = texture;
            _device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertexBuffer, vertexOffset, vertexCount, _indexBuffer, indexOffset, indexCount / 3);
        }

        private void ClearInfoBuffer ()
        {
            for (int i = 0; i < _infoBufferIndex; i++)
                _infoBuffer[i].Texture = null;
        }

        private void RequestBufferSpace (int newVertexCount, int newIndexCount)
        {
            if (_indexBufferIndex + newIndexCount > short.MaxValue) {
                FlushBuffer();
            }

            if (_infoBufferIndex + 1 > _infoBuffer.Length) {
                Array.Resize(ref _infoBuffer, _infoBuffer.Length * 2);
            }

            if (_indexBufferIndex + newIndexCount >= _indexBuffer.Length) {
                Array.Resize(ref _indexBuffer, _indexBuffer.Length * 2);
            }

            if (_vertexBufferIndex + newVertexCount >= _vertexBuffer.Length) {
                Array.Resize(ref _vertexBuffer, _vertexBuffer.Length * 2);
            }
        }

        private Dictionary<int, List<Vector2>> _circleCache = new Dictionary<int,List<Vector2>>();

        private List<Vector2> CalculateCircleSubdivisions (int divisions)
        {
            if (_circleCache.ContainsKey(divisions))
                return _circleCache[divisions];

            if (divisions < 0)
                throw new ArgumentOutOfRangeException("divisions");

            double slice = Math.PI * 2 / divisions;

            List<Vector2> unitCircle = new List<Vector2>();

            for (int i = 0; i < divisions; i++) {
                double angle = slice * i;
                unitCircle.Add(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
            }

            _circleCache.Add(divisions, unitCircle);
            return unitCircle;
        }
    }
}
