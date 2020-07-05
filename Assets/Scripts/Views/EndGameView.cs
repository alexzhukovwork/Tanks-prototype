using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class EndGameView : MonoBehaviour
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
        private Button NewGameButton;

        public void SetLoseText()
        {
            GameEndStatusText.text = LoseText;
        }

        public void SetWinText()
        {
            GameEndStatusText.text = WinText;
        }

        public void ActivateView()
        {
            EndGamePanel.SetActive(true);
        }

        public void DisableVIew()
        {
            EndGamePanel.SetActive(false);
        }

        public void SetNewGameInteractable(bool value)
        {
            NewGameButton.interactable = value;
        }
    }
}
