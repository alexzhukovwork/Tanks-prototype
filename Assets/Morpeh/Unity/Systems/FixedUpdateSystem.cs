﻿namespace Morpeh {
    using UnityEngine;
    
    public abstract class FixedUpdateSystem : ScriptableObject, IFixedSystem {
        private World world;
        private FilterProvider filter;

        public World World {
            get => this.world;
            set => this.world = value;
        }

        public FilterProvider Filter {
            get => this.filter;
            set => this.filter = value;
        }

        public abstract void OnAwake();
        public virtual void OnStart() {
        }

        public abstract void OnUpdate(float deltaTime);

        public virtual void Dispose() { }
    }
}
