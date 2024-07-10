using System.Collections.Generic;
using System.Linq;

namespace Chessed.Logic
{
    public class GameState
    {
        public Board Board { get; }
        public Side CurrentPlayer { get; private set; }
        public Result Result { get; private set; }

        public GameState(Board board, Side currentPlayer)
        {
            Board = board;
            CurrentPlayer = currentPlayer;
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
            move.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
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
            
            if (AllLegalMovesForSide(CurrentPlayer).Any()) return;

            Result = Board.IsInCheck(CurrentPlayer)
                ? Result.Win(CurrentPlayer.Opponent())
                : Result.Draw(EndReason.Stalemate);
        }

        public bool IsGameOver => Result != null;
    }
}