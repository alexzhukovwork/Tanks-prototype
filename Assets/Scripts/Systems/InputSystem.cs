using Morpeh;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InputSystem))]
public class InputSystem : UpdateSystem
{
    private Filter filter;

    private Quaternion rotation;
    private Vector3 dir;
    
    private static readonly Quaternion RightRotation = Quaternion.Euler(0, 0, -90);
    private static readonly Quaternion LeftRotation = Quaternion.Euler(0, 0, 90);
    private static readonly Quaternion UpRotation = Quaternion.Euler(0, 0, 0);
    private static readonly Quaternion DownRotation = Quaternion.Euler(0, 0, 180);
    
    public override void OnAwake()
    {
        filter = Filter.All.With<PlayerComponent>().With<MovementComponent>()
            .With<MovementViewComponent>().With<TransformComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var movementComponents = filter.Select<MovementComponent>();
        var transformComponents = filter.Select<TransformComponent>();

        CheckInput();

        for (int i = 0; i < filter.Length; i++) {
            ref var unitMovementComponent = ref movementComponents.GetComponent(i);
            ref var unityTransformComponent = ref transformComponents.GetComponent(i);
            unitMovementComponent.Dir = dir;
            unityTransformComponent.Transform.rotation = rotation;
        }
    }

    private void CheckInput()
    {
        dir = Vector3.zero;
        
        if (Input.GetKey(KeyCode.W)) {
            dir = Vector3.up;
            rotation = UpRotation;
        } else if (Input.GetKey(KeyCode.S)) {
            dir = Vector3.down;
            rotation = DownRotation;
        } else if (Input.GetKey(KeyCode.A)) {
            dir = Vector3.left;
            rotation = LeftRotation;
        } else if (Input.GetKey(KeyCode.D)) {
            dir = Vector3.right;
            rotation = RightRotation;
        }
    }
}