using Chessed.Logic;
using UnityEngine;

namespace Chessed
{
    public class BoardManager : MonoBehaviour
    {
        public GameState GameState;
        public Square selectedSquare;

        private void Awake()
        {
            GameState = new GameState(Board.Default(), Side.White);
        }

        public void ExecuteMove(Move move) => GameState.ExecuteMove(move);
    }
}
