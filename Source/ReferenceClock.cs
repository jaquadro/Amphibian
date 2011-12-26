using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian
{
    public class ReferenceClock : Component
    {
        private float _timeAccum;
        private float _duration;

        public ReferenceClock (float interval)
        {
            _duration = interval;
        }

        public event EventHandler Tick;

        protected virtual void OnTick (EventArgs e)
        {
            if (Tick != null) {
                Tick(this, e);
            }
        }

        public override void Update ()
        {
            _timeAccum += (float)Parent.Engine.GameTime.ElapsedGameTime.TotalSeconds;

            if (_timeAccum > _duration) {
                _timeAccum -= _duration;
                OnTick(EventArgs.Empty);
            }
        }
    }
}
