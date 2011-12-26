using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphibian.EntitySystem
{
    public class EntityWorld
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;

        private AmphibianGameTime _gameTime;

        public EntityWorld ()
        {
            _systemManager = new SystemManager(this);
            _entityManager = new EntityManager(this);
            _gameTime = new AmphibianGameTime();
        }

        public SystemManager SystemManager
        {
            get { return _systemManager; }
        }

        public EntityManager EntityManager
        {
            get { return _entityManager; }
        }

        public AmphibianGameTime GameTime
        {
            get { return _gameTime; }
            set { _gameTime = value; }
        }
    }
}
