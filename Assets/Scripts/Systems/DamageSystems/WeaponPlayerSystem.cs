using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WeaponPlayerSystem))]
public class WeaponPlayerSystem : WeaponSystemBase {
    protected override bool NeedToShot(IEntity entity)
    {
        return Input.GetKey(KeyCode.Space);
    }

    protected override Filter GetWeaponObject()
    {
        return Filter.All.With<PlayerComponent>().With<TransformComponent>().With<PhotonViewComponent>()
            .With<HealthComponent>().Without<InactiveComponent>().With<WeaponComponent>();
    }
}