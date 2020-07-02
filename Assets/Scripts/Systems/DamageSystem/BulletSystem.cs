using Morpeh;
using Photon.Pun;
using Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BulletSystem))]
public class BulletSystem : UpdateSystem
{
    private Filter filter;

    private Filter bulletFilter;

    public override void OnAwake()
    {
        filter = Filter.All.With<InactiveComponent>().With<BulletComponent>();

        bulletFilter = Filter.All.With<BulletComponent>().With<PhotonViewComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var bulletComponents = filter.Select<BulletComponent>();

        var bullets = bulletFilter.Select<BulletComponent>();
        var photons = bulletFilter.Select<PhotonViewComponent>();

        for (int i = 0; i < bulletFilter.Length; i++) {
            ref var bullet = ref bullets.GetComponent(i);
            ref var photon = ref photons.GetComponent(i);

            if (!photon.PhotonView.IsMine)
                bullet.UnitType = InstantiateTanks.EnemyType;
        }
        
        for (int i = 0; i < filter.Length; i++) {
            ref var bulletComponent = ref bulletComponents.GetComponent(i);

            bulletComponent.Bullet.SetActive(false);
        }
    }
}