using Morpeh;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BulletCollisionSystem))]
public class BulletCollisionSystem : FixedUpdateSystem
{
    protected Filter firstObjectFilter;
    protected Filter secondObjectFilter;
    
    public override void OnAwake()
    {
        firstObjectFilter = InstantiateFirstObjectFilter();
        secondObjectFilter = InstantiateSecondObjectFilter();
    }

    public override void OnUpdate(float deltaTime)
    {
        var secondColliders = secondObjectFilter.Select<Collider2DComponent>();
        var firstColliders = firstObjectFilter.Select<Collider2DComponent>();

        for (int i = 0; i < firstObjectFilter.Length; i++) {
            ref var firstCollider = ref firstColliders.GetComponent(i);

            if (firstCollider.Collsion2D != null) {
                for (int j = 0; j < secondObjectFilter.Length; j++) {
                    if (secondColliders.GetComponent(j).Collider2D.Equals(firstCollider.Collsion2D.otherCollider)) {
                        OnCollision(firstObjectFilter.GetEntity(i), secondObjectFilter.GetEntity(j));
                    }
                }

                firstCollider.Collsion2D = null;
                firstObjectFilter.GetEntity(i).AddComponent<InactiveComponent>();
            }
        }
    }

    private void OnCollision(IEntity first, IEntity second)
    {
        var bullet = first.GetComponent<BulletComponent>();
        var health = second.GetComponent<HealthComponent>();

        ref var bulletCollider = ref first.GetComponent<Collider2DComponent>();
        
        if (health.UnitType != bullet.UnitType) {
            ref var damagedComponent =
                ref second.AddComponent<DamagedComponent>(out _);
            damagedComponent.Bullet = bullet;
            damagedComponent.BulletCollider = bulletCollider;
        }
    }

    private Filter InstantiateFirstObjectFilter()
    {
        Filter filter = Filter.All.With<BulletComponent>().With<TransformComponent>().
            With<Collider2DComponent>().Without<InactiveComponent>();

        return filter;
    }

    private Filter InstantiateSecondObjectFilter()
    {
        Filter filter = Filter.All.With<Collider2DComponent>().With<HealthComponent>().Without<BulletComponent>();
        
        return filter;
    }
}