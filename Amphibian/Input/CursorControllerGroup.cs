using System.Collections.Generic;

namespace Amphibian.Input
{
    public class CursorControllerGroup<TActionSet> : CursorController<TActionSet>, IEnumerable<CursorController<TActionSet>>
        where TActionSet : struct
    {
        private List<CursorController<TActionSet>> _controllers;

        public CursorControllerGroup (params CursorController<TActionSet>[] controllers)
        {
            _controllers = new List<CursorController<TActionSet>>(controllers);
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

        public override bool VirtualInterface
        {
            get { return true; }
        }

        public override int X
        {
            get
            {
                for (int i = 0; i < _controllers.Count; i++)
                    if (_controllers[i].LocationValid)
                        return _controllers[i].X;
                return 0;
            }
        }

        public override int Y
        {
            get
            {
                for (int i = 0; i < _controllers.Count; i++)
                    if (_controllers[i].LocationValid)
                        return _controllers[i].Y;
                return 0;
            }
        }

        public override bool LocationValid
        {
            get
            {
                for (int i = 0; i < _controllers.Count; i++)
                    if (_controllers[i].LocationValid)
                        return true;
                return false;
            }
        }

        #region IEnumerable<CursorController<TActionSet>> Members

        public IEnumerator<CursorController<TActionSet>> GetEnumerator ()
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
