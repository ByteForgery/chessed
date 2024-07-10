namespace Chessed.Logic
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Normal;
        
        public override Square From { get; }
        public override Square To { get; }

        public NormalMove(Square from, Square to)
        {
            From = from;
            To = to;
        }

        public override void Execute(Board board)
        {
            Piece piece = board[From];
            board[To] = piece;
            board[From] = null;
            piece.hasMoved = true;
        }
    }
}