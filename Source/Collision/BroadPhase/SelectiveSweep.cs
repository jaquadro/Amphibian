using System;
using System.Collections.Generic;
using Amphibian.Geometry;

namespace Amphibian.Collision.BroadPhase
{
    using TScaler = FPInt;

    public enum AxisStrategy
    {
        Auto,
        UseXAxis,
        UseYAxis,
    }

    public class SelectiveSweep
    {
        private enum SelectedAxis {
            X,
            Y,
        }

        private static MarkerComparer _markCompare = new MarkerComparer();

        private List<Element> _elements;
        private List<Marker> _xMarkers;
        private List<Marker> _yMarkers;

        private LinkedList<Element> _currentElems;

        public event Action<ICollidable, ICollidable> Collision;

        private sealed class MarkerComparer : IComparer<Marker>
        {
            public int Compare (Marker left, Marker right)
            {
                if (left.value < right.value)
                    return -1;
                if (left.value > right.value)
                    return 1;
                if (left == right)
                    return 0;
                return left.start ? -1 : 1;
            }
        }

        private sealed class Marker
        {
            public Element element;
            public bool start;
            public TScaler value;

            public Marker (Element element, bool start)
            {
                this.element = element;
                this.start = start;
            }
        }

        private sealed class Element
        {
            public LinkedListNode<Element> node;
            public ICollidable cObject;
            public TScaler min;
            public TScaler max;
            private Marker xStart;
            private Marker xEnd;
            private Marker yStart;
            private Marker yEnd;

            public Element (ICollidable cObject)
            {
                this.cObject = cObject;
                node = new LinkedListNode<Element>(this);
                xStart = new Marker(this, true);
                xEnd = new Marker(this, false);
                yStart = new Marker(this, true);
                yEnd = new Marker(this, false);
            }

            public void AddMarkers (List<Marker> xMarkers, List<Marker> yMarkers)
            {
                xMarkers.Add(xStart);
                xMarkers.Add(xEnd);

                yMarkers.Add(yStart);
                yMarkers.Add(yEnd);
            }

            public void Update ()
            {
                RectangleFP bounds = cObject.CollisionMask.Bounds;

                xStart.value = bounds.X;
                xEnd.value = bounds.X + bounds.Width;

                yStart.value = bounds.Y;
                yEnd.value = bounds.Y + bounds.Height;
            }

            public void SetX ()
            {
                min = xStart.value;
                max = xEnd.value;
            }

            public void SetY ()
            {
                min = yStart.value;
                max = yEnd.value;
            }
        }

        public SelectiveSweep ()
        {
            _elements = new List<Element>();
            _xMarkers = new List<Marker>();
            _yMarkers = new List<Marker>();

            _currentElems = new LinkedList<Element>();
        }

        protected virtual void OnCollision (ICollidable first, ICollidable second)
        {
            if (Collision != null) {
                Collision(first, second);
            }
        }

        public void AddCollidable (ICollidable obj)
        {
            Element element = new Element(obj);
            element.AddMarkers(_xMarkers, _yMarkers);

            _elements.Add(element);
        }

        public void RemoveCollidable (ICollidable obj)
        {
            for (int i = 0; i < _elements.Count; i++)
                if (_elements[i].cObject == obj)
                    _elements.RemoveAt(i--);
            for (int i = 0; i < _xMarkers.Count; i++)
                if (_xMarkers[i].element.cObject == obj)
                    _xMarkers.RemoveAt(i--);
            for (int i = 0; i < _yMarkers.Count; i++)
                if (_yMarkers[i].element.cObject == obj)
                    _yMarkers.RemoveAt(i--);
            //_elements.RemoveAll(v => v.cObject == obj);
            //_xMarkers.RemoveAll(v => v.element.cObject == obj);
            //_yMarkers.RemoveAll(v => v.element.cObject == obj);
        }

        public void Clear ()
        {
            _elements.Clear();
            _xMarkers.Clear();
            _yMarkers.Clear();
        }

        private void Update ()
        {
            for (int i = 0; i < _elements.Count; i++) {
                _elements[i].Update();
            }
            _xMarkers.Sort(_markCompare);
            _yMarkers.Sort(_markCompare);
        }

        private SelectedAxis ChooseAxis ()
        {
            int xCount = 0;
            int xDepth = 0;
            int yCount = 0;
            int yDepth = 0;

            for (int i = 0; i < _xMarkers.Count; i++) {
                if (_xMarkers[i].start)
                    xCount += xDepth++;
                else
                    xDepth--;

                if (_yMarkers[i].start)
                    yCount += yDepth++;
                else
                    yDepth--;
            }

            return (xCount < yCount)
                ? SelectedAxis.X
                : SelectedAxis.Y;
        }

        public void Detect ()
        {
            Update();
            Detect(ChooseAxis());
        }

        private void Detect (SelectedAxis axis)
        {
            List<Marker> markers = (axis == SelectedAxis.X)
                ? _xMarkers : _yMarkers;

            for (int i = 0; i < markers.Count; i++) {
                Marker marker = markers[i];
                Element elem1 = marker.element;

                if (marker.start) {
                    if (axis == SelectedAxis.X)
                        elem1.SetY();
                    else
                        elem1.SetX();

                    ICollidable obj1 = elem1.cObject;
                    for (LinkedListNode<Element> node = _currentElems.First; node != null; node = node.Next) {
                        Element elem2 = node.Value;
                        ICollidable obj2 = elem2.cObject;

                        if (elem1.min <= elem2.max &&
                            elem2.min <= elem1.max &&
                            obj1.CollisionMask.TestOverlap(obj2.CollisionMask)) {
                                OnCollision(obj1, obj2);
                        }
                    }
                    _currentElems.AddLast(elem1.node);
                }
                else {
                    _currentElems.Remove(elem1.node);
                }
            }

            _currentElems.Clear();
        }


    }
}
