using System;
using Chessed;
using Chessed.Logic;
using TMPro;
using UnityEngine;

namespace Chessed
{
    public class ClockDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text blackTimeText, whiteTimeText;
        [SerializeField] private GameManager manager;

        private Clock Clock => manager.GameState.Clock;

        private void Update()
        {
            foreach (Side side in new[] { Side.White, Side.Black })
            {
                float time = Clock.Time[side];
                int totalMillis = (int)(time * 1000f);
                int mins = totalMillis / 60000;
                int remainingMillis = totalMillis % 60000;
                int secs = remainingMillis / 1000;
                int millis = remainingMillis % 1000;

                string str = $"{mins}:{secs:D2}" + (time <= 10f ? $":{millis:D3}" : "");
                TMP_Text text = side switch
                {
                    Side.White => whiteTimeText,
                    Side.Black => blackTimeText,
                    _ => throw new ArgumentException($"Clock side cannot be {side}")
                };

                text.text = str;
            }
        }
    }
}
