using System.Collections.Generic;

namespace Amphibian.Systems.Rendering.Spine
{
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
}
