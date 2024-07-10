﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chessed.Logic
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        
        private readonly Vector2Int forward;

        public Pawn(Side side) : base(side)
        {
            forward = side switch
            {
                Side.White => Direction.NORTH,
                Side.Black => Direction.SOUTH,
                _ => Vector2Int.zero
            };
        }
        
        public override Piece Copy()
        {
            Pawn copy = new(side);
            copy.hasMoved = hasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Square from, Board board)
        {
            return ForwardMoves(from, board).Concat(DiagonalMoves(from, board));
        }

        private static bool CanMoveTo(Square square, Board board) => square.IsValid && board.IsSquareEmpty(square);

        private bool CanCaptureAt(Square square, Board board)
        {
            if (!square.IsValid || board.IsSquareEmpty(square)) return false;

            return board[square].side != side;
        }

        private IEnumerable<Move> ForwardMoves(Square from, Board board)
        {
            Square singleMoveTo = from + forward;
            if (CanMoveTo(singleMoveTo, board))
            {
                yield return new NormalMove(from, singleMoveTo);

                Square doubleMoveTo = singleMoveTo + forward;
                if (!hasMoved && CanMoveTo(doubleMoveTo, board))
                    yield return new NormalMove(from, doubleMoveTo);
            }
        }

        private IEnumerable<Move> DiagonalMoves(Square from, Board board)
        {
            foreach (Vector2Int dir in new[] { Direction.EAST, Direction.WEST })
            {
                Square to = from + forward + dir;
                if (CanCaptureAt(to, board))
                    yield return new NormalMove(from, to);
            }
        }

        public override bool CanCaptureOpponentKing(Square from, Board board) =>
            DiagonalMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece is { Type: PieceType.King };
            });
    }
}