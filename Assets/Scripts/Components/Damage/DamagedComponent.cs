using Morpeh;
using UnityEngine;

[System.Serializable]
public struct DamagedComponent : IComponent
{
    public BulletComponent Bullet;
    public Collider2DComponent BulletCollider;
}