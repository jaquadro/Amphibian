using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace Amphibian.Input
{
    public enum GamePadInput
    {
        A,
        B,
        X,
        Y,
        Back,
        Start,
        LeftShoulder,
        RightShoulder,
        BigButton,
        DPadUp,
        DPadDown,
        DPadLeft,
        DPadRight,
        LeftStick,
        LeftStickUp,
        LeftStickDown,
        LeftStickLeft,
        LeftStickRight,
        RightStick,
        RightStickUp,
        RightStickDown,
        RightStickLeft,
        RightStickRight,
        LeftTrigger,
        RightTrigger,
    }

    public class GamePadInputEquality : IEqualityComparer<GamePadInput>
    {
        private static GamePadInputEquality _instance;

        public static GamePadInputEquality Default
        {
            get
            {
                if (_instance == null)
                    _instance = new GamePadInputEquality();
                return _instance;
            }
        }

        public bool Equals (GamePadInput val1, GamePadInput val2)
        {
            return val1 == val2;
        }

        public int GetHashCode (GamePadInput val)
        {
            return ((int)val).GetHashCode();
        }
    }

    public class GamePadController<TActionSet> : ButtonController<TActionSet>
        where TActionSet : struct
    {
        private static Dictionary<GamePadInput, GamePadInput> _identityMap;

        private GamePadState _state;
        private Dictionary<TActionSet, GamePadInput> _keymap;

        private Dictionary<TActionSet, bool> _held;
        private Dictionary<TActionSet, bool> _pressed;
        private Dictionary<TActionSet, bool> _released;

        private PlayerIndex _player;
        private float _stickTolerance = 0.70f;
        private float _triggerTolerance = 0.50f;

        static GamePadController ()
        {
            _identityMap = new Dictionary<GamePadInput, GamePadInput>(GamePadInputEquality.Default);

            foreach (GamePadInput button in GetValues<GamePadInput>()) {
                _identityMap[button] = button;
            }
        }

        public GamePadController (Dictionary<TActionSet, GamePadInput> map, PlayerIndex player)
        {
            if (!typeof(TActionSet).IsSubclassOf(typeof(Enum))) {
                throw new InvalidOperationException("GamePadController must be parameterized with an Enum type");
            }

            _state = GamePad.GetState(player);
            _player = player;

            _keymap = map;
            _held = new Dictionary<TActionSet, bool>(map.Comparer);
            _pressed = new Dictionary<TActionSet, bool>(map.Comparer);
            _released = new Dictionary<TActionSet, bool>(map.Comparer);

            foreach (TActionSet button in GetValues<TActionSet>()) {
                _held[button] = false;
                _pressed[button] = false;
                _released[button] = false;
            }
        }

        public static Dictionary<GamePadInput, GamePadInput> IdentityMap
        {
            get { return _identityMap; }
        }

        public override void Refresh ()
        {
            _state = GamePad.GetState(_player);

            foreach (KeyValuePair<TActionSet, GamePadInput> button in _keymap) {
                bool keystate = GetButtonState(button.Value) == ButtonState.Pressed;
                if (!_held[button.Key] && keystate) {
                    _pressed[button.Key] = true;
                }
                else if (_held[button.Key] && !keystate) {
                    _released[button.Key] = true;
                }
                _held[button.Key] = keystate;
            }
        }

        private ButtonState GetButtonState (GamePadInput button)
        {
            switch (button) {
                case GamePadInput.A:
                    return _state.Buttons.A;
                case GamePadInput.B:
                    return _state.Buttons.B;
                case GamePadInput.X:
                    return _state.Buttons.X;
                case GamePadInput.Y:
                    return _state.Buttons.Y;
                case GamePadInput.Back:
                    return _state.Buttons.Back;
                case GamePadInput.Start:
                    return _state.Buttons.Start;
                case GamePadInput.LeftShoulder:
                    return _state.Buttons.LeftShoulder;
                case GamePadInput.RightShoulder:
                    return _state.Buttons.RightShoulder;
                case GamePadInput.BigButton:
                    return _state.Buttons.BigButton;
                case GamePadInput.LeftStick:
                    return _state.Buttons.LeftStick;
                case GamePadInput.RightStick:
                    return _state.Buttons.RightStick;
                case GamePadInput.DPadUp:
                    return _state.DPad.Up;
                case GamePadInput.DPadDown:
                    return _state.DPad.Down;
                case GamePadInput.DPadLeft:
                    return _state.DPad.Left;
                case GamePadInput.DPadRight:
                    return _state.DPad.Right;
                case GamePadInput.LeftStickUp:
                    return _state.ThumbSticks.Left.Y < (0 - _stickTolerance) ? ButtonState.Pressed : ButtonState.Released;
                case GamePadInput.LeftStickDown:
                    return _state.ThumbSticks.Left.Y > _stickTolerance ? ButtonState.Pressed : ButtonState.Released;
                case GamePadInput.LeftStickLeft:
                    return _state.ThumbSticks.Left.X < (0 - _stickTolerance) ? ButtonState.Pressed : ButtonState.Released;
                case GamePadInput.LeftStickRight:
                    return _state.ThumbSticks.Left.X > _stickTolerance ? ButtonState.Pressed : ButtonState.Released;
                case GamePadInput.RightStickUp:
                    return _state.ThumbSticks.Right.Y < (0 - _stickTolerance) ? ButtonState.Pressed : ButtonState.Released;
                case GamePadInput.RightStickDown:
                    return _state.ThumbSticks.Right.Y > _stickTolerance ? ButtonState.Pressed : ButtonState.Released;
                case GamePadInput.RightStickLeft:
                    return _state.ThumbSticks.Right.X < (0 - _stickTolerance) ? ButtonState.Pressed : ButtonState.Released;
                case GamePadInput.RightStickRight:
                    return _state.ThumbSticks.Right.X > _stickTolerance ? ButtonState.Pressed : ButtonState.Released;
                case GamePadInput.LeftTrigger:
                    return _state.Triggers.Left > _triggerTolerance ? ButtonState.Pressed : ButtonState.Released;
                case GamePadInput.RightTrigger:
                    return _state.Triggers.Right > _triggerTolerance ? ButtonState.Pressed : ButtonState.Released;
                default:
                    return ButtonState.Released;
            }
        }

        public override void Reset ()
        {
            foreach (TActionSet button in _keymap.Keys) {
                _pressed[button] = false;
                _released[button] = false;
            }
        }

        public override bool ButtonPressed (TActionSet action)
        {
            return _pressed[action];
        }

        public override bool ButtonReleased (TActionSet action)
        {
            return _released[action];
        }

        public override bool ButtonHeld (TActionSet action)
        {
            return _held[action];
        }

        private static List<T> GetValues<T> ()
        {
            Type currentEnum = typeof(T);
            List<T> resultSet = new List<T>();
            if (currentEnum.IsEnum) {
                FieldInfo[] fields = currentEnum.GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (FieldInfo field in fields)
                    resultSet.Add((T)field.GetValue(null));
            }

            return resultSet;
        }
    }
}
