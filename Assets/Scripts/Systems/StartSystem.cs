using Morpeh;
using Morpeh.Globals;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(StartSystem))]
public class StartSystem : UpdateSystem
{
    [SerializeField] 
    private GlobalEvent StartEvent;
    
    private Filter filter;
    
    public override void OnAwake() {
        filter = Filter.All.With<PlayerComponent>().With<InactiveComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        if (StartEvent.IsPublished) {
            foreach (var entity in filter) {
                entity.RemoveComponent<InactiveComponent>();
            }
        }
    }
}