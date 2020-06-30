using System.Collections.Generic;
using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PoolSystem))]
public class PoolSystem : PoolSystemBase {
    [SerializeField] 
    private int PoolSize = 10;
    
    [SerializeField] 
    private float AddPercent = 0.1f;

    private Filter activeFilter;
    private Filter afterCreateBullets;

    private Stack<IEntity> inactiveBullets;

    private Transform parentPool;

    public override void OnAwake()
    {
        InstantiatePool();

        activeFilter = Filter.All.With<InactiveComponent>().With<BulletComponent>().
            Without<PoolComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        for (int i = 0; i < activeFilter.Length; i++) {
            activeFilter.GetEntity(i).AddComponent<PoolComponent>();
            inactiveBullets.Push(activeFilter.GetEntity(i));
        }
    }

    public override GameObject Pop()
    {
        if (inactiveBullets.Count < PoolSize * AddPercent) {
            int countNew = (int)(PoolSize * AddPercent);
            
            CreateObjectInPool(countNew);

            PoolSize += countNew;
        }
        
        var entity = inactiveBullets.Pop();

        entity.RemoveComponent<PoolComponent>();
        entity.RemoveComponent<InactiveComponent>();

        var gameObject = entity.GetComponent<BulletComponent>().Bullet;

        return gameObject;
    }

    private void InstantiatePool()
    {
        var gameObject = new GameObject();
        
        gameObject.name = nameof(PoolSystem);

        parentPool = gameObject.transform;
        
        CreateObjectInPool(PoolSize);
    }
    
    private void CreateObjectInPool(int count)
    {
        inactiveBullets = new Stack<IEntity>();
        
        for (int i = 0; i < count; i++) {
            Instantiate(BulletPrefab, parentPool);
        }

        afterCreateBullets = Filter.All.With<BulletComponent>().Without<InactiveComponent>();

        for (int i = 0; i < afterCreateBullets.Length; i++) {
            afterCreateBullets.GetEntity(i).AddComponent<InactiveComponent>();
            afterCreateBullets.GetEntity(i).AddComponent<PoolComponent>();
            inactiveBullets.Push(afterCreateBullets.GetEntity(i));
        }
    }
}