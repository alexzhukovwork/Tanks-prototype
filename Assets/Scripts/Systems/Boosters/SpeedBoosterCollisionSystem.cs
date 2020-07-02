using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(SpeedBoosterCollisionSystem))]
public class SpeedBoosterCollisionSystem : CollisionSystem
{
    private Filter playerWithSpeedBooster;
    
    public override void OnAwake()
    {
        base.OnAwake();

        playerWithSpeedBooster =
            Filter.All.With<PlayerComponent>().With<MovementComponent>().With<SpeedBoosterComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        var movementComponents = playerWithSpeedBooster.Select<MovementComponent>();
        var speedBoosterComponents = playerWithSpeedBooster.Select<SpeedBoosterComponent>();

        for (int i = 0; i < playerWithSpeedBooster.Length; i++) {
            ref var movementComponent = ref movementComponents.GetComponent(i);
            ref var speedBoosterComponent = ref speedBoosterComponents.GetComponent(i);

            speedBoosterComponent.CurrentTime += deltaTime;

            if (speedBoosterComponent.CurrentTime > speedBoosterComponent.AllTime) {
                movementComponent.Speed = speedBoosterComponent.OldSpeed;
                playerWithSpeedBooster.GetEntity(i).RemoveComponent<SpeedBoosterComponent>();
            }
        }
    }

    protected override void OnCollision(IEntity first, IEntity second)
    {
        var speedBooster = first.GetComponent<SpeedBoosterComponent>();

        ref var newSpeedBooster = ref second.AddComponent<SpeedBoosterComponent>();
        ref var movementComponent = ref second.GetComponent<MovementComponent>();

        speedBooster.This.SetActive(false);
        
        newSpeedBooster.AllTime = speedBooster.AllTime;
        newSpeedBooster.CurrentTime = speedBooster.CurrentTime;
        newSpeedBooster.NewSpeed = speedBooster.NewSpeed;
        newSpeedBooster.OldSpeed = movementComponent.Speed;

        movementComponent.Speed = newSpeedBooster.NewSpeed;
    }

    protected override Filter InstantiateFirstObjectFilter()
    {
        Filter filter = Filter.All.With<SpeedBoosterComponent>().
            With<Collider2DComponent>();

        return filter;
    }

    protected override Filter InstantiateSecondObjectFilter()
    {
        Filter filter = Filter.All.With<Collider2DComponent>().With<PlayerComponent>().With<MovementComponent>();
            
        return filter;
    }
}