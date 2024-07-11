using System.Collections.Generic;
using Chessed.Logic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chessed
{
    public class GameManager : MonoBehaviour
    {
        public enum GameMode
        {
            PlayerVsPlayer,
            PlayerVsAI,
            AIVsPlayer,
            AIVsAI
        }
        
        public GameState GameState;
        public Square selectedSquare;
        public Promotion runningPromotion;

        [SerializeField] private GameMode gameMode;
        [SerializeField] private string startPositionFEN;
        [SerializeField] private ClockSettings clockSettings;
        [SerializeField] private SoundPlayer soundPlayer;
        [SerializeField] private StockfishController stockfish;

        private bool firstGameOverUpdate = true;

        public readonly Dictionary<Side, MoveProvider> moveProviders = new()
        {
            { Side.White, null },
            { Side.Black, null }
        };

        private void Awake()
        {
            Board board = startPositionFEN == string.Empty ? Board.Default() : Board.LoadFromFEN(startPositionFEN);
            GameState = new GameState(board, clockSettings, Side.White);

            InitializeMoveProviders();
        }

        private void InitializeMoveProviders()
        {
            switch (gameMode)
            {
                case GameMode.PlayerVsPlayer:
                    moveProviders[Side.White] = new PlayerMoveProvider();
                    moveProviders[Side.Black] = new PlayerMoveProvider();
                    break;
                case GameMode.PlayerVsAI:
                    moveProviders[Side.White] = new PlayerMoveProvider();
                    moveProviders[Side.Black] = new AIMoveProvider(stockfish);
                    break;
                case GameMode.AIVsPlayer:
                    moveProviders[Side.White] = new AIMoveProvider(stockfish);
                    moveProviders[Side.Black] = new PlayerMoveProvider();
                    break;
                case GameMode.AIVsAI:
                    moveProviders[Side.White] = new AIMoveProvider(stockfish);
                    moveProviders[Side.Black] = new AIMoveProvider(stockfish);
                    break;
            }
        }

        private void Update()
        {
            GameState.Clock.Update(Time.deltaTime);
            
            UpdateMove();

            if (!firstGameOverUpdate || GameState.Result == null) return;
            
            firstGameOverUpdate = false;
            soundPlayer.OnGameEnd();
        }

        private void UpdateMove()
        {
            MoveProvider currentMoveProvider = moveProviders[GameState.CurrentPlayer];
            Move move = currentMoveProvider.RequestMove(GameState);

            if (move == null) return;
            
            ExecuteMove(move);
        }

        public void ExecuteMove(Move move)
        {
            MoveResult result = GameState.HandleMove(move);
            stockfish.OnMove(move.Squares);
            soundPlayer.OnMove(result, move.Type);
        }

        public void ExecutePromotion(MoveSquares squares, Side side) =>
            runningPromotion = new Promotion(squares, side);

        public void ResetGame() => SceneManager.LoadScene(0);

        public MoveProvider CurrentMoveProvider => moveProviders[GameState.CurrentPlayer];

        public bool IsCurrentPlayer => IsPlayer(GameState.CurrentPlayer);
        public bool IsCurrentAI => IsAI(GameState.CurrentPlayer);

        public bool IsPlayer(Side side) => moveProviders[side] is PlayerMoveProvider;
        public bool IsAI(Side side) => moveProviders[side] is AIMoveProvider;
    }
}
