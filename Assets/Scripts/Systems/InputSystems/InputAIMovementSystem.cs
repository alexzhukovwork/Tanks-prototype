using System.Collections.Generic;
using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InputAIMovementSystem))]
public class InputAIMovementSystem : InputMovementSystemBase
{
    private List<MovementComponent> movementComponents;
    private List<PathComponent> pathComponents;
    private List<TransformComponent> transformComponents;
    private List<PathFinderComponent> pathFinderComponents;

    private int currentIndex;

    public override void OnAwake()
    {
        movementComponents = new List<MovementComponent>();
        pathComponents = new List<PathComponent>();
        transformComponents = new List<TransformComponent>();
        pathFinderComponents = new List<PathFinderComponent>();
        
        base.OnAwake();
    }

    public override void OnUpdate(float deltaTime)
    {
        OnUpdateStart();
        
        foreach (var entity in filter) {
            movementComponents.Add(entity.GetComponent<MovementComponent>());
            pathComponents.Add(entity.GetComponent<PathComponent>());
            transformComponents.Add(entity.GetComponent<TransformComponent>());
            pathFinderComponents.Add(entity.GetComponent<PathFinderComponent>());
        }

        base.OnUpdate(deltaTime);
    }

    private void OnUpdateStart()
    {
        movementComponents.Clear();
        pathComponents.Clear();
        transformComponents.Clear();
        pathFinderComponents.Clear();
        currentIndex = 0;
    }

    protected override bool IsUp()
    {
        Vector2Int dir = GetDir();

        return dir.y == 1 && dir.x == 0;
    }

    protected override bool IsDown()
    {
        Vector2Int dir = GetDir();

        return dir.y == -1 && dir.x == 0;
    }

    protected override bool IsRight()
    {
        Vector2Int dir = GetDir();

        return dir.y == 0 && dir.x == 1;
    }

    protected override bool IsLeft()
    {
        Vector2Int dir = GetDir();

        return dir.y == 0 && dir.x == -1;
    }

    private Vector2Int GetDir()
    {
        Vector2Int dir = Vector2Int.zero;

        if (currentIndex < pathComponents.Count)
            dir = pathComponents[currentIndex].NextPoint - pathComponents[currentIndex].CurrentPoint;
        
        return dir;
    }
    protected override Filter GetManipulateObject()
    {
        return Filter.All.With<PathFinderComponent>().With<MovementComponent>()
            .With<MovementViewComponent>().With<TransformComponent>().
            With<PhotonViewComponent>().Without<InactiveComponent>().
            With<PathComponent>();
    }

    protected override void InputOnEntity(IEntity entity)
    {
        base.InputOnEntity(entity);

        currentIndex++;
    }
}