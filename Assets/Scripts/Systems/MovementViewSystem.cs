using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MovementViewSystem))]
public class MovementViewSystem : UpdateSystem
{
    private Filter filter;
    
    public override void OnAwake()
    {
        filter = Filter.All.With<MovementComponent>().With<MovementViewComponent>().Without<InactiveComponent>().With<PhotonViewComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var views = filter.Select<MovementViewComponent>();
        var units = filter.Select<MovementComponent>();
        var photons = filter.Select<PhotonViewComponent>();

        for (int i = 0; i < filter.Length; i++) {
            ref var view = ref views.GetComponent(i);
            ref var unit = ref units.GetComponent(i);
            ref var photon = ref photons.GetComponent(i);
            
            if (photon.PhotonView.IsMine)
                view.Rigidbody2D.velocity = unit.Speed * unit.Dir;
        }
    }
}