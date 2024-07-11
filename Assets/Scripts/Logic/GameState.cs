using System.Collections.Generic;
using System.Linq;

namespace Chessed.Logic
{
    public class GameState
    {
        public Board Board { get; }
        public Side CurrentPlayer { get; private set; }
        public Result Result { get; private set; }
        public Clock Clock { get; }
        public string StateFEN { get; private set; }
        
        private int fiftyMoveRuleCounter;

        private readonly Dictionary<string, int> stateHistory = new();

        public GameState(Board board, ClockSettings clockSettings, Side currentPlayer)
        {
            Board = board;
            Clock = new Clock(clockSettings, this);
            CurrentPlayer = currentPlayer;

            StateFEN = FEN.FromGame(currentPlayer, board);
            stateHistory[StateFEN] = 1;
        }

        public IEnumerable<Move> LegalMovesForPiece(Square square)
        {
            Piece piece = Board[square];
            if (Board.IsSquareEmpty(square) || piece.side != CurrentPlayer)
                return Enumerable.Empty<Move>();

            IEnumerable<Move> moveCandidates = piece.GetMoves(square, Board);
            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        public void HandlePromotion(Promotion promotion)
        {
            PawnPromotionMove move = new PawnPromotionMove(promotion.From, promotion.To, promotion.type);
            HandleMove(move);
        }

        public MoveResult HandleMove(Move move)
        {
            Board.SetPawnSkipSquare(CurrentPlayer, null);

            MoveResult result = move.Execute(Board);
            if (result.ResetsFiftyMoveRule)
            {
                fiftyMoveRuleCounter = 0;
                stateHistory.Clear();
            } else
                fiftyMoveRuleCounter++;
            
            Clock.OnMove();
            CurrentPlayer = CurrentPlayer.Opponent();
            UpdateStateFEN();
            CheckForGameEnd();

            return result;
        }

        public IEnumerable<Move> AllLegalMovesForSide(Side side)
        {
            IEnumerable<Move> moveCandidates = Board.PieceSquaresForSide(side).SelectMany(square =>
            {
                Piece piece = Board[square];
                return piece.GetMoves(square, Board);
            });

            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        public void EndGameByTimeout(Side winner) => Result = Result.Win(winner, EndReason.Timeout);

        private void CheckForGameEnd()
        {
            if (Board.HasInsufficientMaterial())
                Result = Result.Draw(EndReason.InsufficientMaterial);
            if (IsFiftyMoveRule)
                Result = Result.Draw(EndReason.FiftyMoveRule);
            if (IsThreefoldRepetition)
                Result = Result.Draw(EndReason.ThreefoldRepetition);
            
            if (AllLegalMovesForSide(CurrentPlayer).Any()) return;

            Result = Board.IsInCheck(CurrentPlayer)
                ? Result.Win(CurrentPlayer.Opponent(), EndReason.Checkmate)
                : Result.Draw(EndReason.Stalemate);
                
        }

        private void UpdateStateFEN()
        {
            StateFEN = FEN.FromGame(CurrentPlayer, Board);

            if (!stateHistory.TryAdd(StateFEN, 1))
                stateHistory[StateFEN]++;
        }

        private bool IsFiftyMoveRule => (fiftyMoveRuleCounter / 2) >= 50;
        private bool IsThreefoldRepetition => stateHistory[StateFEN] == 3;

        public bool IsGameOver => Result != null;
    }
}