using System;
using System.Collections.Generic;
using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WeaponSystemBase))]
public abstract class WeaponSystemBase : UpdateSystem
{
    [SerializeField]
    protected GameObject Bullet;
    
    private Filter filterPlayer;
    
    protected GameObject bullet;

    private int damage = 1;
    private float speed = 5;

    protected BulletProvider bulletProvider;
    protected MovementProvider movementProvider;
    protected TransformProvider transformProvider;
    protected Collider2DProvider collider2DProvider;

    private static readonly float TOLERANCE = 0.1f;
    private static readonly float MINIMAL_VELOCITY = 0.5f;
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
                movementProvider.GetData(out _).Speed = speed;
                bulletProvider.GetData(out _).Damage = damage;
            }
        }

        if (collider2DProvider != null) {
            List<RaycastHit2D> raycastHit2D = new List<RaycastHit2D>();
            collider2DProvider.GetData(out _).Collider2D.Raycast(movementProvider.GetData(out _).Dir, 
                new ContactFilter2D(), raycastHit2D, 0.1f);
            if (raycastHit2D.Count > 0)
                OnBulletCollised();
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

    protected abstract void GetBulletComponents();

    protected abstract void OnBulletCollised();
}