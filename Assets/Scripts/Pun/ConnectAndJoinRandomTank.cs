using System.Collections;
using System.Collections.Generic;
using Morpeh.Globals;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Views;

namespace Pun
{
    public class ConnectAndJoinRandomTank : ConnectAndJoinRandom
    {
        [SerializeField] 
        private StartGameView StartGameView;
        
        [SerializeField] 
        private GlobalEvent StartGameEvent;

        [SerializeField] 
        private GlobalEvent NewGameEvent;
        
        [SerializeField]
        private GlobalEvent DisconnectEvent;
        
        [SerializeField]
        private GlobalEvent PlayButtonEvent;
        
        [SerializeField] 
        private int MinimalPlayerCount = 1;

        private int countNumber = 3;

        private static readonly string SCENE_NAME = "Main";
        
        public override void OnEnable()
        {
            base.OnEnable();

            NewGameEvent.Subscribe(OnNewGame);
            DisconnectEvent.Subscribe(OnNewGame);
            PlayButtonEvent.Subscribe(OnPlayButton);
        }

        private void OnPlayButton(IEnumerable<int> obj)
        {
            StartGameView.SetPlayInteractable(false);
            ConnectNow();
        }

        private void OnNewGame(IEnumerable<int> obj)
        {
            StartGameView.ActivateView();
            StartGameView.SetPlayInteractable(false);
            
            Disconnect();
        }

        public void Disconnect()
        {
            StartGameView.ActivateView();
            
            StartGameView.SetSearchingText(string.Empty);
            
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
                StartGameView.UpdateSearchingText();

                yield return waitForSeconds;
            }

            var parameters = new LoadSceneParameters(LoadSceneMode.Additive);

            var operation = SceneManager.LoadSceneAsync(SCENE_NAME, parameters);

            while (operation.isDone) {
                StartGameView.UpdateSearchingText();
                yield return waitForSeconds;
            }
            
            StartGameView.DisableView();

            for (i = countNumber; i > 0; i--) {
                StartGameView.SetTimerText(i.ToString());
                
                yield return waitForSeconds;
            }
            
            StartGameView.SetTimerText(string.Empty);
            
            StartGameEvent.Publish();
        }
    }
}
