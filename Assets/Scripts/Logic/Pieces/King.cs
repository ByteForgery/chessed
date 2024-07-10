using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chessed.Logic
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;

        public King(Side side) : base(side) {}
        
        public override Piece Copy()
        {
            King copy = new(side);
            copy.hasMoved = hasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Square from, Board board)
        {
            foreach (Square to in MoveSquares(from, board))
                yield return new NormalMove(from, to);
        }

        private IEnumerable<Square> MoveSquares(Square from, Board board)
        {
            foreach (Vector2Int dir in Direction.PRINCIPALS)
            {
                Square to = from + dir;
                if (!to.IsValid) continue;

                if (board.IsSquareEmpty(to) || board[to].side != side)
                    yield return to;
            }
        }

        public override bool CanCaptureOpponentKing(Square from, Board board) =>
            MoveSquares(from, board).Any(to =>
            {
                Piece piece = board[to];
                return piece is { Type: PieceType.King };
            });
    }
}