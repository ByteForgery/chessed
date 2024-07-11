using System;
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

        protected Piece(Side side) => this.side = side;

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

        protected IEnumerable<Square> MoveSliding(Square from, Vector2Int[] dirs, Board board) =>
            dirs.SelectMany(dir => MoveSliding(from, dir, board));

        public virtual bool CanCaptureOpponentKing(Square from, Board board) =>
            GetMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece is { Type: PieceType.King };
            });
    }
    
    public enum PieceType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public static class PieceTypeExtensions
    {
        public static Piece Create(this PieceType type, Side side) => type switch
        {
            PieceType.Pawn => new Pawn(side),
            PieceType.Knight => new Knight(side),
            PieceType.Bishop => new Bishop(side),
            PieceType.Rook => new Rook(side),
            PieceType.Queen => new Queen(side),
            PieceType.King => new King(side),
            _ => throw new ArgumentException($"Piece type {type} does not exist!")
        };
        
        public static char BlackSymbol(this PieceType type) => type switch
        {
            PieceType.Pawn => 'p',
            PieceType.Knight => 'n',
            PieceType.Bishop => 'b',
            PieceType.Rook => 'r',
            PieceType.Queen => 'q',
            PieceType.King => 'k',
            _ => throw new ArgumentException($"Piece type {type} does not exist!")
        };

        public static char WhiteSymbol(this PieceType type) => char.ToUpper(BlackSymbol(type));
    }
}