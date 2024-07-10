using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace Chessed.Logic
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;

        public King(Side side) : base(side) {}

        private static bool IsUnmovedRook(Square square, Board board)
        {
            if (board.IsSquareEmpty(square)) return false;

            Piece piece = board[square];
            return piece.Type == PieceType.Rook && !piece.hasMoved;
        }

        private static bool AllEmpty(IEnumerable<Square> squares, Board board) =>
            squares.All(board.IsSquareEmpty);

        private bool CanCastleKS(Square from, Board board)
        {
            if (hasMoved) return false;

            Square rookSquare = new Square(7, from.Y);
            Square[] betweenPositions = { new(5, from.Y), new(6, from.Y) };

            return IsUnmovedRook(rookSquare, board) && AllEmpty(betweenPositions, board);
        }

        private bool CanCastleQS(Square from, Board board)
        {
            if (hasMoved) return false;

            Square rookSquare = new Square(0, from.Y);
            Square[] betweenPositions = { new(1, from.Y), new(2, from.Y), new(3, from.Y) };

            return IsUnmovedRook(rookSquare, board) && AllEmpty(betweenPositions, board);
        }
        
        public override Piece Copy()
        {
            King copy = new(side);
            copy.hasMoved = hasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Square from, Board board)
        {
            foreach (Square to in MoveSquares(from, board))
                yield return new NormalMove(from, to);

            if (CanCastleKS(from, board))
                yield return new CastleMove(MoveType.CastleKS, from);
            if (CanCastleQS(from, board))
                yield return new CastleMove(MoveType.CastleQS, from);
        }

        private IEnumerable<Square> MoveSquares(Square from, Board board)
        {
            foreach (Vector2Int dir in Direction.PRINCIPALS)
            {
                Square to = from + dir;
                if (!to.IsValid) continue;

                if (board.IsSquareEmpty(to) || board[to].side != side)
                    yield return to;
            }
        }

        public override bool CanCaptureOpponentKing(Square from, Board board) =>
            MoveSquares(from, board).Any(to =>
            {
                Piece piece = board[to];
                return piece is { Type: PieceType.King };
            });
    }
}