using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Amphibian.Utility;
using Amphibian.Systems.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Amphibian.EntitySystem;
using Amphibian.Components;
using System.Collections;

namespace Amphibian.Systems.Particle
{
    public class ParticleSystem : ProcessingSystem
    {
        public ParticleSystem ()
            : base(typeof(ParticleComponent))
        {
        }

        protected override void ProcessEntities (EntityManager.EntityEnumerator entities)
        {
            foreach (Entity entity in entities) {
                Process(entity);
            }
        }

        private void Process (Entity entity)
        {
            ParticleComponent comParticle = EntityManager.GetComponent<ParticleComponent>(entity);
            if (comParticle != null)
                comParticle.Emitter.Update(SystemManager.World.GameTime);
        }
    }

    public class ParticleComponent : IComponent
    {
        public StandardEmitter Emitter;
    }

    public class ParticleSpatial : SpriteSpatial
    {
        private ParticleEmitter _emitter;

        private Texture2D _texture;

        public ParticleSpatial (EntityWorld world, ParticleEmitter emitter, Texture2D texture)
            : base(world)
        {
            _emitter = emitter;
            _texture = texture;
        }

        public override void Render (SpriteBatch spriteBatch, Entity entity, Renderable position)
        {
            foreach (ParticleRenderData particle in _emitter.Particles) {
                Vector2 pos = new Vector2((float)position.RenderX + particle.Position.X, (float)position.RenderY + particle.Position.Y);
                spriteBatch.Draw(_texture, pos, null, particle.Color, particle.Rotation, new Vector2(8, 8), 1f, SpriteEffects.None, 0f);
            }
        }
    }

    public abstract class ParticleEmitter
    {
        public abstract int Count { get; }

        public ParticleEnumerator Particles
        {
            get { return new ParticleEnumerator(this); }
        }

        public abstract void Update (AmphibianGameTime time);

        protected abstract ParticleRenderData RenderData (int index);

        #region Particle Enumerator

        public struct ParticleEnumerator : IEnumerator<ParticleRenderData>
        {
            private ParticleEmitter _emitter;
            private int _index;

            internal ParticleEnumerator (ParticleEmitter emitter)
            {
                _index = -1;
                _emitter = emitter;
            }

            public ParticleRenderData Current
            {
                get { return _emitter.RenderData(_index); }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose ()
            {
            }

            public bool MoveNext ()
            {
                _index++;
                return _index < _emitter.Count;
            }

            public void Reset ()
            {
                _index = -1;
            }

            public ParticleEnumerator GetEnumerator ()
            {
                return this;
            }
        }

        #endregion
    }

    public struct ParticleRenderData
    {
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
    }
}
