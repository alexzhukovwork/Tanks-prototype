using System;
using System.Collections;
using System.Collections.Generic;
using Morpeh;
using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BotSpawnerSystem))]
public class BotSpawnerSystem : UpdateSystem
{
    [SerializeField] 
    private GameObject[] Bots;

    [SerializeField] 
    private GameObject[] Positions;

    [SerializeField] 
    private float TimeFirstBotSpawn = 5;
    
    [SerializeField] 
    private float TimePerSpawn = 20;

    private Filter filterPlayer;

    private float currentTime = 0;
    
    public override void OnAwake()
    {
        filterPlayer = Filter.All.With<PlayerComponent>().With<HealthComponent>().With<PhotonViewComponent>();

        currentTime = TimePerSpawn - TimeFirstBotSpawn;
    }

    public override void OnUpdate(float deltaTime)
    {
        currentTime += deltaTime;

        if (currentTime > TimePerSpawn) {

            currentTime = 0;
            
            foreach (var entity in filterPlayer) {
                var health = entity.GetComponent<HealthComponent>();
                var photonView = entity.GetComponent<PhotonViewComponent>();

                if (photonView.PhotonView.IsMine) {
                    SpawnBot((int) health.UnitType);
                }
            }
        }
    }
    
    private void SpawnBot(int index)
    {
        if (index > -1 && index < Bots.Length && index < Positions.Length) {
            var transformSpawn = Positions[index].transform;
            PhotonNetwork.Instantiate(Bots[index].name, transformSpawn.position, transformSpawn.rotation);
        }
    }

}
