using System.Collections.Generic;
using System.Linq;

namespace Chessed.Logic
{
    public class Queen : Piece
    {
        public override PieceType Type => PieceType.Queen;

        public Queen(Side side) : base(side) {}
        
        public override Piece Copy()
        {
            Queen copy = new(side);
            copy.hasMoved = hasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Square from, Board board) =>
            MoveSliding(from, Direction.PRINCIPALS, board).Select(to => new NormalMove(from, to));
    }
}