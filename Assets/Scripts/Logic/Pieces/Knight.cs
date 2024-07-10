using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Chessed.Logic
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;

        public Knight(Side side) : base(side) {}
        
        public override Piece Copy()
        {
            Knight copy = new(side);
            copy.hasMoved = hasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Square from, Board board) =>
            MoveSquares(from, board).Select(to => new NormalMove(from, to));

        private static IEnumerable<Square> PotentialMoveSquares(Square from)
        {
            foreach (Vector2Int vDir in new[] { Direction.NORTH, Direction.SOUTH })
            {
                foreach (Vector2Int hDir in new[] { Direction.EAST, Direction.WEST })
                {
                    yield return from + vDir * 2 + hDir;
                    yield return from + hDir * 2 + vDir;
                }
            }
        }

        private IEnumerable<Square> MoveSquares(Square from, Board board) =>
            PotentialMoveSquares(from).Where(square => 
                square.IsValid && (board.IsSquareEmpty(square) || board[square].side != side));
    }
}