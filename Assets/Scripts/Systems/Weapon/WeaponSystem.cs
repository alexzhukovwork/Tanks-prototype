using System;
using System.Collections.Generic;
using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WeaponSystem))]
public class WeaponSystem : UpdateSystem
{
    [SerializeField] 
    private PoolSystemBase PoolSystem;
    
    private Filter filterPlayer;

    private int damage = 1;
    private float speed = 5;

    protected BulletProvider bulletProvider;
    protected MovementProvider movementProvider;
    protected TransformProvider transformProvider;

    private static readonly float TOLERANCE = 0.1f;
    public override void OnAwake()
    {
        filterPlayer = Filter.All.With<PlayerComponent>().With<TransformComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        if (Input.GetKeyDown(KeyCode.Space)) {
            var playerTransforms = filterPlayer.Select<TransformComponent>();

            for (int i = 0; i < filterPlayer.Length; i++) {
                var playerTransform = playerTransforms.GetComponent(i);
                GetBulletComponents();
                movementProvider.GetData(out _).Dir = CalculateDir(playerTransform.Transform);
                transformProvider.GetData(out _).Transform.position = 
                    playerTransform.Transform.position + movementProvider.GetData(out _).Dir / 2;
                transformProvider.GetData(out _).Transform.rotation = playerTransform.Transform.rotation;
                movementProvider.GetData(out _).Speed = speed;
                bulletProvider.GetData(out _).Damage = damage;
                bulletProvider.GetData(out _).UnitType = EUnitType.Teammate;
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