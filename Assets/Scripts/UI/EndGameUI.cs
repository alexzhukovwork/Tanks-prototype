using System;
using System.Collections.Generic;
using Morpeh.Globals;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EndGameUI : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI GameEndStatusText;

        [SerializeField] 
        private GameObject EndGamePanel;

        [SerializeField] 
        private string LoseText = "You Lose!";

        [SerializeField] 
        private string WinText = "You Win!";

        [SerializeField] 
        private GlobalEvent LoseEvent;
        
        [SerializeField] 
        private GlobalEvent WinEvent;

        private void Start()
        {
            LoseEvent.Subscribe(OnLose);
            WinEvent.Subscribe(OnWin);
        }

        private void OnLose(IEnumerable<int> enumerable)
        {
            GameEndStatusText.text = LoseText;
            EndGamePanel.SetActive(true);
        }

        private void OnWin(IEnumerable<int> enumerable)
        {
            GameEndStatusText.text = WinText;
            EndGamePanel.SetActive(true);
        }
    }
}
