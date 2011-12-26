using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian
{
    public class AmphibianGameTime
    {
        private TimeSpan _elapsedTime;
        private TimeSpan _totalTime;
        private bool _runningSlowly;

        public AmphibianGameTime ()
        {
        }

        public AmphibianGameTime (TimeSpan totalGameTime, TimeSpan elapsedGameTime)
            : this(totalGameTime, elapsedGameTime, false)
        {
        }

        public AmphibianGameTime (TimeSpan totalGameTime, TimeSpan elapsedGameTime, bool isRunningSlowly)
        {
            _totalTime = totalGameTime;
            _elapsedTime = elapsedGameTime;
            _runningSlowly = isRunningSlowly;
        }

        public TimeSpan ElapsedGameTime
        {
            get { return _elapsedTime; }
            protected internal set { _elapsedTime = value; }
        }

        public TimeSpan TotalGameTime
        {
            get { return _totalTime; }
            protected internal set { _totalTime = value; }
        }

        public bool IsRunningSlowly
        {
            get { return _runningSlowly; }
            protected internal set { _runningSlowly = value; }
        }

        public void Copy (AmphibianGameTime time)
        {
            _totalTime = time._totalTime;
            _elapsedTime = time._elapsedTime;
            _runningSlowly = time._runningSlowly;
        }

#if XNA
        public void Copy (Microsoft.Xna.Framework.GameTime time)
        {
            _totalTime = time.TotalGameTime;
            _elapsedTime = time.ElapsedGameTime;
            _runningSlowly = time.IsRunningSlowly;
        }

        public static explicit operator Microsoft.Xna.Framework.GameTime (AmphibianGameTime value)
        {
            return new Microsoft.Xna.Framework.GameTime(value._totalTime, value._elapsedTime, value._runningSlowly);
        }

        public static explicit operator AmphibianGameTime (Microsoft.Xna.Framework.GameTime value)
        {
            return new AmphibianGameTime(value.TotalGameTime, value.ElapsedGameTime, value.IsRunningSlowly);
        }
#endif
    }
}
