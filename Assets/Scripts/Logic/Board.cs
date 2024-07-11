using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chessed.Logic
{
    public class Board
    {
        private const string STANDARD_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";

        private readonly Piece[,] pieces = new Piece[8, 8];

        private readonly Dictionary<Side, Square> pawnSkipPositions = new Dictionary<Side, Square>
        {
            { Side.White, null },
            { Side.Black, null }
        };

        public static Board Default() => LoadFromFEN(STANDARD_FEN);

        public static Board LoadFromFEN(string fen)
        {
            Board board = new Board();
            
            Piece[,] fenPieces = FEN.Parse(fen);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    board.pieces[x, y] = fenPieces[x, y];
                }
            }

            return board;
        }

        public IEnumerable<Square> PieceSquares()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Square square = new Square(x, y);
                    if (!IsSquareEmpty(square))
                        yield return square;
                }
            }
        }

        public IEnumerable<Square> PieceSquaresForSide(Side side) =>
            PieceSquares().Where(square => this[square].side == side);

        public bool IsInCheck(Side side) => PieceSquaresForSide(side.Opponent()).Any(square =>
        {
            Piece piece = this[square];
            return piece.CanCaptureOpponentKing(square, this);
        });

        public Board Copy()
        {
            Board copy = new();
            foreach (Square square in PieceSquares())
                copy[square] = this[square].Copy();

            return copy;
        }

        public PieceCounter CountPieces()
        {
            PieceCounter counter = new PieceCounter();

            foreach (Square square in PieceSquares())
            {
                Piece piece = this[square];
                counter.Increment(piece.side, piece.Type);
            }

            return counter;
        }

        public bool HasInsufficientMaterial()
        {
            PieceCounter counter = CountPieces();
            return IsKingVKing(counter) || IsKingBishopVKing(counter) ||
                   IsKingKnightVKing(counter) || IsKingBishopVKingBishop(counter);
        }

        private static bool IsKingVKing(PieceCounter counter) => counter.TotalCount == 2;

        private static bool IsKingBishopVKing(PieceCounter counter) => counter.TotalCount == 3 &&
                                                                       (counter.White(PieceType.Bishop) == 1 ||
                                                                        counter.Black(PieceType.Bishop) == 1);

        private static bool IsKingKnightVKing(PieceCounter counter) => counter.TotalCount == 3 &&
                                                                       (counter.White(PieceType.Knight) == 1 ||
                                                                        counter.Black(PieceType.Knight) == 1);

        private bool IsKingBishopVKingBishop(PieceCounter counter)
        {
            if (counter.TotalCount != 4) return false;

            if (counter.White(PieceType.Bishop) != 1 || counter.Black(PieceType.Bishop) != 1) return false;

            Square wBishopSquare = FindPiece(Side.White, PieceType.Bishop);
            Square bBishopSquare = FindPiece(Side.Black, PieceType.Bishop);

            return wBishopSquare.Color == bBishopSquare.Color;
        }

        private Square FindPiece(Side side, PieceType type) =>
            PieceSquaresForSide(side).First(square => this[square].Type == type);

        private bool IsUnmovedKingAndRook(Square kingSquare, Square rookSquare)
        {
            if (IsSquareEmpty(kingSquare) || IsSquareEmpty(rookSquare)) return false;

            Piece king = this[kingSquare];
            Piece rook = this[rookSquare];

            return king.Type == PieceType.King && rook.Type == PieceType.Rook &&
                   !king.hasMoved && !rook.hasMoved;
        }

        public bool HasCastleRightKS(Side side) => side switch
        {
            Side.White => IsUnmovedKingAndRook(new Square(4, 7), new Square(7, 7)),
            Side.Black => IsUnmovedKingAndRook(new Square(4, 0), new Square(7, 0)),
            _ => false
        };

        public bool HasCastleRightQS(Side side) => side switch
        {
            Side.White => IsUnmovedKingAndRook(new Square(4, 7), new Square(0, 7)),
            Side.Black => IsUnmovedKingAndRook(new Square(4, 0), new Square(0, 0)),
            _ => false
        };

        private bool HasPawnInPosition(Side side, Square[] pawnSquares, Square skipSquare)
        {
            foreach (Square square in pawnSquares.Where(square => square.IsValid))
            {
                Piece piece = this[square];
                if (piece == null || piece.side != side || piece.Type != PieceType.Pawn) continue;

                EnPassantMove move = new EnPassantMove(square, skipSquare);
                if (move.IsLegal(this)) return true;
            }

            return false;
        }

        public bool CanCaptureEnPassant(Side side)
        {
            Square skipSquare = GetPawnSkipSquare(side.Opponent());

            if (skipSquare == null) return false;

            Square[] pawnSquares = side switch
            {
                Side.White => new[] { skipSquare + Direction.SOUTH_EAST, skipSquare + Direction.SOUTH_WEST },
                Side.Black => new[] { skipSquare + Direction.NORTH_EAST, skipSquare + Direction.NORTH_WEST },
                _ => Array.Empty<Square>()
            };

            return HasPawnInPosition(side, pawnSquares, skipSquare);
        }
        
        public Square GetPawnSkipSquare(Side side) => pawnSkipPositions[side];
        public void SetPawnSkipSquare(Side side, Square square) => pawnSkipPositions[side] = square;

        public bool IsSquareEmpty(Square square) => this[square] == null;
        public bool IsSquareEmpty(int x, int y) => this[x, y] == null;

        public static int SquareToIndex(Square square) => PosToIndex(square.Position);
        public static Square IndexToSquare(int index) => new Square(IndexToPos(index));

        public static int PosToIndex(int x, int y) => y * 8 + x;
        public static int PosToIndex(Vector2Int pos) => PosToIndex(pos.x, pos.y);
        public static Vector2Int IndexToPos(int index) => new Vector2Int(index % 8, index / 8);

        public Piece this[int x, int y]
        {
            get => pieces[x, y];
            set => pieces[x, y] = value;
        }

        public Piece this[Vector2Int pos]
        {
            get => this[pos.x, pos.y];
            set => this[pos.x, pos.y] = value;
        }

        public Piece this[Square square]
        {
            get => this[square.Position];
            set => this[square.Position] = value;
        }

        public Piece this[int index]
        {
            get => this[IndexToPos(index)];
            set => this[IndexToPos(index)] = value;
        }
    }
}