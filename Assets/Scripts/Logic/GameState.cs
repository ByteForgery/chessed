using System.Collections.Generic;
using System.Linq;

namespace Chessed.Logic
{
    public class GameState
    {
        public Board Board { get; }
        public Side CurrentPlayer { get; private set; }

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

        public void ExecuteMove(Move move)
        {
            move.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
        }
    }
}