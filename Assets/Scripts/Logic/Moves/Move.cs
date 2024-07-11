using UnityEngine;

namespace Chessed.Logic
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract MoveSquares Squares { get; }

        public Square From => Squares.From;
        public Square To => Squares.To;
        
        public Vector2Int FromPos => From.Position;
        public Vector2Int ToPos => To.Position;

        public abstract MoveResult Execute(Board board);

        public virtual bool IsLegal(Board board)
        {
            Side side = board[FromPos].side;
            Board boardCopy = board.Copy();
            Execute(boardCopy);
            return !boardCopy.IsInCheck(side);
        }

        public string Algebraic => From.Algebraic + To.Algebraic;
    }

    public class MoveSquares
    {
        public Square From { get; }
        public Square To { get; }

        public MoveSquares(Square from, Square to)
        {
            From = from;
            To = to;
        }
    }

    public readonly struct MoveResult
    {
        public bool IsPawnMove { get; }
        public bool IsCapture { get; }
        public bool IsCheck { get; }

        public MoveResult(bool isPawnMove, bool isCapture, bool isCheck)
        {
            IsPawnMove = isPawnMove;
            IsCapture = isCapture;
            IsCheck = isCheck;
        }

        public bool ResetsFiftyMoveRule => IsPawnMove || IsCapture;
    }

    public enum MoveType
    {
        Normal,
        CastleKS,
        CastleQS,
        DoublePawn,
        EnPassant,
        Promotion
    }
}