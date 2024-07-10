using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chessed.Logic
{
    public class Bishop : Piece
    {
        public override PieceType Type => PieceType.Bishop;

        public Bishop(Side side) : base(side) {}
        
        public override Piece Copy()
        {
            Bishop copy = new(side);
            copy.hasMoved = hasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Square from, Board board) =>
            MoveSliding(from, Direction.ORDINALS, board).Select(to => new NormalMove(from, to));
    }
}