using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Amphibian.Input
{
    public class KeyboardController<TActionSet> : ButtonController<TActionSet> 
        where TActionSet : struct
    {
        private KeyboardState _state;
        private Dictionary<TActionSet, Keys> _keymap;

        private Dictionary<TActionSet, bool> _held;
        private Dictionary<TActionSet, bool> _pressed;

        public KeyboardController (Dictionary<TActionSet, Keys> map)
        {
            if (!typeof(TActionSet).IsSubclassOf(typeof(Enum))) {
                throw new InvalidOperationException("KeyboardController must be parameterized with an Enum type");
            }

            _state = Keyboard.GetState();

            _keymap = map;
            _held = new Dictionary<TActionSet, bool>();
            _pressed = new Dictionary<TActionSet, bool>();

            foreach (TActionSet button in Enum.GetValues(typeof(TActionSet))) {
                _held[button] = false;
                _pressed[button] = false;
            }
        }

        public override void Refresh ()
        {
            _state = Keyboard.GetState();

            foreach (KeyValuePair<TActionSet, Keys> button in _keymap) {
                bool keystate = _state.IsKeyDown(button.Value);
                if (_held[button.Key] && !keystate) {
                    _pressed[button.Key] = true;
                }
                _held[button.Key] = keystate;
            }
        }

        public override void Reset ()
        {
            foreach (TActionSet button in _keymap.Keys) {
                _pressed[button] = false;
            }
        }

        public override bool ButtonPressed (TActionSet action)
        {
            return _pressed[action];
        }

        public override bool ButtonHeld (TActionSet action)
        {
            return _held[action];
        }
    }
}
