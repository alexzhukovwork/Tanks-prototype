using System.Linq;
using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(RespawnSystem))]
public class RespawnSystem : UpdateSystem
{
    [SerializeField] 
    private float ImmortalTime = 3;
    
    private Filter filter;
    
    public override void OnAwake()
    {
        filter = Filter.All.With<RespawnComponent>().With<DeadComponent>().
            With<HealthComponent>().With<TransformComponent>().With<ImmortalEffectComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var respawns = filter.Select<RespawnComponent>();
        var healths = filter.Select<HealthComponent>();
        var transforms = filter.Select<TransformComponent>();
        
        var immortalEffects = filter.Select<ImmortalEffectComponent>();

        for (int i = 0; i < filter.Length; i++) {
            ref var respawn = ref respawns.GetComponent(i);
            ref var health = ref healths.GetComponent(i);
            ref var transform = ref transforms.GetComponent(i);
            ref var immortalEffect = ref immortalEffects.GetComponent(i);

            transform.Transform.position = respawn.RespawnTransform.position;
            transform.Transform.rotation = respawn.RespawnTransform.rotation;

            health.Health = respawn.health;
            health.GameObject.SetActive(true);
                
            ref var immortal = ref filter.GetEntity(i).AddComponent<ImmortalComponent>(out _);
            immortalEffect.Effect.SetActive(true);
            
            immortal.CurrentTime = 0;
            immortal.ImmortalTime = ImmortalTime;
            
            filter.GetEntity(i).RemoveComponent<DeadComponent>();
        }
    }
}