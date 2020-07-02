using System;
using UnityEngine;
using Photon;
using Photon.Pun;

namespace Pun
{
    public class EventHandlerTanks : Photon.Pun.MonoBehaviourPun, IPunObservable
    {
        [SerializeField] 
        private PoolSystemBase PoolSystem;
        
        private PhotonView photonView;
        private void Start()
        {
            photonView = PhotonView.Get(this);
        }

        [PunRPC]
        private void Shot(int damage, EUnitType unitType, Vector3 position, Quaternion rotation, Vector3 dir, float speed)
        {
            photonView = PhotonView.Get(this);
            
            var gameObject = PoolSystem.Pop();
        
            gameObject.SetActive(true);

            ref var newBulletComponent = ref gameObject.GetComponent<BulletProvider>().GetData(out _);
            ref var newTransformComponent = ref gameObject.GetComponent<TransformProvider>().GetData(out _);
            ref var newMovementComponent = ref gameObject.GetComponent<MovementProvider>().GetData(out _);

            newBulletComponent.Damage = damage;
            newBulletComponent.UnitType = unitType;

            newTransformComponent.Transform.position = position;
            newTransformComponent.Transform.rotation = rotation;

            newMovementComponent.Dir = dir;
            newMovementComponent.Position = position;
            newMovementComponent.Speed = speed;
        }

        public void SendShot(int damage, EUnitType unitType, Vector3 position, Quaternion rotation, Vector3 dir, float speed)
        {
            photonView.RPC("Shot", RpcTarget.Others, damage, unitType, position, rotation,
                dir, speed);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext (transform.position);
                stream.SendNext (transform.rotation);
            } else {
                Debug.Log((Vector3)stream.ReceiveNext());
                Debug.Log((Quaternion)stream.ReceiveNext());
            }
        }
    }
}
