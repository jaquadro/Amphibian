using System;
using System.Collections.Generic;
using Amphibian.EntitySystem;
using Amphibian.Utility;

namespace Amphibian.Systems
{
    public class RefClock
    {
        private float _timeAccum;
        private float _duration;
        private bool _triggered;

        public RefClock (float interval)
        {
            _duration = interval;
        }

        public bool Triggered
        {
            get { return _triggered; }
        }

        public void Update (float elapsed)
        {
            _timeAccum += elapsed;

            if (_timeAccum > _duration) {
                _timeAccum -= _duration;
                _triggered = true;
            }
        }

        public void Reset ()
        {
            _triggered = false;
        }
    }

    public class ClockSystem : BaseSystem
    {
        private UnorderedList<WeakReference> _clocks;

        public ClockSystem ()
        {
            _clocks = new UnorderedList<WeakReference>();
        }

        protected override void ProcessInner ()
        {
            float elapsed = (float)SystemManager.World.GameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < _clocks.Count; i++) {
                RefClock clock = _clocks[i].Target as RefClock;
                if (clock != null) {
                    clock.Update(elapsed);
                }
                else {
                    _clocks.RemoveAt(i--);
                }
            }
        }

        internal void Reset ()
        {
            for (int i = 0; i < _clocks.Count; i++) {
                RefClock clock = _clocks[i].Target as RefClock;
                if (clock != null) {
                    clock.Reset();
                }
                else {
                    _clocks.RemoveAt(i--);
                }
            }
        }

        public RefClock Create (float interval)
        {
            RefClock clock = new RefClock(interval);
            _clocks.Add(new WeakReference(clock));

            return clock;
        }
    }

    public class ClockResetSystem : BaseSystem
    {
        protected override void ProcessInner ()
        {
            ClockSystem clockSys = SystemManager.GetSystem(typeof(ClockSystem)) as ClockSystem;
            if (clockSys != null) {
                clockSys.Reset();
            }
        }
    }
}
