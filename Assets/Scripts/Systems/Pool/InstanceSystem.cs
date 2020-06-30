using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InstanceSystem))]
public class InstanceSystem : PoolSystemBase
{
    private Filter filter;
    
    public override GameObject Pop()
    {
        return Instantiate(BulletPrefab);
    }

    public override void OnAwake()
    {
        filter = Filter.All.With<InactiveComponent>().With<BulletComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var bullets = filter.Select<BulletComponent>();
        
        for (int i = 0; i < filter.Length; i++) {
            var bullet = bullets.GetComponent(i);
            
            Destroy(bullet.Bullet);
            filter.GetEntity(i).Dispose();
        }
    }
}