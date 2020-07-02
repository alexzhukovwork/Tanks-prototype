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

    protected BulletProvider bulletProvider;
    protected MovementProvider movementProvider;
    protected TransformProvider transformProvider;

    private static readonly float TOLERANCE = 0.1f;
    public override void OnAwake()
    {
        filterPlayer = Filter.All.With<PlayerComponent>().With<TransformComponent>().With<PhotonViewComponent>().
            With<HealthComponent>().Without<InactiveComponent>().With<WeaponComponent>();

        filterEventHandler = Filter.All.With<EventHandlerComponnet>().With<PhotonViewComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        EventHandlerTanks eventHandler = GetEventHandler();

        UpdateWeaponTime(deltaTime);
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            var photonViews = filterPlayer.Select<PhotonViewComponent>();
            
            for (int i = 0; i < filterPlayer.Length; i++) {
                
                var photonView = photonViews.GetComponent(i);
                
                if (photonView.PhotonView.IsMine) {
                    MakeShot(eventHandler, filterPlayer.GetEntity(i));
                }
            }
        }
    }

    private void UpdateWeaponTime(float deltaTime)
    { 
        var weapons = filterPlayer.Select<WeaponComponent>();

        for (int i = 0; i < filterPlayer.Length; i++) {
            ref var weapon = ref weapons.GetComponent(i);

            weapon.TimeToNextShot -= deltaTime;

            if (weapon.TimeToNextShot < 0)
                weapon.TimeToNextShot = 0;
        }

    }

    private void MakeShot(EventHandlerTanks eventHandler, IEntity player)
    {
        ref var playerTransform = ref player.GetComponent<TransformComponent>();
        ref var health = ref player.GetComponent<HealthComponent>();
        ref var weapon = ref player.GetComponent<WeaponComponent>();

        if (weapon.TimeToNextShot <= 0) {

            GetBulletComponents();
            movementProvider.GetData(out _).Dir = CalculateDir(playerTransform.Transform);
            transformProvider.GetData(out _).Transform.position =
                playerTransform.Transform.position + movementProvider.GetData(out _).Dir / 2;
            transformProvider.GetData(out _).Transform.rotation = playerTransform.Transform.rotation;
            movementProvider.GetData(out _).Speed = weapon.BulletSpeed;
            bulletProvider.GetData(out _).Damage = weapon.Damage;
            bulletProvider.GetData(out _).UnitType = health.UnitType;

            eventHandler.SendShot(
                bulletProvider.GetData(out _).Damage,
                bulletProvider.GetData(out _).UnitType,
                transformProvider.GetData(out _).Transform.position,
                transformProvider.GetData(out _).Transform.rotation,
                movementProvider.GetData(out _).Dir,
                movementProvider.GetData(out _).Speed
            );

            weapon.TimeToNextShot = weapon.Cooldown;
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

    private EventHandlerTanks GetEventHandler()
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

        return eventHandler;
    }

    private void GetBulletComponents()
    {
        var gameObject = PoolSystem.Pop();
        
        gameObject.SetActive(true);

        bulletProvider = gameObject.GetComponent<BulletProvider>();
        transformProvider = gameObject.GetComponent<TransformProvider>();
        movementProvider = gameObject.GetComponent<MovementProvider>();
    }
}