﻿namespace Chessed.Logic
{
    public class EnPassantMove : Move
    {
        public override MoveType Type => MoveType.EnPassant;

        public override MoveSquares Squares { get; }

        private readonly Square captureSquare;

        public EnPassantMove(Square from, Square to)
        {
            Squares = new MoveSquares(from, to);
            captureSquare = new Square(to.X, from.Y);
        }
        
        public override MoveResult Execute(Board board)
        {
            Side side = board[From].side;
            
            new NormalMove(From, To).Execute(board);
            board[captureSquare] = null;

            bool isCheck = board.IsInCheck(side.Opponent());
            return new MoveResult(true, true, isCheck);
        }
    }
}