using System;
using System.Collections.Generic;
using Morpeh;
using Photon.Pun;
using Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WeaponSystem))]
public class WeaponSystem : UpdateSystem
{
    [SerializeField] 
    private PoolSystemBase PoolSystem;

    private Filter filterPlayer;

    private Filter filterEventHandler;

    private int damage = 1;
    private float speed = 5;

    protected BulletProvider bulletProvider;
    protected MovementProvider movementProvider;
    protected TransformProvider transformProvider;

    private static readonly float TOLERANCE = 0.1f;
    public override void OnAwake()
    {
        filterPlayer = Filter.All.With<PlayerComponent>().With<TransformComponent>().With<PhotonViewComponent>().
            With<HealthComponent>().Without<InactiveComponent>();

        filterEventHandler = Filter.All.With<EventHandlerComponnet>().With<PhotonViewComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        EventHandlerTanks eventHandler = null;

        var handlers = filterEventHandler.Select<EventHandlerComponnet>();
        var photons = filterEventHandler.Select<PhotonViewComponent>();

        for (int i = 0; i < filterEventHandler.Length; i++) {
            var handler = handlers.GetComponent(i);
            var photon = photons.GetComponent(i);

            if (photon.PhotonView.IsMine)
                eventHandler = handler.EventHandlerTanks;
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            var playerTransforms = filterPlayer.Select<TransformComponent>();
            var photonViews = filterPlayer.Select<PhotonViewComponent>();
            var healths = filterPlayer.Select<HealthComponent>();
            
            for (int i = 0; i < filterPlayer.Length; i++) {
                var playerTransform = playerTransforms.GetComponent(i);
                var photonView = photonViews.GetComponent(i);
                var health = healths.GetComponent(i);

                if (photonView.PhotonView.IsMine) {
                    GetBulletComponents();
                    movementProvider.GetData(out _).Dir = CalculateDir(playerTransform.Transform);
                    transformProvider.GetData(out _).Transform.position =
                        playerTransform.Transform.position + movementProvider.GetData(out _).Dir / 2;
                    transformProvider.GetData(out _).Transform.rotation = playerTransform.Transform.rotation;
                    movementProvider.GetData(out _).Speed = speed;
                    bulletProvider.GetData(out _).Damage = damage;
                    bulletProvider.GetData(out _).UnitType = health.UnitType;
                    
                    
                    eventHandler?.SendShot(
                        bulletProvider.GetData(out _).Damage,
                        bulletProvider.GetData(out _).UnitType,
                        transformProvider.GetData(out _).Transform.position,
                        transformProvider.GetData(out _).Transform.rotation,
                        movementProvider.GetData(out _).Dir,
                        movementProvider.GetData(out _).Speed
                    );
                }
            }
        }
    }

    private Vector3 CalculateDir(Transform transform)
    {
        Vector3 dir = Vector3.up;
        
        if (Math.Abs(transform.rotation.z - Quaternion.Euler(0, 0, 90).z) < TOLERANCE) {
            dir = Vector3.left;
        } else if (Math.Abs(transform.rotation.z - Quaternion.Euler(0, 0, -90).z) < TOLERANCE) {
            dir = Vector3.right;
        }  else if (Math.Abs(transform.rotation.z - Quaternion.Euler(0, 0, 180).z) < TOLERANCE) {
            dir = Vector3.down;
        }

        return dir;
    }

    protected void GetBulletComponents()
    {
        var gameObject = PoolSystem.Pop();
        
        gameObject.SetActive(true);

        bulletProvider = gameObject.GetComponent<BulletProvider>();
        transformProvider = gameObject.GetComponent<TransformProvider>();
        movementProvider = gameObject.GetComponent<MovementProvider>();
    }
}