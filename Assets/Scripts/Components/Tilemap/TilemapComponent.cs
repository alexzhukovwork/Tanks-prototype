using Morpeh;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct TilemapComponent : IComponent
{
    public Tilemap Tilemap;
}