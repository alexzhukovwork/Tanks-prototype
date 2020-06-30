using Morpeh;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BulletCollisionSystem))]
public class BulletCollisionSystem : FixedUpdateSystem
{
    private Filter bulletFilter;
    private Filter objectFilter;
    
    public override void OnAwake()
    {
        bulletFilter = Filter.All.With<BulletComponent>().With<TransformComponent>().
            With<Collider2DComponent>().Without<InactiveComponent>();

        objectFilter = Filter.All.With<Collider2DComponent>().With<HealthComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var objectColliders = objectFilter.Select<Collider2DComponent>();
        var objectHealth = objectFilter.Select<HealthComponent>();
        var bulletColliders = bulletFilter.Select<Collider2DComponent>();
        var bullets = bulletFilter.Select<BulletComponent>();

        for (int i = 0; i < bulletFilter.Length; i++) {
            var bulletCollider = bulletColliders.GetComponent(i);
            var bullet = bullets.GetComponent(i);
            
            Collider2D [] colliders = new Collider2D[1];
            
            if (bulletCollider.Collider2D.GetContacts(new ContactFilter2D(), colliders) > 0) {

                for (int j = 0; j < objectFilter.Length; j++) {
                    
                    if (objectColliders.GetComponent(j).Collider2D.Equals(colliders[0])) {
                        ref var health = ref objectHealth.GetComponent(j);

                        if (health.UnitType != bullet.UnitType) {
                            ref var damagedComponent =
                                ref objectFilter.GetEntity(j).AddComponent<DamagedComponent>(out _);
                            damagedComponent.Bullet = bullet;
                        }
                    }
                }
                
                bulletFilter.GetEntity(i).AddComponent<InactiveComponent>();
            }
        }

    }
}