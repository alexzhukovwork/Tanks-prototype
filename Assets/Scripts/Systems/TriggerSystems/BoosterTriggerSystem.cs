using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BoosterTriggerSystem))]
public abstract class BoosterTriggerSystem : UpdateSystem {
    
    private Filter firstObjectFilter;
    private Filter secondObjectFilter;

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

            if (firstCollider.TriggerCollider != null) {
                for (int j = 0; j < secondObjectFilter.Length; j++) {
                    if (secondColliders.GetComponent(j).Collider2D.Equals(firstCollider.TriggerCollider)) {
                        OnTrigger(firstObjectFilter.GetEntity(i), secondObjectFilter.GetEntity(j));
                    }
                }

                firstCollider.TriggerCollider = null;
                firstObjectFilter.GetEntity(i).AddComponent<InactiveComponent>();
            }
        }
    }

    protected abstract void OnTrigger(IEntity first, IEntity second);

    protected abstract Filter InstantiateFirstObjectFilter();

    protected abstract Filter InstantiateSecondObjectFilter();
}