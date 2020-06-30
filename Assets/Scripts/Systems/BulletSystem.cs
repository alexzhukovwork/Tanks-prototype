using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BulletSystem))]
public class BulletSystem : UpdateSystem
{
    private Filter filter;
    
    public override void OnAwake()
    {
        filter = Filter.All.With<InactiveComponent>().With<BulletComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var bulletComponents = filter.Select<BulletComponent>();

        for (int i = 0; i < filter.Length; i++) {
            ref var bulletComponent = ref bulletComponents.GetComponent(i);
            
            bulletComponent.Bullet.SetActive(false);
        }
    }
}