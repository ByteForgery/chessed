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

        public override MoveResult Execute(Board board)
        {
            Piece piece = board[From];
            bool isCapture = !board.IsSquareEmpty(To);
            
            board[To] = piece;
            board[From] = null;
            piece.hasMoved = true;

            bool isCheck = board.IsInCheck(piece.side.Opponent());
            return new MoveResult(piece.Type == PieceType.Pawn, isCapture, isCheck);
        }
    }
}