using Chessed.Logic;
using TMPro;
using UnityEngine;

namespace Chessed
{
    public class GameStatusDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text resultText, descriptionText;
        [SerializeField] private GameObject resetButton;
        [SerializeField] private GameManager manager;

        private void Update()
        {
            Result result = manager.GameState.Result;
            if (result == null) return;

            bool isCheckmate = result.Reason == EndReason.Checkmate;
            resultText.text = isCheckmate ? "WIN" : "DRAW";
            descriptionText.text =
                isCheckmate ? $"For {result.Winner.ToString()}" : $"by {result.Reason.DisplayName()}";
            
            resetButton.SetActive(true);
        }
    }
}
