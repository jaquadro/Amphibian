using System;
using Microsoft.Xna.Framework.Graphics;

namespace Amphibian.Drawing
{
    public class Pen : IDisposable
    {
        public Brush Brush { get; set; }
        public int Width { get; set; }

        public Pen (Brush brush, int width)
        {
            Brush = brush;
            Width = width;
        }

        public Pen (Brush brush)
            : this(brush, 1)
        {
        }

        #region IDisposable

        private bool _disposed;

        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose (bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    Brush.Dispose();
                    DisposeManaged();
                }
                DisposeUnmanaged();
                _disposed = true;
            }
        }

        ~Pen ()
        {
            Dispose(false);
        }

        protected virtual void DisposeManaged () { }

        protected virtual void DisposeUnmanaged () { }

        #endregion
    }
}
