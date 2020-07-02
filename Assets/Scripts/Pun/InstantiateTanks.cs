using Morpeh.Globals;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Pun
{
    public class InstantiateTanks : OnJoinedInstantiate
    {
        [SerializeField] 
        private GlobalEvent JoinedRoom;

        [SerializeField] 
        private GameObject EventHandler;
        
        public static EUnitType MyType;
        public static EUnitType EnemyType;
        
        public override void OnJoinedRoom()
        {
            if (AutoSpawnObjects && !PhotonNetwork.LocalPlayer.HasRejoined)
            {
                SpawnObject(PhotonNetwork.CurrentRoom.PlayerCount - 1);
                
                JoinedRoom.Publish();
            }
        }

        public void SpawnObject(int index)
        {
            if (this.PrefabsToInstantiate != null && index < this.PrefabsToInstantiate.Count) {
                MyType = (EUnitType) index;
                EnemyType = MyType == EUnitType.FirstPlayer ? EUnitType.SecondPlayer : EUnitType.FirstPlayer;
               
                var prefab = this.PrefabsToInstantiate[index];
            
                if (prefab == null)
                    return;
#if UNITY_EDITOR
                Debug.Log("Auto-Instantiating: " + prefab.name);
#endif
                Vector3 spawnPos; Quaternion spawnRot;
                GetSpawnPoint(index, out spawnPos, out spawnRot);
                
                PhotonNetwork.Instantiate(EventHandler.name, spawnPos, spawnRot);

                var newobj = PhotonNetwork.Instantiate(prefab.name, spawnPos, spawnRot, 0);
                
                SpawnedObjects.Push(newobj);
            }
        }

        public void GetSpawnPoint(int index, out Vector3 spawnPos, out Quaternion spawnRot)
        {
            spawnPos = Vector3.zero;
            spawnRot = Quaternion.identity;
        
            if (index < SpawnPoints.Count) {
                var spawnPoint = SpawnPoints[index];

                if (spawnPoint != null) {
                    spawnPos = spawnPoint.position;
                    spawnRot = spawnPoint.rotation;
                }
            }
        }
    }
}
