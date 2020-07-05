using System.Collections.Generic;
using Morpeh;
using UnityEngine;

[System.Serializable]
public struct TilemapHealthComponent : IComponent
{
    public List<string> Tiles;
    public List<int> Health;
}