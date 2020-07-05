using System.Collections;
using System.Collections.Generic;
using Morpeh.Globals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Views;

namespace UI
{
    public class EndGameEventHandler : MonoBehaviour
    {
        [SerializeField] 
        private EndGameView endGameView;
        
        [SerializeField] 
        private GlobalEvent LoseEvent;
        
        [SerializeField] 
        private GlobalEvent WinEvent;
        
        [SerializeField] 
        private GlobalEvent DisconnectEvent;
        
        [SerializeField] 
        private GlobalEvent NewGameEvent;

        private static readonly string SCENE_NAME = "Main";
        
        private void Start()
        {
            LoseEvent.Subscribe(OnLose);
            WinEvent.Subscribe(OnWin);
            DisconnectEvent.Subscribe(OnDisconnect);
            NewGameEvent.Subscribe(OnNewGame);
        }

        private void OnNewGame(IEnumerable<int> obj)
        {
            endGameView.SetNewGameInteractable(false);
            endGameView.DisableVIew();
        }

        private void OnDisconnect(IEnumerable<int> enumerable)
        {
            StartCoroutine(nameof(UnloadSceneAsync));
            OnLose(enumerable);
        }

        private IEnumerator UnloadSceneAsync()
        {
            var asyncOperation = SceneManager.UnloadSceneAsync(SCENE_NAME);
            
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

            while (!asyncOperation.isDone) {
                yield return waitForSeconds;
            }
            
            endGameView.SetNewGameInteractable(true);
        }

        private void OnLose(IEnumerable<int> enumerable)
        {
            StartCoroutine(nameof(UnloadSceneAsync));

            endGameView.SetLoseText();
            endGameView.ActivateView();
        }

        private void OnWin(IEnumerable<int> enumerable)
        {
            StartCoroutine(nameof(UnloadSceneAsync));

            endGameView.SetWinText();
            endGameView.ActivateView();
        }
    }
}
