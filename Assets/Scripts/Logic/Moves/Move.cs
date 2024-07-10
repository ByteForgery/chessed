using UnityEngine;

namespace Chessed.Logic
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Square From { get; }
        public abstract Square To { get; }
        
        public Vector2Int FromPos => From.Position;
        public Vector2Int ToPos => To.Position;

        public abstract void Execute(Board board);

        public virtual bool IsLegal(Board board)
        {
            Side side = board[FromPos].side;
            Board boardCopy = board.Copy();
            Execute(boardCopy);
            return !boardCopy.IsInCheck(side);
        }
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