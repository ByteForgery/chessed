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
        
        public Square GetPawnSkipSquare(Side side) => pawnSkipPositions[side];
        public void SetPawnSkipSquare(Side side, Square square) => pawnSkipPositions[side] = square;

        public bool IsSquareEmpty(Square square) => this[square] == null;

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