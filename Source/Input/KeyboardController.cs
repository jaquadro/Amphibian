using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Reflection;

namespace Amphibian.Input
{
    public class KeyboardController<TActionSet> : ButtonController<TActionSet> 
        where TActionSet : struct
    {
        private KeyboardState _state;
        private Dictionary<TActionSet, Keys> _keymap;

        private Dictionary<TActionSet, bool> _held;
        private Dictionary<TActionSet, bool> _pressed;
        private Dictionary<TActionSet, bool> _released;

        public KeyboardController (Dictionary<TActionSet, Keys> map)
        {
            if (!typeof(TActionSet).IsSubclassOf(typeof(Enum))) {
                throw new InvalidOperationException("KeyboardController must be parameterized with an Enum type");
            }

            _state = Keyboard.GetState();

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

        public override void Refresh ()
        {
            _state = Keyboard.GetState();

            foreach (KeyValuePair<TActionSet, Keys> button in _keymap) {
                bool keystate = _state.IsKeyDown(button.Value);
                if (!_held[button.Key] && keystate) {
                    _pressed[button.Key] = true;
                }
                else if (_held[button.Key] && !keystate) {
                    _released[button.Key] = true;
                }
                _held[button.Key] = keystate;
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

        private static List<T> GetValues<T>()
        {
          Type currentEnum = typeof(T);
          List<T> resultSet = new List<T>();
          if (currentEnum.IsEnum)
          {
            FieldInfo[] fields = currentEnum.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo field in fields)
              resultSet.Add((T)field.GetValue(null));
          }

          return resultSet;
        }
    }
}
