using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian
{
    public interface IGameService
    {
        bool IsMouseVisible { get; set; }
    }

    public class GameServiceAdapter : IGameService
    {
        public virtual bool IsMouseVisible
        {
            get { return false; }
            set { }
        }
    }
}
