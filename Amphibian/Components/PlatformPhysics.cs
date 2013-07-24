using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Geometry;
using Amphibian.EntitySystem;
using Amphibian.Utility;

namespace Amphibian.Components
{
    public enum PlatformAccelState
    {
        Fixed,
        Accelerate,
        Decelerate
    }

    public enum PlatformState
    {
        NonSolid,
        BackgroundSolid,
        Solid,
        Platform,
    }

    public class MemoValue<T>
    {
        private T _value;

        public MemoValue (T initValue)
        {
            InitValue = initValue;
            CurrentValue = initValue;
        }

        public void Reset ()
        {
            PreviousValue = CurrentValue;
            CurrentValue = InitValue;
        }

        public T InitValue { get; set; }

        public T PreviousValue { get; set; }

        public T CurrentValue
        {
            get { return _value; }
            set
            {
                PreviousValue = _value;
                _value = value;
            }
        }
    }

    public sealed class PlatformPhysics : IComponent
    {
        public FPInt AccelX
        {
            get { return AccelXMem.CurrentValue; }
            set { AccelXMem = SetMemValue<FPInt>(value, AccelXMem); }
        }

        public FPInt AccelY
        {
            get { return AccelYMem.CurrentValue; }
            set { AccelYMem = SetMemValue<FPInt>(value, AccelYMem); }
        }

        public MemoValue<FPInt> AccelXMem { get; set; }
        public MemoValue<FPInt> AccelYMem { get; set; }

        public FPInt DecelX;
        public FPInt DecelY;

        public FPInt MaxVelocityX
        {
            get { return MaxVelocityXMem.CurrentValue; }
            set { MaxVelocityXMem = SetMemValue<FPInt>(value, MaxVelocityXMem); }
        }

        public FPInt MaxVelocityY;

        public MemoValue<FPInt> MaxVelocityXMem { get; set; }

        public FPInt MinVelocityX
        {
            get { return MinVelocityXMem.CurrentValue; }
            set { MinVelocityXMem = SetMemValue<FPInt>(value, MinVelocityXMem); }
        }

        public FPInt MinVelocityY;

        public MemoValue<FPInt> MinVelocityXMem { get; set; }

        public FPInt VelocityX;
        public FPInt VelocityY;

        public PlatformAccelState AccelStateX;
        public PlatformAccelState AccelStateY;

        public PlatformState State = PlatformState.BackgroundSolid;

        private static MemoValue<T> SetMemValue<T> (T value, MemoValue<T> memo)
        {
            if (memo == null) {
                memo = new MemoValue<T>(value);
                return memo;
            }

            memo.CurrentValue = value;
            return memo;
        }

        /// <summary>
        /// Entities that should be affected by changes to this component
        /// </summary>
        public UnorderedList<Entity> DependentEntities = new UnorderedList<Entity>(2);

        public PlatformPusher Pusher = null;
        public PlatformPushable Pushable = null;
    }

    public class Physics : IComponent
    {
        public FPInt AccelX;
        public FPInt AccelY;

        public FPInt DecelX;
        public FPInt DecelY;

        public FPInt MaxVelocityX;
        public FPInt MaxVelocityY;

        public FPInt MinVelocityX;
        public FPInt MinVelocityY;

        public FPInt VelocityX;
        public FPInt VelocityY;

        public PlatformAccelState AccelStateX;
        public PlatformAccelState AccelStateY;
    }
}
