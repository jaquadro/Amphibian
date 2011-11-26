using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRulerLibrary;
using Microsoft.Xna.Framework;

namespace Amphibian.Debug
{
    public static class Performance
    {
        private static TimeRuler _currentRuler;
        private static DebugManager _debugManager;
        private static DebugCommandUI _debugCommandUI;
        private static FpsCounter _fpsCounter;
        private static bool _firstFrame = true;

        public static TimeRuler TimeRuler
        {
            get { return _currentRuler; }
        }

        public static void Initialize (Game g)
        {
            _debugManager = new DebugManager(g);
            g.Components.Add(_debugManager);

            _debugCommandUI = new DebugCommandUI(g);
            _debugCommandUI.DrawOrder = int.MaxValue;
            g.Components.Add(_debugCommandUI);

            _fpsCounter = new FpsCounter(g);
            g.Components.Add(_fpsCounter);

            _currentRuler = new TimeRuler(g);
            g.Components.Add(_currentRuler);
        }

        public static void StartFrame ()
        {
            if (_firstFrame) {
                _firstFrame = false;
                _debugCommandUI.ExecuteCommand("tr on log:on");
                _debugCommandUI.ExecuteCommand("fps on");
            }
            _currentRuler.StartFrame();
        }
    }

    public struct PerformanceRuler : IDisposable
    {
        private int _index;
        private string _name;

        public PerformanceRuler (int index, string name, Color c)
        {
            _index = index;
            _name = name;
            Performance.TimeRuler.BeginMark(_index, _name, c);
        }

        public PerformanceRuler (string name, Color c)
            : this(0, name, c)
        {
        }

        public void Dispose ()
        {
            Performance.TimeRuler.EndMark(_index, _name);
        }
    }

    public static class ADebug
    {
        public static bool RenderCollisionGeometry { get; private set; }

        public static void Initialize (Game g)
        {
            IDebugCommandHost host = g.Services.GetService(typeof(IDebugCommandHost)) as IDebugCommandHost;

            if (host != null) {
                host.RegisterCommand("rendercol", "Render Collision Geometry", RenderCollisionCommand);
            }
        }

        private static void RenderCollisionCommand (IDebugCommandHost host, string command, IList<string> arguments)
        {
            if (arguments.Count == 0) {
                RenderCollisionGeometry = !RenderCollisionGeometry;
            }

            foreach (string arg in arguments) {
                switch (arg.ToLower()) {
                    case "on":
                        RenderCollisionGeometry = true;
                        break;
                    case "off":
                        RenderCollisionGeometry = false;
                        break;
                }
            }
        }
    }
}
