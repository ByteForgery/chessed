using System.Collections.Generic;
using System.Linq;

namespace Chessed.Logic
{
    public class Rook : Piece
    {
        public override PieceType Type => PieceType.Rook;

        public Rook(Side side) : base(side) {}
        
        public override Piece Copy()
        {
            Rook copy = new(side);
            copy.hasMoved = hasMoved;
            return copy;
        }
        
        public override IEnumerable<Move> GetMoves(Square from, Board board) =>
            MoveSliding(from, Direction.CARDINALS, board).Select(to => new NormalMove(from, to));
    }
}