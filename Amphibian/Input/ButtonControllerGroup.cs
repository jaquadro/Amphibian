using System.Collections.Generic;

namespace Amphibian.Input
{
    public class ButtonControllerGroup<TActionSet> : ButtonController<TActionSet>, IEnumerable<ButtonController<TActionSet>>
        where TActionSet : struct
    {
        private List<ButtonController<TActionSet>> _controllers;

        public ButtonControllerGroup (params ButtonController<TActionSet>[] controllers)
        {
            _controllers = new List<ButtonController<TActionSet>>(controllers);
        }

        public override bool ButtonPressed (TActionSet action)
        {
            for (int i = 0; i < _controllers.Count; i++)
                if (_controllers[i].ButtonPressed(action))
                    return true;
            return false;
        }

        public override bool ButtonReleased (TActionSet action)
        {
            for (int i = 0; i < _controllers.Count; i++)
                if (_controllers[i].ButtonReleased(action))
                    return true;
            return false;
        }

        public override bool ButtonHeld (TActionSet action)
        {
            for (int i = 0; i < _controllers.Count; i++)
                if (_controllers[i].ButtonHeld(action))
                    return true;
            return false;
        }

        public override void Refresh ()
        {
            for (int i = 0; i < _controllers.Count; i++)
                _controllers[i].Refresh();
        }

        public override void Reset ()
        {
            for (int i = 0; i < _controllers.Count; i++)
                _controllers[i].Reset();
        }

        #region IEnumerable<ButtonController<TActionSet>> Members

        public IEnumerator<ButtonController<TActionSet>> GetEnumerator ()
        {
            return _controllers.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
        {
            return _controllers.GetEnumerator();
        }

        #endregion
    }
}
