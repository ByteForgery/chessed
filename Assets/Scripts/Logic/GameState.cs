using System.Collections.Generic;
using System.Linq;

namespace Chessed.Logic
{
    public class GameState
    {
        public Board Board { get; }
        public Side CurrentPlayer { get; private set; }
        public Result Result { get; private set; }

        private int fiftyMoveRuleCounter = 0;
        private string stateFEN;

        private readonly Dictionary<string, int> stateHistory = new();

        public GameState(Board board, Side currentPlayer)
        {
            Board = board;
            CurrentPlayer = currentPlayer;

            stateFEN = FEN.FromGame(currentPlayer, board);
            stateHistory[stateFEN] = 1;
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

        public void HandleMove(Move move)
        {
            Board.SetPawnSkipSquare(CurrentPlayer, null);

            MoveResult result = move.Execute(Board);
            if (result.ResetsFiftyMoveRule)
            {
                fiftyMoveRuleCounter = 0;
                stateHistory.Clear();
            } else
                fiftyMoveRuleCounter++;
            
            CurrentPlayer = CurrentPlayer.Opponent();
            UpdateStateString();
            CheckForGameEnd();
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
                ? Result.Win(CurrentPlayer.Opponent())
                : Result.Draw(EndReason.Stalemate);
        }

        private void UpdateStateString()
        {
            stateFEN = FEN.FromGame(CurrentPlayer, Board);

            if (!stateHistory.TryAdd(stateFEN, 1))
                stateHistory[stateFEN]++;
        }

        private bool IsFiftyMoveRule => (fiftyMoveRuleCounter / 2) >= 50;
        private bool IsThreefoldRepetition => stateHistory[stateFEN] == 3;

        public bool IsGameOver => Result != null;
    }
}