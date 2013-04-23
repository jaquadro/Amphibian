using System.Collections.Generic;

namespace Amphibian.Systems.Rendering.Common
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
}
