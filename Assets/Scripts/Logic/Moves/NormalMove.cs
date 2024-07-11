namespace Chessed.Logic
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Normal;
        
        public override MoveSquares Squares { get; }

        public NormalMove(Square from, Square to) => Squares = new MoveSquares(from, to);

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