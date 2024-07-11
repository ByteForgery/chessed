using System;
using UnityEngine;

namespace Chessed.Logic
{
    public class CastleMove : Move
    {
        public override MoveType Type { get; }
        public override MoveSquares Squares { get; }

        private readonly Vector2Int kingMoveDir;
        private readonly Square rookFrom, rookTo;
        
        public CastleMove(MoveType type, Square kingSquare)
        {
            Type = type;
            Square to = null;

            int y = kingSquare.Y;
            switch (type)
            {
                case MoveType.CastleKS:
                    kingMoveDir = Direction.EAST;
                    to = new Square(6, y);
                    rookFrom = new Square(7, y);
                    rookTo = new Square(5, y);
                    break;
                case MoveType.CastleQS:
                    kingMoveDir = Direction.WEST;
                    to = new Square(2, y);
                    rookFrom = new Square(0, y);
                    rookTo = new Square(3, y);
                    break;
            }

            Squares = new MoveSquares(kingSquare, to);
        }
        
        public override MoveResult Execute(Board board)
        {
            Side side = board[From].side;
            
            new NormalMove(From, To).Execute(board);
            new NormalMove(rookFrom, rookTo).Execute(board);

            return new MoveResult(false, false, board.IsInCheck(side.Opponent()));
        }

        public override bool IsLegal(Board board)
        {
            Side side = board[From].side;
            if (board.IsInCheck(side)) return false;

            Board copy = board.Copy();
            Square kingSquareInCopy = From;

            for (int i = 0; i < 2; i++)
            {
                new NormalMove(kingSquareInCopy, kingSquareInCopy + kingMoveDir).Execute(copy);
                kingSquareInCopy += kingMoveDir;

                if (copy.IsInCheck(side)) return false;
            }

            return true;
        }
    }
}