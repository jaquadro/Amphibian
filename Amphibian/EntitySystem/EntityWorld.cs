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
        private TagManager _tagManager;

        private AmphibianGameTime _gameTime;
        private EntityFrame _frame;

        public EntityWorld ()
        {
            _systemManager = new SystemManager(this);
            _entityManager = new EntityManager(this);
            _tagManager = new TagManager(this);
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

        public TagManager TagManager
        {
            get { return _tagManager; }
        }

        public AmphibianGameTime GameTime
        {
            get { return _gameTime; }
            set { _gameTime = value; }
        }

        public EntityFrame Frame
        {
            get { return _frame; }
            set { _frame = value; }
        }
    }
}
