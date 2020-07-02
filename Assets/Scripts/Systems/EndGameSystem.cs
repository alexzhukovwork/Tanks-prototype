using Morpeh;
using Morpeh.Globals;
using Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EndGameSystem))]
public class EndGameSystem : UpdateSystem
{ 
    [SerializeField]
    private GlobalEvent LoseGameEvent;
    
    [SerializeField]
    private GlobalEvent WinGameEvent;
    
    private Filter filter;
    
    public override void OnAwake()
    {
        filter = Filter.All.With<EmblemComponent>().With<HealthComponent>().With<DeadComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var healths = filter.Select<HealthComponent>();

        for (int i = 0; i < filter.Length; i++) {
            if (healths.GetComponent(i).UnitType == InstantiateTanks.MyType)
                LoseGameEvent.Publish();
            else
                WinGameEvent.Publish();

            filter.GetEntity(i).RemoveComponent<DeadComponent>();
        }
    }
}