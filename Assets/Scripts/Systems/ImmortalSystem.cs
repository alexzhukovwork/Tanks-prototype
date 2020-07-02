using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ImmortalSystem))]
public class ImmortalSystem : UpdateSystem
{
    private Filter filter;
    
    public override void OnAwake()
    {
        filter = Filter.All.With<ImmortalComponent>().With<ImmortalEffectComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var immortals = filter.Select<ImmortalComponent>();
        var immortalEffects = filter.Select<ImmortalEffectComponent>();
        
        for (int i = 0; i < filter.Length; i++) {
            ref var immortal = ref immortals.GetComponent(i);
            ref var immortalEffect = ref immortalEffects.GetComponent(i);
                
            immortal.CurrentTime += deltaTime;

            if (immortal.CurrentTime > immortal.ImmortalTime) {
                filter.GetEntity(i).RemoveComponent<ImmortalComponent>();
                immortalEffect.Effect.SetActive(false);
            }
        }
    }
}