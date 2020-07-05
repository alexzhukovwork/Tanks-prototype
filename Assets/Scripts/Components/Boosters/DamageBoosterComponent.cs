using Morpeh;
using UnityEngine;

[System.Serializable]
public struct DamageBoosterComponent : IComponent {
    public float CurrentTime;
    public float AllTime;
    public int NewDamage;
    public int OldDamage;
    public GameObject This;
}