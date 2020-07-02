using Morpeh;
using Morpeh.Globals;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(StartSystem))]
public class StartSystem : UpdateSystem
{
    [SerializeField] 
    private GlobalEvent StartEvent;
    
    private Filter filter;
    private Filter emblemFilter;
    
    public override void OnAwake() {
        emblemFilter = Filter.All.With<EmblemComponent>().With<RespawnComponent>().With<HealthComponent>();
        filter = Filter.All.With<PlayerComponent>().With<InactiveComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        if (StartEvent.IsPublished) {
            var respawns = emblemFilter.Select<RespawnComponent>();
            var healths = emblemFilter.Select<HealthComponent>();

            for (int i = 0; i < emblemFilter.Length; i++) {
                ref var respawn = ref respawns.GetComponent(i);
                ref var health = ref healths.GetComponent(i);
                
                health.GameObject.SetActive(true);
                health.Health = respawn.health;
            }
            
            foreach (var entity in filter) {
                entity.RemoveComponent<InactiveComponent>();
            }
        }
    }
}