namespace Chessed.Logic
{
    public class DoublePawnMove : Move
    {
        public override MoveType Type => MoveType.DoublePawn;

        public override MoveSquares Squares { get; }

        private readonly Square skippedSquare;

        public DoublePawnMove(Square from, Square to)
        {
            Squares = new MoveSquares(from, to);
            skippedSquare = new Square(from.X, (from.Y + to.Y) / 2);
        }

        public override MoveResult Execute(Board board)
        {
            Side side = board[From].side;
            
            board.SetPawnSkipSquare(side, skippedSquare);
            new NormalMove(From, To).Execute(board);

            bool isCheck = board.IsInCheck(side.Opponent());
            return new MoveResult(true, false, isCheck);
        }
    }
}