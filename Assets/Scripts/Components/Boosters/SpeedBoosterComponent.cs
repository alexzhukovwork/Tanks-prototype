using Morpeh;
using UnityEngine;

[System.Serializable]
public struct SpeedBoosterComponent : IComponent
{
    public float CurrentTime;
    public float AllTime;
    public float NewSpeed;
    public float OldSpeed;
    public GameObject This;
}