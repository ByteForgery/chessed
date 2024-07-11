using System.Collections.Generic;
using UnityEngine;

namespace Chessed.Logic
{
    public class Clock
    {
        public Dictionary<Side, float> Time { get; }
        
        private readonly GameState state;
        private readonly float increment;
        
        public Clock(ClockSettings settings, GameState state)
        {
            Time = new()
            {
                { Side.White, settings.startTime },
                { Side.Black, settings.startTime }
            };

            increment = settings.increment;
            this.state = state;
        }

        public void Update(float deltaTime)
        {
            if (state.IsGameOver) return;
            Time[state.CurrentPlayer] = Mathf.Max(Time[state.CurrentPlayer] - deltaTime, 0f);

            if (Time[state.CurrentPlayer] != 0f) return;
            state.EndGameByTimeout(state.CurrentPlayer.Opponent());
        }

        public void OnMove() => Time[state.CurrentPlayer] += increment;
    }

    [System.Serializable]
    public class ClockSettings
    {
        public float startTime;
        public float increment;
    }
}