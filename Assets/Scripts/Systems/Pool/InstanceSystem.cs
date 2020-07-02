using Morpeh;
using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InstanceSystem))]
public class InstanceSystem : PoolSystemBase
{
    private Filter filter;
    
    public override GameObject Pop()
    {
        return Instantiate(BulletPrefab, Vector3.zero, Quaternion.identity);
    }

    public override void OnAwake()
    {
        filter = Filter.All.With<InactiveComponent>().With<BulletComponent>().With<PhotonViewComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var bullets = filter.Select<BulletComponent>();
        var photons = filter.Select<PhotonViewComponent>();
        
        for (int i = 0; i < filter.Length; i++) {
            ref var bullet = ref bullets.GetComponent(i);
            ref var photon = ref photons.GetComponent(i);

            if (photon.PhotonView.IsMine) {
                PhotonNetwork.Destroy(bullet.Bullet);
                filter.GetEntity(i).RemoveComponent<BulletComponent>();
                filter.GetEntity(i).RemoveComponent<InactiveComponent>();
            }
        }
    }
}