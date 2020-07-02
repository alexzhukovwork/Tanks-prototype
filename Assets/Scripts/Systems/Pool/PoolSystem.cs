using System.Collections;
using System.Collections.Generic;
using Morpeh;
using Morpeh.Globals;
using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PoolSystem))]
public class PoolSystem : PoolSystemBase {
    [SerializeField] 
    private int PoolSize = 10;
    
    [SerializeField] 
    private float AddPercent = 0.1f;

    [SerializeField] 
    private GlobalEvent StartEvent;
    
    private Filter activeFilter;
    private Filter afterCreateBullets;

    private Stack<IEntity> inactiveBullets;

    private Transform parentPool;

    public override void OnAwake()
    {
        activeFilter = Filter.All.With<InactiveComponent>().With<BulletComponent>().
            Without<PoolComponent>().With<PhotonViewComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (StartEvent.IsPublished)
            InstantiatePool();

        var photonComponents = activeFilter.Select<PhotonViewComponent>();
        
        for (int i = 0; i < activeFilter.Length; i++) {

            if (photonComponents.GetComponent(i).PhotonView.IsMine) {
                activeFilter.GetEntity(i).AddComponent<PoolComponent>();
                inactiveBullets.Push(activeFilter.GetEntity(i));
            }
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
        var gameObject = PhotonNetwork.Instantiate(PoolParent.name, Vector3.zero, Quaternion.identity);
        
        gameObject.name = nameof(PoolSystem);

        parentPool = gameObject.transform;
        
        CreateObjectInPool(PoolSize);
    }
    
    private void CreateObjectInPool(int count)
    {
        inactiveBullets = new Stack<IEntity>();
        
        for (int i = 0; i < count; i++) {
            var bullet = PhotonNetwork.Instantiate(BulletPrefab.name, Vector3.zero, Quaternion.identity);
            bullet.transform.SetParent(parentPool);
        }

        afterCreateBullets = Filter.All.With<BulletComponent>().Without<InactiveComponent>();

        for (int i = 0; i < afterCreateBullets.Length; i++) {
            afterCreateBullets.GetEntity(i).AddComponent<InactiveComponent>();
            afterCreateBullets.GetEntity(i).AddComponent<PoolComponent>();
            inactiveBullets.Push(afterCreateBullets.GetEntity(i));
        }
    }
}