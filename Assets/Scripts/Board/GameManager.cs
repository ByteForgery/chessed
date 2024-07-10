using Chessed.Logic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chessed
{
    public class GameManager : MonoBehaviour
    {
        public GameState GameState;
        public Square selectedSquare;
        public Promotion runningPromotion;

        [SerializeField] private string startPositionFEN;

        private void Awake()
        {
            Board board = startPositionFEN == string.Empty ? Board.Default() : Board.LoadFromFEN(startPositionFEN);
            GameState = new GameState(board, Side.White);
        }

        public void ExecuteMove(Move move) => GameState.HandleMove(move);

        public void ExecutePromotion(Square from, Square to, Side side) =>
            runningPromotion = new Promotion(from, to, side);

        public void ResetGame() => SceneManager.LoadScene(0);
    }
}
