﻿using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MovementSystem))]
public class MovementSystem : UpdateSystem
{
    private Filter filter;
    
    public override void OnAwake()
    {
        filter = Filter.All.With<MovementComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var entity in filter) {
            ref var unit = ref entity.GetComponent<MovementComponent>(out _);
            unit.Position += Vector3.one * deltaTime;
        }
    }
}