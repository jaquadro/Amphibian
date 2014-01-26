using Amphibian.EntitySystem;
using Amphibian.Geometry;

namespace Amphibian.Systems.Actions
{
    public abstract class TemporalAction : EntityAction
    {
        private bool _complete;

        protected TemporalAction ()
        { }

        protected TemporalAction (float duration)
        {
            Duration = duration;
        }

        protected TemporalAction (float duration, Interpolation interpolation)
        {
            Duration = duration;
            Interpolation = interpolation;
        }

        public override bool Update (EntityWorld world, Entity entity)
        {
            if (_complete)
                return true;

            float delta = (float)world.GameTime.ElapsedGameTime.TotalSeconds;

            if (Time == 0)
                Begin(world, entity);
            Time += delta;

            _complete = Time >= Duration;
            float percent = 1;

            if (!_complete) {
                percent = Time / Duration;
                if (Interpolation != null)
                    percent = Interpolation.Apply(percent);
            }

            Update(world, entity, IsReverse ? 1 - percent : percent);
            if (_complete)
                End(world, entity);

            return _complete;
        }

        protected virtual void Begin (EntityWorld world, Entity entity)
        { }

        protected virtual void End (EntityWorld world, Entity entity)
        { }

        protected abstract void Update (EntityWorld world, Entity entity, float percent);

        public void Finish ()
        {
            Time = Duration;
        }

        public override void Restart ()
        {
            Time = 0;
            _complete = false;
        }

        public override void Reset ()
        {
            base.Reset();
            IsReverse = false;
        }

        public float Time { get; set; }
        public float Duration { get; set; }
        public bool IsReverse { get; set; }
        public Interpolation Interpolation { get; set; }
    }
}
