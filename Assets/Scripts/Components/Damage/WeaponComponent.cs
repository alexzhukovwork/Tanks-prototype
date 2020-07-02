using Morpeh;
using UnityEngine;

[System.Serializable]
public struct WeaponComponent : IComponent
{
    public int Damage;
    public int BulletSpeed;
    public int Cooldown;
}