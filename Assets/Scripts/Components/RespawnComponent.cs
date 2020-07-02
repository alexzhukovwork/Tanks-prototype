using Morpeh;
using UnityEngine;

[System.Serializable]
public struct RespawnComponent : IComponent
{
    public Transform RespawnTransform;
    public int health;
}