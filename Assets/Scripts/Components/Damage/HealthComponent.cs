using Morpeh;
using UnityEngine;

[System.Serializable]
public struct HealthComponent : IComponent
{
    public int Health;
    public EUnitType UnitType;
    public GameObject GameObject;
}