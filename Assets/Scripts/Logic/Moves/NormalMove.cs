namespace Chessed.Logic
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Normal;
        
        public NormalMove(Square from, Square to) : base(from, to) {}

        public override void Execute(Board board)
        {
            Piece piece = board[From];
            piece.SetHasMoved();
            board[To] = piece;
            board[From] = null;
        }
    }
}