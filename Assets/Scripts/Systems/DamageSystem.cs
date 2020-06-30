using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DamageSystem))]
public class DamageSystem : UpdateSystem
{
    private Filter filter;
    
    public override void OnAwake()
    {
        filter = Filter.All.With<DamagedComponent>().With<HealthComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var healthComponents = filter.Select<HealthComponent>();
        var damagedComponents = filter.Select<DamagedComponent>();

        for (int i = 0; i < filter.Length; i++) {
            ref var health = ref healthComponents.GetComponent(i);
            ref var damage = ref damagedComponents.GetComponent(i);

            health.Health -= damage.Bullet.Damage;
            
            if (health.Health <= 0)
                health.GameObject.SetActive(false);

            filter.GetEntity(i).RemoveComponent<DamagedComponent>();
        }
    }
}