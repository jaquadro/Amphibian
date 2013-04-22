﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Amphibian.Systems.Rendering.Sprites
{
    public interface IActivityElement
    {
        string Name { get; set; }
        string Animation { get; set; }
    }

    public interface IActivityMapElement
    {
        string DefaultAnimation { get; set; }
        IList<IActivityElement> Activities { get; set; }
    }

    public interface ISpineAnimationMapElement
    {
        IList<ISpineAnimationSetElement> Animations { get; set; }
    }

    public interface ISpineAnimationSetElement
    {
        string Name { get; set; }
        string DefaultAnimation { get; set; }
        IList<ISpineDirectionElement> Directions { get; set; }
    }

    public interface ISpineDirectionElement
    {
        string Value { get; set; }
        string Animation { get; set; }
        bool FlipX { get; set; }
        bool FlipY { get; set; }
    }

    public interface ISpineAtlasElement
    {
        string Source { get; set; }
    }

    public interface ISpineSkeletonElement
    {
        string Source { get; set; }
    }

    public interface IAnimationElement
    {
        int Repeat { get; set; }
        IList<IFrameElement> Frames { get; set; }
    }

    public interface IAnimationSetElement
    {
        string Name { get; set; }
        IList<IDirectionElement> Directions { get; set; }
    }

    public interface IDirectionElement
    {
        string Value { get; set; }
        IAnimationElement Animation { get; set; }
    }

    public interface IFrameElement
    {
        string Sprite { get; set; }
        float Duration { get; set; }
    }

    public interface ISpriteElement
    {
        string Name { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        int OriginX { get; set; }
        int OriginY { get; set; }
    }

    public interface ISpriteListElement
    {
        string Source { get; set; }
        IList<ISpriteElement> Sprites { get; set; }
    }

    public interface ITransformElement
    {
        float Scale { get; set; }
        float Rotation { get; set; }
        float Opacity { get; set; }
        Color BlendColor { get; set; }
    }
}
