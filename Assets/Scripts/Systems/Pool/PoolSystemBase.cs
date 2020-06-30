using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PoolSystemBase))]
public abstract class PoolSystemBase : UpdateSystem {
    [SerializeField] 
    protected GameObject BulletPrefab;

    public abstract GameObject Pop();
}