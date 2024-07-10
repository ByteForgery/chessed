using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chessed.Logic
{
    public class Board
    {
        private const string STANDARD_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        
        private readonly Piece[,] pieces = new Piece[8, 8];

        public static Board Default()
        {
            Board board = new Board();
            board.LoadFromFEN(STANDARD_FEN);
            return board;
        }

        private void LoadFromFEN(string fen)
        {
            Piece[,] fenPieces = FEN.Parse(fen);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    pieces[x, y] = fenPieces[x, y];
                }
            }
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