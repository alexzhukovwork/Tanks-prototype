using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TilemapHealth", menuName = "ScriptableObjects/TilemapHealth", order = 1)]
public class TilemapHealth : ScriptableObject
{
    public List<string> Tiles;
    
    public List<int> Health;
}
