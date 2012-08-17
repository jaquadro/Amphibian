﻿using System;
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
        public ParticleEmitter Emitter;
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
                spriteBatch.Draw(_texture, pos, null, particle.Color, 0f, new Vector2(8, 8), 1f, SpriteEffects.None, 0f);
            }
        }
    }

    public class ParticleEmitter
    {
        private int _maxParticles;

        private bool[] _active;
        private List<int> _stagedIndexes;
        private Stack<int> _freeIndexes;

        private float _spawnRate = 0.1f; //0.33f;
        private float _spawnAccum = 0f;

        private static Random _random = new Random();

        public ParticleEmitter (int maxParticles)
        {
            _maxParticles = maxParticles;

            _active = new bool[maxParticles];
            _stagedIndexes = new List<int>(maxParticles);

            _freeIndexes = new Stack<int>(maxParticles);

            for (int i = 0; i < maxParticles; i++) {
                _freeIndexes.Push(i);
            }

            _position = new Vector2[maxParticles];
            _velocity = new Vector2[maxParticles];
            _color = new Color[maxParticles];
            _lifetime = new float[maxParticles];

            Lifetime = 3;
            LifetimeVariance = 2;
            StartColor = Color.White;
            EndColor = Color.Transparent;
            StartColorVariance = Color.White;
            EmitSpeedVariance = 10f;
        }

        public ParticleEnumerator Particles
        {
            get { return new ParticleEnumerator(this); }
        }

        public void Update (AmphibianGameTime time)
        {
            float elapsed = time.ElapsedGameTime.Milliseconds / 1000f;

            SpawnParticles(elapsed);

            StageUpdate();

            UpdateParticleLifetime(elapsed);
            UpdateParticlePosition(elapsed);
            UpdateParticleColor();

            ExpireParticles();
        }

        private void SpawnParticles (float elapsedTime)
        {
            _spawnAccum += elapsedTime;

            while (_spawnAccum > _spawnRate) {
                _spawnAccum -= _spawnRate;

                InitializeParticle();
            }
        }

        private void InitializeParticle ()
        {
            if (_freeIndexes.Count == 0)
                return;

            int index = _freeIndexes.Pop();
            _active[index] = true;

            InitializeParticleLifetime(index);
            InitializeParticlePosition(index);
            InitializeParticleColor(index);
        }

        private void StageUpdate ()
        {
            _stagedIndexes.Clear();

            for (int i = 0; i < _maxParticles; i++) {
                if (_active[i])
                    _stagedIndexes.Add(i);
            }
        }

        private void ExpireParticles ()
        {
            for (int i = 0; i < _stagedIndexes.Count; i++) {
                int index = _stagedIndexes[i];
                if (_lifetime[index] <= 0) {
                    _freeIndexes.Push(index);
                    _active[index] = false;
                }
            }
        }

        #region Lifetime

        private float _baseLifetime = 1f;
        private float _baseLifetimeDecay = 1f;
        private float _varianceLifetime = 0f;

        private float[] _lifetime = null;
        private float[] _lifetimeDecay = null;

        public float Lifetime
        {
            get { return _baseLifetime; }
            set
            {
                _baseLifetime = value;
                _baseLifetimeDecay = 1f / _baseLifetime;
            }
        }

        public float LifetimeVariance
        {
            get { return _varianceLifetime; }
            set
            {
                _varianceLifetime = value;
                CheckLifetimeVariance();
            }
        }

        private void InitializeParticleLifetime (int index)
        {
            _lifetime[index] = 1f;
            if (_varianceLifetime != 0)
                _lifetimeDecay[index] = 1f / (float)Math.Max(0.001f, _baseLifetime + _random.NextDouble() * _varianceLifetime - (_varianceLifetime / 2f));               
        }

        private void UpdateParticleLifetime (float elapsed)
        {
            if (_varianceLifetime == 0) {
                float decay = _baseLifetimeDecay * elapsed;
                foreach (int index in _stagedIndexes)
                    _lifetime[index] -= decay;
            }
            else {
                foreach (int index in _stagedIndexes)
                    _lifetime[index] -= _lifetimeDecay[index] * elapsed;
            }
        }

        private void CheckLifetimeVariance ()
        {
            if (_varianceLifetime != 0 && _lifetimeDecay == null)
                _lifetimeDecay = new float[_maxParticles];
            else if (_varianceLifetime == 0)
                _lifetimeDecay = null;               
        }

        #endregion

        #region Position

        private float _baseEmitAngle = 0f;
        private float _baseEmitAngleVariance = 360f;
        private float _baseEmitSpeed = 20f;
        private float _baseEmitSpeedVariance = 0f;

        private Vector2[] _position;
        private Vector2[] _velocity;

        public float EmitAngle
        {
            get { return _baseEmitAngle; }
            set { _baseEmitAngle = value; }
        }

        public float EmitAngleVariance
        {
            get { return _baseEmitAngleVariance; }
            set { _baseEmitAngleVariance = value; }
        }

        public float EmitSpeed
        {
            get { return _baseEmitSpeed; }
            set { _baseEmitSpeed = value; }
        }

        public float EmitSpeedVariance
        {
            get { return _baseEmitSpeedVariance; }
            set { _baseEmitSpeedVariance = value; }
        }

        private void InitializeParticlePosition (int index)
        {
            double angle = DegToRad(_baseEmitAngle + (float)_random.NextDouble() * _baseEmitAngleVariance - (_baseEmitAngleVariance / 2f));
            float speed = _baseEmitSpeed + (float)_random.NextDouble() * _baseEmitSpeedVariance - (_baseEmitSpeedVariance / 2f);

            _position[index] = Vector2.Zero;
            _velocity[index] = new Vector2((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed);
        }

        private void UpdateParticlePosition (float elapsed)
        {
            foreach (int index in _stagedIndexes) {
                Vector2 position = _position[index];
                Vector2 velocity = _velocity[index];

                _position[index] = new Vector2(position.X + velocity.X * elapsed, position.Y + velocity.Y * elapsed);
            }
        }

        private float DegToRad (float degrees)
        {
            return (float)Math.PI * degrees / 180f;
        }

        #endregion

        #region Color

        private Color _baseStartColor = Color.White;
        private Color _baseEndColor = Color.Transparent;
        private Color _varianceStartColor;
        private Color _varianceEndColor;
        private bool _useColorVariance;

        private Color[] _color;
        private Color[] _startColor;
        private Color[] _endColor;

        public Color StartColor
        {
            get { return _baseStartColor; }
            set { _baseStartColor = value; }
        }

        public Color EndColor
        {
            get { return _baseEndColor; }
            set { _baseEndColor = value; }
        }

        public Color StartColorVariance
        {
            get { return _varianceStartColor; }
            set
            {
                _varianceStartColor = value;
                CheckColorVarianceState();
            }
        }

        public Color EndColorVariance
        {
            get { return _varianceEndColor; }
            set
            {
                _varianceEndColor = value;
                CheckColorVarianceState();
            }
        }

        private void InitializeParticleColor (int index)
        {
            if (_useColorVariance) {
                Color startColor = new Color(
                    AdjustColorChannel(_baseStartColor.R, _varianceStartColor.R),
                    AdjustColorChannel(_baseStartColor.G, _varianceStartColor.G),
                    AdjustColorChannel(_baseStartColor.B, _varianceStartColor.B),
                    AdjustColorChannel(_baseStartColor.A, _varianceStartColor.A));

                Color endColor = new Color(
                    AdjustColorChannel(_baseEndColor.R, _varianceEndColor.R),
                    AdjustColorChannel(_baseEndColor.G, _varianceEndColor.G),
                    AdjustColorChannel(_baseEndColor.B, _varianceEndColor.B),
                    AdjustColorChannel(_baseEndColor.A, _varianceEndColor.A));

                _color[index] = startColor;
                _startColor[index] = startColor;
                _endColor[index] = endColor;
            }
            else {
                _color[index] = _baseStartColor;
            }
        }

        private void UpdateParticleColor ()
        {
            if (_useColorVariance) {
                foreach (int index in _stagedIndexes)
                    _color[index] = Color.Lerp(_endColor[index], _startColor[index], _lifetime[index]);
            }
            else {
                foreach (int index in _stagedIndexes)
                    _color[index] = Color.Lerp(_baseEndColor, _baseStartColor, _lifetime[index]);
            }
        }

        private void CheckColorVarianceState ()
        {
            _useColorVariance = (_varianceStartColor != Color.Transparent || _varianceEndColor != Color.Transparent);
            if (_useColorVariance && _startColor == null) {
                _startColor = new Color[_maxParticles];
                _endColor = new Color[_maxParticles];
            }
            else if (!_useColorVariance) {
                _startColor = null;
                _endColor = null;
            }
        }

        private byte AdjustColorChannel (byte value, byte variance)
        {
            int adjusted = value + _random.Next(variance) - (variance / 2);
            return (byte)Math.Max(0, Math.Min(255, adjusted));
        }

        #endregion

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
                get
                {
                    int index = _emitter._stagedIndexes[_index];
                    return new ParticleRenderData() {
                        Position = _emitter._position[index],
                        Color = _emitter._color[index],
                    };
                }
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
                return _index < _emitter._stagedIndexes.Count;
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
    }
}
