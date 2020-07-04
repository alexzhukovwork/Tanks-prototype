using Morpeh;
using UnityEngine;

[System.Serializable]
public struct PathComponent : IComponent
{
    public Vector2Int CurrentPoint;
    public Vector2Int NextPoint;
    public int Distance;
}