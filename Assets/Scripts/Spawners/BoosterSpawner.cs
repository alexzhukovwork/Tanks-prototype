using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pun
{
    public class BoosterSpawner : MonoBehaviourPun, IPunObservable
    {
        private PhotonView photonView;

        [SerializeField] 
        private Vector2 MinimalPosition;
        
        [SerializeField] 
        private Vector2 MaxPosition;
        
        [SerializeField] 
        private GameObject[] Boosters;

        [SerializeField] 
        private float TimeRepeating = 15;

        private void Start()
        {
            photonView = PhotonView.Get(this);
            
            InvokeRepeating(nameof(SpawnBooster), 0, TimeRepeating);
        }

        [PunRPC]
        private void SpawnBoosterRPC(int index, Vector3 position, bool isMine)
        {
            if (!isMine)
                CancelInvoke(nameof(SpawnBooster));
            
            Boosters[index].SetActive(true);

            Boosters[index].transform.position = position;
        }

        private Vector3 GetRandomPosition()
        {
            float x = Random.Range(MinimalPosition.x, MaxPosition.x);
            float y = Random.Range(MinimalPosition.y, MaxPosition.y);
            
            Vector3 position = new Vector3(x, y);

            return position;
        }

        private void SpawnBooster()
        {
            if (HasAnyBooster() || !PhotonNetwork.IsConnected) {
                return;
            }

            int index = Random.Range(0, Boosters.Length);

            Vector3 position = GetRandomPosition();
            
            SpawnBoosterRPC(index, position, true);
            photonView.RPC("SpawnBoosterRPC", RpcTarget.Others, index, position, false);
        }

        private bool HasAnyBooster()
        {
            bool hasBooster = false;
            
            for (int i = 0; i < Boosters.Length; i++) {
                if (Boosters[i].activeSelf) {
                    hasBooster = true;
                    break;
                }
            }

            return hasBooster;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            
        }
    }
}
