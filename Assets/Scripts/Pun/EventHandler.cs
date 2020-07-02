using System;
using UnityEngine;
using Photon;
using Photon.Pun;

namespace Pun
{
    public class EventHandler : Photon.Pun.MonoBehaviourPun, IPunObservable
    {
        private PhotonView photonView;
        private void Start()
        {
            photonView = PhotonView.Get(this);
            photonView.RPC("ChatMessage", RpcTarget.All, photonView.Controller.IsLocal.ToString(), photonView.Controller.ActorNumber.ToString());
        }

        [PunRPC]
        private void ChatMessage(string a, string b)
        {
            photonView = PhotonView.Get(this);

            Debug.Log(string.Format("ChatMessage {0} {1} {2}", a, b, photonView.Controller.ActorNumber.ToString()));
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
