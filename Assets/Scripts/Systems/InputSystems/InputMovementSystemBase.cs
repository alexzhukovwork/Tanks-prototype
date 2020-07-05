using System.ComponentModel;
using Morpeh;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InputMovementSystemBase))]
public abstract class InputMovementSystemBase : UpdateSystem
{
    protected Filter filter;

    private Quaternion rotation;
    private Vector3 dir;

    private static readonly Quaternion RightRotation = Quaternion.Euler(0, 0, -90);
    private static readonly Quaternion LeftRotation = Quaternion.Euler(0, 0, 90);
    private static readonly Quaternion UpRotation = Quaternion.Euler(0, 0, 0);
    private static readonly Quaternion DownRotation = Quaternion.Euler(0, 0, 180);
    
    public override void OnAwake()
    {
        filter = GetManipulateObject();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in filter) {
            CheckInput();

            InputOnEntity(entity);
        }
    }

    private void CheckInput()
    {
        dir = Vector3.zero;
        
        if (IsUp()) {
            dir = Vector3.up;
            rotation = UpRotation;
        } else if (IsDown()) {
            dir = Vector3.down;
            rotation = DownRotation;
        } else if (IsLeft()) {
            dir = Vector3.left;
            rotation = LeftRotation;
        } else if (IsRight()) {
            dir = Vector3.right;
            rotation = RightRotation;
        }
    }

    protected virtual void InputOnEntity(IEntity entity)
    {
        ref var unitMovementComponent = ref entity.GetComponent<MovementComponent>();
        ref var unitTransformComponent = ref entity.GetComponent<TransformComponent>();
        ref var photonComponent = ref entity.GetComponent<PhotonViewComponent>();
        
        if (photonComponent.PhotonView.IsMine) {
            unitMovementComponent.Dir = dir;
            unitTransformComponent.Transform.rotation = rotation;
        }
    }
    
    protected abstract bool IsUp();
    protected abstract bool IsDown();
    protected abstract bool IsRight();
    protected abstract bool IsLeft();
    protected abstract Filter GetManipulateObject();

}