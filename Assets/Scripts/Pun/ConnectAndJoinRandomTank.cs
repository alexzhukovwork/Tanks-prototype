using System.Collections;
using Morpeh.Globals;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pun
{
    public class ConnectAndJoinRandomTank : ConnectAndJoinRandom
    {
        [SerializeField] 
        private GameObject PanelPlay;
        
        [SerializeField] 
        private TextMeshProUGUI SearchingText;
        
        [SerializeField] 
        private Button PlayerButton;

        [SerializeField] 
        private GlobalEvent GlobalEvent;

        [SerializeField] 
        private int MinimalPlayerCount = 1;
        
        private int countNumber = 3;
        
        private string[] searchingTexts = {"Player searching", "Player searching.", "Player searching..", "Player searching..."};
        
        public void Disconnect()
        {
            PanelPlay.SetActive(true);
            PlayerButton.interactable = true;
            SearchingText.text = string.Empty;
            
            PhotonNetwork.Disconnect();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            StartCoroutine(nameof(WaitPlayer));
        }

        private IEnumerator WaitPlayer()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(1);
            int i = 0;

            while (PhotonNetwork.CurrentRoom == null) {
                yield return waitForSeconds;
            }
            
            while (PhotonNetwork.CurrentRoom.PlayerCount < MinimalPlayerCount) {
                SearchingText.text = searchingTexts[i++];

                i = i < searchingTexts.Length ? i : 0;
                    
                yield return waitForSeconds;
            }

            TextAlignmentOptions textAlignment = SearchingText.alignment;
            SearchingText.alignment = TextAlignmentOptions.Center;
            
            for (i = countNumber; i > 0; i--) {
                SearchingText.text = i.ToString();
                
                yield return waitForSeconds;
            }

            SearchingText.alignment = textAlignment;
            
            SearchingText.text = string.Empty;
            
            PanelPlay.SetActive(false);
            
            GlobalEvent.Publish();
        }
    }
}
