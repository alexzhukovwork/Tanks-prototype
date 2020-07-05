using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DamageBoosterSystem))]
public class DamageBoosterSystem : BoosterTriggerSystem
{
    private Filter playerWithDamageBooster;
    
    public override void OnAwake()
    {
        base.OnAwake();

        playerWithDamageBooster =
            Filter.All.With<PlayerComponent>().With<MovementComponent>().With<DamageBoosterComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        var weaponComponents = playerWithDamageBooster.Select<WeaponComponent>();
        var damageBoosterComponents = playerWithDamageBooster.Select<DamageBoosterComponent>();

        for (int i = 0; i < playerWithDamageBooster.Length; i++) {
            ref var weaponComponent = ref weaponComponents.GetComponent(i);
            ref var damageBoosterComponent = ref damageBoosterComponents.GetComponent(i);

            damageBoosterComponent.CurrentTime += deltaTime;

            if (damageBoosterComponent.CurrentTime > damageBoosterComponent.AllTime) {
                weaponComponent.Damage = damageBoosterComponent.OldDamage;
                playerWithDamageBooster.GetEntity(i).RemoveComponent<DamageBoosterComponent>();
            }
        }
    }
    
    protected override void OnTrigger(IEntity first, IEntity second)
    {
        var damageBooster = first.GetComponent<DamageBoosterComponent>();

        ref var damageBoosterComponent = ref second.AddComponent<DamageBoosterComponent>();
        ref var weaponComponent = ref second.GetComponent<WeaponComponent>();

        damageBooster.This.SetActive(false);
        
        damageBoosterComponent.AllTime = damageBooster.AllTime;
        damageBoosterComponent.CurrentTime = damageBooster.CurrentTime;
        damageBoosterComponent.NewDamage = damageBooster.NewDamage;
        damageBoosterComponent.OldDamage = weaponComponent.Damage;

        weaponComponent.Damage = damageBoosterComponent.NewDamage;

        first.RemoveComponent<InactiveComponent>();
    }

    protected override Filter InstantiateFirstObjectFilter()
    {
        Filter filter = Filter.All.With<DamageBoosterComponent>().
            With<Collider2DComponent>().With<BoosterComponent>();

        return filter;
    }

    protected override Filter InstantiateSecondObjectFilter()
    {
        Filter filter = Filter.All.With<Collider2DComponent>().With<PlayerComponent>().With<WeaponComponent>();
            
        return filter;
    }
}