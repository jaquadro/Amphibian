using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework.Input.Touch;

namespace Amphibian.Input
{
    public enum TouchInput
    {
        Primary,
    }

    public class TouchInputEquality : IEqualityComparer<TouchInput>
    {
        private static TouchInputEquality _instance;

        public static TouchInputEquality Default
        {
            get
            {
                if (_instance == null)
                    _instance = new TouchInputEquality();
                return _instance;
            }
        }

        public bool Equals (TouchInput val1, TouchInput val2)
        {
            return val1 == val2;
        }

        public int GetHashCode (TouchInput val)
        {
            return ((int)val).GetHashCode();
        }
    }

    public class TouchController<TActionSet> : CursorController<TActionSet>
        where TActionSet : struct
    {
        private static Dictionary<TouchInput, TouchInput> _identityMap;

        TouchCollection _state;
        private Dictionary<TActionSet, TouchInput> _touchmap;

        private Dictionary<TActionSet, bool> _held;
        private Dictionary<TActionSet, bool> _pressed;
        private Dictionary<TActionSet, bool> _released;

        static TouchController ()
        {
            _identityMap = new Dictionary<TouchInput, TouchInput>(TouchInputEquality.Default);

            foreach (TouchInput input in GetValues<TouchInput>()) {
                _identityMap[input] = input;
            }
        }

        public static Dictionary<TouchInput, TouchInput> IdentityMap
        {
            get { return _identityMap; }
        }

        public TouchController (Dictionary<TActionSet, TouchInput> map)
        {
            if (!typeof(TActionSet).IsSubclassOf(typeof(Enum))) {
                throw new InvalidOperationException("TouchController must be parameterized with an Enum type");
            }

            _state = TouchPanel.GetState();

            _touchmap = map;
            _held = new Dictionary<TActionSet, bool>(map.Comparer);
            _pressed = new Dictionary<TActionSet, bool>(map.Comparer);
            _released = new Dictionary<TActionSet, bool>(map.Comparer);

            foreach (TActionSet button in GetValues<TActionSet>()) {
                _held[button] = false;
                _pressed[button] = false;
                _released[button] = false;
            }
        }

        public override void Refresh ()
        {
            base.Refresh();

            _state = TouchPanel.GetState();

            foreach (KeyValuePair<TActionSet, TouchInput> touch in _touchmap) {
                bool touchstate = (GetTouchState(touch.Value) & (TouchLocationState.Pressed | TouchLocationState.Moved)) != 0;
                if (!_held[touch.Key] && touchstate) {
                    _pressed[touch.Key] = true;
                }
                else if (_held[touch.Key] && !touchstate) {
                    _released[touch.Key] = true;
                }
                _held[touch.Key] = touchstate;
            }
        }

        private TouchLocationState GetTouchState (TouchInput touch)
        {
            switch (touch) {
                case TouchInput.Primary:
                    return _state.Count == 0 ? TouchLocationState.Invalid : _state[0].State;
                default:
                    return TouchLocationState.Invalid;
            }
        }

        public override void Reset ()
        {
            foreach (TActionSet touch in _touchmap.Keys) {
                _pressed[touch] = false;
                _released[touch] = false;
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

        public override int X
        {
            get { return !StateEngaged(0) ? 0 : (int)_state[0].Position.X; }
        }

        public override int Y
        {
            get { return !StateEngaged(0) ? 0 : (int)_state[0].Position.Y; }
        }

        public override bool LocationValid
        {
            get { return StateEngaged(0); }
        }

        public override bool VirtualInterface
        {
            get { return true; }
        }

        private bool StateEngaged (int index)
        {
            if (_state.Count <= index)
                return false;
            return (_state[index].State & (TouchLocationState.Pressed | TouchLocationState.Moved)) != 0;
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
