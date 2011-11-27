using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TimeRulerLibrary;

namespace Amphibian.Debug
{
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
