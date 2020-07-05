using Morpeh;
using Morpeh.Globals;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(StartSystem))]
public class StartSystem : UpdateSystem
{
    [SerializeField] 
    private GlobalEvent StartEvent;
    
    private Filter playerFilter;
    private Filter emblemFilter;
    private Filter botFilter;
    
    public override void OnAwake() {
        emblemFilter = Filter.All.With<EmblemComponent>().With<RespawnComponent>().With<HealthComponent>();
        playerFilter = Filter.All.With<PlayerComponent>().With<InactiveComponent>();
        botFilter = Filter.All.With<PathFinderComponent>().With<InactiveComponent>();
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

            foreach (var entity in botFilter) {
                entity.RemoveComponent<InactiveComponent>();
            }
            
            foreach (var entity in playerFilter) {
                entity.RemoveComponent<InactiveComponent>();
            }
        }
    }
}