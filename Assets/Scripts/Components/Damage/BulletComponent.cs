using Morpeh;
using UnityEngine;

[System.Serializable]
public struct BulletComponent : IComponent
{
    public int Damage;
    public EUnitType UnitType;
    public GameObject Bullet;
}