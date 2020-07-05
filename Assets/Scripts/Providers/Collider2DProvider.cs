using System;
using Morpeh;
using UnityEngine;

public class Collider2DProvider : MonoProvider<Collider2DComponent>
{
    [SerializeField] 
    private string IgnoreTag = "Bullet"; 
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag(IgnoreTag))
            GetData(out _).Collsion2D = other;
        else
            GetData(out _).Collsion2D = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GetData(out _).TriggerCollider = other;
    }
}