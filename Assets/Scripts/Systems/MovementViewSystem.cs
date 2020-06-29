using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MovementViewSystem))]
public class MovementViewSystem : UpdateSystem
{
    private Filter filter;
    
    public override void OnAwake()
    {
        filter = Filter.All.With<MovementComponent>().With<MovementViewComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var views = filter.Select<MovementViewComponent>();
        var units = filter.Select<MovementComponent>();

        for (int i = 0; i < filter.Length; i++) {
            ref var view = ref views.GetComponent(i);
            ref var unit = ref units.GetComponent(i);
            
            
            view.Rigidbody2D.velocity = unit.Speed * unit.Dir;
        }
    }
}