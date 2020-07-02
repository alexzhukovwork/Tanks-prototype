using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BulletCollisionSystem))]
public class BulletCollisionSystem : CollisionSystem {
    protected override void OnCollision(IEntity first, IEntity second)
    {
        var bullet = first.GetComponent<BulletComponent>();
        var health = second.GetComponent<HealthComponent>();

        if (health.UnitType != bullet.UnitType) {
            ref var damagedComponent =
                ref second.AddComponent<DamagedComponent>(out _);
            damagedComponent.Bullet = bullet;
        }
    }

    protected override Filter InstantiateFirstObjectFilter()
    {
        Filter filter = Filter.All.With<BulletComponent>().With<TransformComponent>().
            With<Collider2DComponent>().Without<InactiveComponent>();

        return filter;
    }

    protected override Filter InstantiateSecondObjectFilter()
    {
        Filter filter = Filter.All.With<Collider2DComponent>().With<HealthComponent>();
        
        return filter;
    }
}