﻿namespace Morpeh {
#if UNITY_EDITOR && ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    using UnityEngine;

    public abstract class MonoProvider<T> : EntityProvider where T : struct, IComponent {
        [SerializeField, HideInInspector] private T serializedData;
#if UNITY_EDITOR && ODIN_INSPECTOR
        private string typeName = typeof(T).Name;
        [PropertySpace, ShowInInspector, PropertyOrder(1), LabelText("$typeName")]
#endif
        private T Data {
            get {
                if (this.Entity != null) {
                    return this.Entity.GetComponent<T>(out _);
                }
                return this.serializedData;
            }
            set {
                if (this.Entity != null) {
                    this.Entity.SetComponent(value);
                }
                else {
                    this.serializedData = value;
                }
            }
        }

        public ref T GetData(out bool existOnEntity) {
            if (this.Entity != null) {

                return ref this.Entity.GetComponent<T>(out existOnEntity);
            }
            existOnEntity = false;
            return ref this.serializedData;
        }

        protected override void PreInitialize()
        {
            this.Entity.SetComponent(this.serializedData);
        }
    }
}