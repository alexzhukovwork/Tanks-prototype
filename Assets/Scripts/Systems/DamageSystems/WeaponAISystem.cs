using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WeaponAISystem))]
public class WeaponAISystem : WeaponSystemBase {

    protected override bool NeedToShot(IEntity entity)
    {
        return true;
    }

    protected override Filter GetWeaponObject()
    {
        return Filter.All.With<TransformComponent>().With<PhotonViewComponent>()
            .With<HealthComponent>().Without<InactiveComponent>().
            With<WeaponComponent>().With<PathFinderComponent>().Without<DeadComponent>();
    }
}