using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Amphibian.Systems.Particle
{
    public class StandardEmitter : ParticleEmitter
    {
        private int _maxParticles;

        private bool[] _active;
        private List<int> _stagedIndexes;
        private Stack<int> _freeIndexes;

        private float _spawnAccum = 0f;

        private static Random _random = new Random();

        public StandardEmitter (int maxParticles)
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

            Lifetime = new Range(1f, 5f);
            StartColor = Color.White;
            EndColor = Color.Transparent;
            StartColorVariance = Color.White;
            EmitSpeed = new Range(10f, 40f);
            SpinVelocity = 1f;
            EmitRate = .2f;
        }

        public override int Count
        {
            get { return _stagedIndexes.Count; }
        }

        public override void Update (AmphibianGameTime time)
        {
            float elapsed = time.ElapsedGameTime.Milliseconds / 1000f;

            if (CanEmit)
                SpawnParticles(elapsed);

            StageUpdate();

            UpdateParticleLifetime(elapsed);
            UpdateParticlePosition(elapsed);
            UpdateParticleSpin(elapsed);
            UpdateParticleColor();

            ExpireParticles();
            ExpireEmitter();
        }

        private void SpawnParticles (float elapsedTime)
        {
            _spawnAccum += elapsedTime;
            _accumEmitTime += elapsedTime;

            float rate = _emitRate.Sample();

            while (CanEmit && _spawnAccum > rate) {
                _accumEmitParticles++;
                _spawnAccum -= rate;

                rate = _emitRate.Sample();

                InitializeParticle();
                ExpireEmitter();
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
            InitializeParticleSpin(index);
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

        private void ExpireEmitter ()
        {
            if (_maxEmitTime > 0 && _accumEmitTime >= _maxEmitTime)
                _emitterExpired = true;

            if (_maxEmitParticles > 0 && _accumEmitParticles >= _maxEmitParticles)
                _emitterExpired = true;
        }

        protected override ParticleRenderData RenderData (int index)
        {
            int particleIndex = _stagedIndexes[index];

            return new ParticleRenderData() {
                Position = _position[particleIndex],
                Color = _color[particleIndex],
                Rotation = ParticleSpin(particleIndex),
            };
        }

        #region Emit Rate

        private Range _emitRate = 0f;

        private float _maxEmitTime = 0f;
        private float _accumEmitTime = 0f;

        private int _maxEmitParticles = 0;
        private int _accumEmitParticles = 0;

        private bool _emitterExpired;

        public Range EmitRate
        {
            get { return _emitRate; }
            set { _emitRate = value; }
        }

        public float MaxEmitTime
        {
            get { return _maxEmitTime; }
            set { _maxEmitTime = value; }
        }

        public int MaxEmitCount
        {
            get { return _maxEmitParticles; }
            set { _maxEmitParticles = value; }
        }

        public bool IsExpired
        {
            get { return _emitterExpired; }
        }

        public bool IsAlive
        {
            get { return !IsExpired || _stagedIndexes.Count > 0; }
        }

        private bool CanEmit
        {
            get
            {
                if (IsExpired)
                    return false;
                if (_emitRate.Min <= 0 && _emitRate.Max <= 0 && _maxEmitParticles <= 0)
                    return false;

                return true;
            }
        }


        #endregion

        #region Lifetime

        private Range _baseLifetime = 1f;

        private float _baseLifetimeDecay = 1f;

        private float[] _lifetime = null;
        private float[] _lifetimeDecay = null;

        public Range Lifetime
        {
            get { return _baseLifetime; }
            set
            {
                _baseLifetime = value;
                CheckLifetime();
            }
        }

        private void InitializeParticleLifetime (int index)
        {
            _lifetime[index] = 1f;
            if (_baseLifetime.IsRangedValue)
                _lifetimeDecay[index] = 1f / (float)Math.Max(0.001f, _baseLifetime.Sample());
        }

        private void UpdateParticleLifetime (float elapsed)
        {
            if (_baseLifetime.IsFixedValue) {
                float decay = _baseLifetimeDecay * elapsed;
                foreach (int index in _stagedIndexes)
                    _lifetime[index] -= decay;
            }
            else {
                foreach (int index in _stagedIndexes)
                    _lifetime[index] -= _lifetimeDecay[index] * elapsed;
            }
        }

        private void CheckLifetime ()
        {
            if (_baseLifetime.IsFixedValue)
                _baseLifetimeDecay = 1f / _baseLifetime.Min;

            if (_baseLifetime.IsRangedValue && _lifetimeDecay == null)
                _lifetimeDecay = new float[_maxParticles];
            else if (_baseLifetime.IsFixedValue)
                _lifetimeDecay = null;
        }

        #endregion

        #region Position

        private Range _baseEmitAngle = new Range(0, 2f * (float)Math.PI);
        private Range _baseEmitSpeed = 20f;

        private Vector2[] _position;
        private Vector2[] _velocity;

        public Range EmitAngle
        {
            get { return _baseEmitAngle; }
            set { _baseEmitAngle = value; }
        }

        public Range EmitSpeed
        {
            get { return _baseEmitSpeed; }
            set { _baseEmitSpeed = value; }
        }

        private void InitializeParticlePosition (int index)
        {
            float angle = _baseEmitAngle.Sample();
            float speed = _baseEmitSpeed.Sample();

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

        #endregion

        #region Spin

        private Range _baseSpinAngle = Range.Zero;
        private Range _baseSpinVelocity = Range.Zero;

        private float _groupSpinAngle;

        private float[] _spinAngle;
        private float[] _spinVelocity;

        public Range SpinAngle
        {
            get { return _baseSpinAngle; }
            set
            {
                _baseSpinAngle = value;
                CheckSpinAngle();
            }
        }

        public Range SpinVelocity
        {
            get { return _baseSpinVelocity; }
            set
            {
                _baseSpinVelocity = value;
                CheckSpinVelocity();
            }
        }

        private void InitializeParticleSpin (int index)
        {
            if (_baseSpinVelocity.IsRangedValue) {
                _spinAngle[index] = 0f;
                _spinVelocity[index] = _baseSpinVelocity.Sample();
            }

            if (_baseSpinAngle.IsRangedValue)
                _spinAngle[index] = _baseSpinAngle.Sample();
        }

        private void UpdateParticleSpin (float elapsed)
        {
            if (_baseSpinAngle.IsFixedValue && _baseSpinVelocity.IsFixedValue) {
                _groupSpinAngle += _baseSpinVelocity.Sample() * elapsed;
            }
            else if (_baseSpinVelocity.IsFixedValue) {
                float adjust = _baseSpinVelocity.Sample() * elapsed;
                foreach (int index in _stagedIndexes)
                    _spinAngle[index] += adjust;
            }
            else {
                foreach (int index in _stagedIndexes)
                    _spinAngle[index] += _spinVelocity[index] * elapsed;
            }
        }

        private void CheckSpinAngle ()
        {
            if (_baseSpinAngle.IsRangedValue || _baseSpinVelocity.IsRangedValue) {
                if (_spinAngle == null)
                    _spinAngle = new float[_maxParticles];
            }
            else {
                _spinAngle = null;
                _groupSpinAngle = _baseSpinAngle.Sample();
            }
        }

        private void CheckSpinVelocity ()
        {
            if (_baseSpinVelocity.IsRangedValue) {
                if (_spinAngle == null)
                    _spinAngle = new float[_maxParticles];
                if (_spinVelocity == null)
                    _spinVelocity = new float[_maxParticles];
            }
            else {
                _spinVelocity = null;
                if (_baseSpinAngle.IsFixedValue)
                    _spinAngle = null;
            }
        }

        private float ParticleSpin (int index)
        {
            if (_baseSpinAngle.IsFixedValue && _baseSpinVelocity.IsFixedValue)
                return _groupSpinAngle;
            else
                return _spinAngle[index];
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
    }
}
