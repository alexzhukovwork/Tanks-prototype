using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class StartGameView : MonoBehaviour
    {
        [SerializeField] 
        private GameObject PanelPlay;
        
        [SerializeField] 
        private TextMeshProUGUI SearchingText;
        
        [SerializeField]
        private TextMeshProUGUI TimerText;
        
        [SerializeField] 
        private Button PlayButton;

        private string[] searchingTexts = {"Player searching", "Player searching.", "Player searching..", "Player searching..."};

        private int i = 0;
        
        public void ActivateView()
        {
            PanelPlay.SetActive(true);
            PlayButton.interactable = true;
        }

        public void SetSearchingText(string text)
        {
            SearchingText.text = text;
        }
        
        public void SetTimerText(string text)
        {
            TimerText.text = text;
        }

        public void UpdateSearchingText()
        {
            SearchingText.text = searchingTexts[i++];

            i = i < searchingTexts.Length ? i : 0;
        }

        public void DisableView()
        {
            PanelPlay.SetActive(false);
        }

        public void SetPlayInteractable(bool value)
        {
            PlayButton.interactable = value;
        }
    }
}
