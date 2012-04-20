using System;

namespace Amphibian.Systems.Rendering.Sprites
{
    public enum Direction
    {
        East = 0,
        NorthEastEast = 1,
        NorthEast = 2,
        NorthNorthEast = 3,
        North = 4,
        NorthNorthWest = 5,
        NorthWest = 6,
        NorthWestWest = 7,
        West = 8,
        SouthWestWest = 9,
        SouthWest = 10,
        SouthSouthWest = 11,
        South = 12,
        SouthSouthEast = 13,
        SouthEast = 14,
        SouthEastEast = 15,
    }

    [Flags]
    public enum DirectionSet
    {
        None = 0x0000,
        East = 0x0001,
        NorthEastEast = 0x0002,
        NorthEast = 0x0004,
        NorthNorthEast = 0x0008,
        North = 0x0010,
        NorthNorthWest = 0x0020,
        NorthWest = 0x0040,
        NorthWestWest = 0x0080,
        West = 0x0100,
        SouthWestWest = 0x0200,
        SouthWest = 0x0400,
        SouthSouthWest = 0x0800,
        South = 0x1000,
        SouthSouthEast = 0x2000,
        SouthEast = 0x4000,
        SouthEastEast = 0x8000,
    }
}
