using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PoolSystemBase))]
public abstract class PoolSystemBase : UpdateSystem {
    [SerializeField] 
    protected GameObject BulletPrefab;

    [SerializeField] 
    protected GameObject PoolParent;
    
    public abstract GameObject Pop();
}