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

        secondObjectFilter = Filter.All.With<Collider2DComponent>().Without<InactiveComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var secondColliders = secondObjectFilter.Select<Collider2DComponent>();
        var firstColliders = firstObjectFilter.Select<Collider2DComponent>();

        for (int i = 0; i < firstObjectFilter.Length; i++) {
            var firstCollider = firstColliders.GetComponent(i);
            
            Collider2D [] colliders = new Collider2D[1];
            
            if (firstCollider.Collider2D.GetContacts(new ContactFilter2D(), colliders) > 0) {

                for (int j = 0; j < secondObjectFilter.Length; j++) {
                        
                    if (secondColliders.GetComponent(j).Collider2D.Equals(colliders[0])) {
                        OnCollision(firstObjectFilter.GetEntity(i), secondObjectFilter.GetEntity(j));
                    }
                }
                
                firstObjectFilter.GetEntity(i).AddComponent<InactiveComponent>();
            }
        }
    }

    protected abstract void OnCollision(IEntity first, IEntity second);
    protected abstract Filter InstantiateFirstObjectFilter();
    protected abstract Filter InstantiateSecondObjectFilter();
}