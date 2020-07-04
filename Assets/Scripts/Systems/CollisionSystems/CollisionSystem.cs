using Morpeh;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CollisionSystem))]
public abstract class CollisionSystem : FixedUpdateSystem
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

                    Debug.Log(firstCollider.Collsion2D.otherCollider.gameObject.name);
                    
                    if (secondColliders.GetComponent(j).Collider2D.Equals(firstCollider.Collsion2D.otherCollider)) {
                        OnCollision(firstObjectFilter.GetEntity(i), secondObjectFilter.GetEntity(j));
                    }
                }

                firstCollider.Collsion2D = null;
                
                firstObjectFilter.GetEntity(i).AddComponent<InactiveComponent>();
            }
        }
    }

    protected abstract void OnCollision(IEntity first, IEntity second);
    protected abstract Filter InstantiateFirstObjectFilter();
    protected abstract Filter InstantiateSecondObjectFilter();
}