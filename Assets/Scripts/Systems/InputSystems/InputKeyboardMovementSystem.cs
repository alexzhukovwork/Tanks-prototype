using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InputKeyboardMovementSystem))]
public class InputKeyboardMovementSystem : InputMovementSystemBase {
    protected override bool IsUp()
    {
        return Input.GetKey(KeyCode.W);
    }

    protected override bool IsDown()
    {
        return Input.GetKey(KeyCode.S);
    }

    protected override bool IsRight()
    {        
        return Input.GetKey(KeyCode.D);
    }

    protected override bool IsLeft()
    {
        return Input.GetKey(KeyCode.A);
    }

    protected override Filter GetManipulateObject()
    {
        return Filter.All.With<PlayerComponent>().With<MovementComponent>()
            .With<MovementViewComponent>().With<TransformComponent>().With<PhotonViewComponent>().Without<InactiveComponent>();
    }
}