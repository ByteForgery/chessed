using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private int width = 8, height = 8;
    
    private static readonly Piece[] PIECES =
    {
        Piece.NONE,

        Piece.WHITE_PAWN, Piece.WHITE_KNIGHT, Piece.WHITE_BISHOP,
        Piece.WHITE_ROOK, Piece.WHITE_QUEEN, Piece.WHITE_KING,

        Piece.BLACK_PAWN, Piece.BLACK_KNIGHT, Piece.BLACK_BISHOP,
        Piece.BLACK_ROOK, Piece.BLACK_QUEEN, Piece.BLACK_KING
    };
    
    public int[] PieceGrid { get; private set; }

    private void Awake()
    {
        PieceGrid = new int[NumCells];

        for (int i = 0; i < NumCells; i++)
            PieceGrid[i] = PIECES[Random.Range(0, PIECES.Length)].Id;
    }

    public void SetPieceOnCoord(int x, int y, Piece piece)
    {
        PieceGrid[CoordToIndex(x, y)] = piece.Id;
    }

    private int CoordToIndex(int x, int y) => y * width + x;
    private (int x, int y) IndexToCoord(int index) => (index % width, index / width);

    public int NumCells => width * height;
}