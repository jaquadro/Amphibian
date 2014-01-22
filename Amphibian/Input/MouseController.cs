using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework.Input;

namespace Amphibian.Input
{
    public enum MouseInput
    {
        Left,
        Middle,
        Right,
        X1,
        X2,
    }

    public class MouseInputEquality : IEqualityComparer<MouseInput>
    {
        private static MouseInputEquality _instance;

        public static MouseInputEquality Default
        {
            get
            {
                if (_instance == null)
                    _instance = new MouseInputEquality();
                return _instance;
            }
        }

        public bool Equals(MouseInput val1, MouseInput val2)
        {
            return val1 == val2;
        }

        public int GetHashCode(MouseInput val)
        {
            return ((int)val).GetHashCode();
        }
    }

    public class MouseController<TActionSet> : CursorController<TActionSet>
        where TActionSet : struct
    {
        private static Dictionary<MouseInput, MouseInput> _identityMap;

        private MouseState _state;
        private Dictionary<TActionSet, MouseInput> _keymap;

        private Dictionary<TActionSet, bool> _held;
        private Dictionary<TActionSet, bool> _pressed;
        private Dictionary<TActionSet, bool> _released;

        static MouseController()
        {
            _identityMap = new Dictionary<MouseInput, MouseInput>(MouseInputEquality.Default);

            foreach (MouseInput button in GetValues<MouseInput>()) {
                _identityMap[button] = button;
            }
        }

        public static Dictionary<MouseInput, MouseInput> IdentityMap
        {
            get { return _identityMap; }
        }

        public MouseController(Dictionary<TActionSet, MouseInput> map)
        {
            if (!typeof(TActionSet).IsSubclassOf(typeof(Enum))) {
                throw new InvalidOperationException("MouseController must be parameterized with an Enum type");
            }

            _state = Mouse.GetState();

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

        public override void Refresh()
        {
            base.Refresh();

            _state = Mouse.GetState();

            foreach (KeyValuePair<TActionSet, MouseInput> button in _keymap) {
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

        private ButtonState GetButtonState(MouseInput button)
        {
            switch (button) {
                case MouseInput.Left:
                    return _state.LeftButton;
                case MouseInput.Middle:
                    return _state.MiddleButton;
                case MouseInput.Right:
                    return _state.RightButton;
                case MouseInput.X1:
                    return _state.XButton1;
                case MouseInput.X2:
                    return _state.XButton2;
                default:
                    return ButtonState.Released;
            }
        }

        public override void Reset()
        {
            foreach (TActionSet button in _keymap.Keys) {
                _pressed[button] = false;
                _released[button] = false;
            }
        }

        public override bool ButtonPressed(TActionSet action)
        {
            return _pressed[action];
        }

        public override bool ButtonReleased(TActionSet action)
        {
            return _released[action];
        }

        public override bool ButtonHeld(TActionSet action)
        {
            return _held[action];
        }

        public override int X
        {
            get { return _state.X; }
        }

        public override int Y
        {
            get { return _state.Y; }
        }

        public override bool LocationValid
        {
            get { return Mouse.WindowHandle != null; }
        }

        public override bool VirtualInterface
        {
            get { return true; }
        }

        private static List<T> GetValues<T>()
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
