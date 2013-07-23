using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.Input
{
    [Flags]
    public enum ControllerType
    {
        None = 0x0,
        Keyboard = 0x1,
        Mouse = 0x2,
        Gamepad = 0x4,
    };
}
