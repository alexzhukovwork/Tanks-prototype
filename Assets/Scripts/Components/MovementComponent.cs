using Morpeh;
using UnityEngine;

[System.Serializable]
public struct MovementComponent : IComponent
{
    public Vector3 Position;
    public Vector3 Dir;
    public float Speed;
}