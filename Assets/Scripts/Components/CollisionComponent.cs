using Morpeh;
using UnityEngine;

[System.Serializable]
public struct CollisionComponent : IComponent
{
    public GameObject GameObject;
    public GameObject Other;
}