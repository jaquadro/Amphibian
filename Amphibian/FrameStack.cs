using System;
using System.Collections.Generic;

namespace Amphibian
{
    public class FrameStack : IEnumerable<Frame>
    {
        private Engine _engine;
        List<Frame> _frames;

        internal FrameStack (Engine engine)
        {
            _engine = engine;
            _frames = new List<Frame>();
        }

        public int Count
        {
            get { return _frames.Count; }
        }

        public bool Empty
        {
            get { return _frames.Count == 0; }
        }

        public Frame TopFrame
        {
            get { return (_frames.Count > 0) ? _frames[_frames.Count - 1] : null; }
        }

        internal Frame this[int index]
        {
            get { return _frames[index]; }
        }

        public void Push (Frame frame)
        {
            if (frame.Engine != null && frame.Engine != _engine)
                throw new Exception("Frame already bound to engine");

            if (!_frames.Contains(frame)) {
                _frames.Add(frame);

                frame.Engine = _engine;
                frame.LoadFrame();
            }
        }

        public Frame Pop ()
        {
            if (_frames.Count == 0)
                return null;

            Frame frame = _frames[_frames.Count - 1];

            frame.UnloadFrame();
            frame.Engine = null;

            _frames.RemoveAt(_frames.Count - 1);

            return frame;
        }

        public void ReplaceFrame (Frame existingFrame, Frame newFrame)
        {
            if (existingFrame == newFrame)
                return;

            if (newFrame.Engine != null && newFrame.Engine != _engine)
                throw new Exception("Frame already bound to engine");

            int existingIndex = _frames.IndexOf(existingFrame);
            if (existingIndex < 0) {
                Push(newFrame);
                return;
            }

            existingFrame.UnloadFrame();
            existingFrame.Engine = null;

            _frames[existingIndex] = newFrame;

            newFrame.Engine = _engine;
            newFrame.LoadFrame();
        }

        public Frame ReplaceTopFrame (Frame newFrame)
        {
            if (newFrame.Engine != null && newFrame.Engine != _engine)
                throw new Exception("Frame already bound to engine");

            Frame oldFrame = Pop();
            Push(newFrame);

            return oldFrame;
        }

        public IEnumerator<Frame> GetEnumerator ()
        {
            for (int i = _frames.Count - 1; i >= 0; i--)
                yield return _frames[i];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }
    }
}
