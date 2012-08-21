using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.Systems.Particle
{
    public abstract class Distribution
    {
        private static Random _random = new Random();

        public static UniformDistribution Uniform { get; private set; }
        public static TriangleDistribution Triangle { get; private set; }

        static Distribution ()
        {
            Uniform = new UniformDistribution();
            Triangle = new TriangleDistribution();
        }

        protected float Random ()
        {
            return (float)_random.NextDouble();
        }

        public abstract float Sample ();
    }

    public class UniformDistribution : Distribution
    {
        public override float Sample ()
        {
            return Random();
        }
    }

    public class TriangleDistribution : Distribution
    {
        public override float Sample ()
        {
            float q = Random();
            if (q <= .5f)
                return (float)Math.Sqrt(.5f * q);
            else
                return 1f - (float)Math.Sqrt(.5f * (1 - q));
        }
    }
}
