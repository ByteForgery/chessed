﻿namespace Chessed.Logic
{
    public class EnPassantMove : Move
    {
        public override MoveType Type => MoveType.EnPassant;

        public override Square From { get; }
        public override Square To { get; }

        private readonly Square captureSquare;

        public EnPassantMove(Square from, Square to)
        {
            From = from;
            To = to;
            captureSquare = new Square(to.X, from.Y);
        }
        
        public override void Execute(Board board)
        {
            new NormalMove(From, To).Execute(board);
            board[captureSquare] = null;
        }
    }
}