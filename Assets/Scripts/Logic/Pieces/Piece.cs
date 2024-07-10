using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chessed.Logic
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }

        public readonly Side side;
        public bool hasMoved;

        public Piece(Side side) => this.side = side;

        public abstract Piece Copy();

        public abstract IEnumerable<Move> GetMoves(Square from, Board board);

        protected IEnumerable<Square> MoveSliding(Square from, Vector2Int dir, Board board)
        {
            for (Square square = from + dir; square.IsValid; square += dir)
            {
                if (board.IsSquareEmpty(square))
                {
                    yield return square;
                    continue;
                }

                Piece piece = board[square];
                if (piece.side != side)
                    yield return square;

                yield break;
            }
        }

        public void SetHasMoved()
        {
            hasMoved = true;
            Debug.Log("HAS MOVED NOW");
        }

        protected IEnumerable<Square> MoveSliding(Square from, Vector2Int[] dirs, Board board) =>
            dirs.SelectMany(dir => MoveSliding(from, dir, board));

        public virtual bool CanCaptureOpponentKing(Square from, Board board) =>
            GetMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece is { Type: PieceType.King };
            });
    }
}