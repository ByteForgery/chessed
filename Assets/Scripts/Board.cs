using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private int width = 8, height = 8;
    [SerializeField] private string startPositionFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
    
    private static readonly Piece[] PIECES =
    {
        Piece.NONE,

        Piece.WHITE_PAWN, Piece.WHITE_KNIGHT, Piece.WHITE_BISHOP,
        Piece.WHITE_ROOK, Piece.WHITE_QUEEN, Piece.WHITE_KING,

        Piece.BLACK_PAWN, Piece.BLACK_KNIGHT, Piece.BLACK_BISHOP,
        Piece.BLACK_ROOK, Piece.BLACK_QUEEN, Piece.BLACK_KING
    };
    
    public Piece[] PieceGrid { get; private set; }

    private void Awake()
    {
        PieceGrid = new Piece[NumCells];
        
        LoadFEN(startPositionFEN);
    }

    public void LoadFEN(string fen)
    {
        int squareIndex = 0;
        foreach (char currentChar in fen)
        {
            if (char.IsLetter(currentChar))
            {
                PieceGrid[squareIndex] = Piece.SYMBOL_TO_PIECE[currentChar];
                squareIndex++;
            }

            if (char.IsDigit(currentChar))
            {
                int digit = (int)char.GetNumericValue(currentChar);
                squareIndex += digit;
            }
        }
    }

    public bool IsEmpty(int x, int y) => GetPieceOnCoord(x, y) == Piece.NONE;

    public bool IsEnemy(int x, int y, bool isWhite)
    {
        Piece piece = PieceGrid[CoordToIndex(x, y)];
        if (isWhite)
            return piece.Id is >= PieceId.BLACK_PAWN and <= PieceId.BLACK_KING;
        
        return piece.Id is >= PieceId.WHITE_PAWN and <= PieceId.WHITE_KING;
    }

    public bool IsValidCoord(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;

    public bool IsSameSide(int fromX, int fromY, int toX, int toY)
    {
        Piece fromPiece = GetPieceOnCoord(fromX, fromY);
        Piece toPiece = GetPieceOnCoord(toX, toY);
        if (fromPiece == Piece.NONE || toPiece == Piece.NONE) return false;
        
        return fromPiece.IsWhite == toPiece.IsWhite;
    }

    public void MovePiece(Move move) => move.Execute(this);

    public void SetPieceOnCoord(int x, int y, Piece piece)
    {
        PieceGrid[CoordToIndex(x, y)] = piece;
    }

    public Piece GetPieceOnCoord(int x, int y) => PieceGrid[CoordToIndex(x, y)];

    private int CoordToIndex(int x, int y) => y * width + x;
    private (int x, int y) IndexToCoord(int index) => (index % width, index / width);

    public int NumCells => width * height;
}