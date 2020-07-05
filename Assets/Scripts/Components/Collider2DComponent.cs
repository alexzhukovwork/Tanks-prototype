﻿using Morpeh;
using UnityEngine;

[System.Serializable]
public struct Collider2DComponent : IComponent
{
    public Collider2D Collider2D;
    public Collision2D Collsion2D;
    public Collider2D TriggerCollider;
}