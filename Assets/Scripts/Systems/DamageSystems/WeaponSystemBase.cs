using System;
using System.Collections.Generic;
using Morpeh;
using Photon.Pun;
using Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WeaponSystemBase))]
public abstract class WeaponSystemBase : UpdateSystem
{
    [SerializeField] 
    private PoolSystemBase PoolSystem;

    private Filter filterPlayer;

    private Filter filterEventHandler;

    private BulletProvider bulletProvider;
    private MovementProvider movementProvider;
    private TransformProvider transformProvider;

    private static readonly float TOLERANCE = 0.1f;
    
    public override void OnAwake()
    {
        filterPlayer = GetWeaponObject();

        filterEventHandler = Filter.All.With<EventHandlerComponnet>().With<PhotonViewComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        ShotNotifier eventHandler = GetEventHandler();

        UpdateWeaponTime(deltaTime);
        
        var photonViews = filterPlayer.Select<PhotonViewComponent>();
        
        for (int i = 0; i < filterPlayer.Length; i++) {
            if (NeedToShot(filterPlayer.GetEntity(i))) {
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

    private void MakeShot(ShotNotifier eventHandler, IEntity player)
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

    private ShotNotifier GetEventHandler()
    {
        ShotNotifier eventHandler = null;

        var handlers = filterEventHandler.Select<EventHandlerComponnet>();
        var photons = filterEventHandler.Select<PhotonViewComponent>();

        for (int i = 0; i < filterEventHandler.Length; i++) {
            var handler = handlers.GetComponent(i);
            var photon = photons.GetComponent(i);

            if (photon.PhotonView.IsMine)
                eventHandler = handler.shotNotifier;
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

    protected abstract bool NeedToShot(IEntity entity);

    protected abstract Filter GetWeaponObject();
}