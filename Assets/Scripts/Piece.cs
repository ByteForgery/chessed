public struct Piece
{
    public static readonly Piece NONE = new Piece(PieceType.NONE, 0, ' ');

    public static readonly Piece WHITE_PAWN = new Piece(PieceType.PAWN, 1, 'p');
    public static readonly Piece WHITE_KNIGHT = new Piece(PieceType.KNIGHT, 2, 'n');
    public static readonly Piece WHITE_BISHOP = new Piece(PieceType.BISHOP, 3, 'b');
    public static readonly Piece WHITE_ROOK = new Piece(PieceType.ROOK, 4, 'r');
    public static readonly Piece WHITE_QUEEN = new Piece(PieceType.QUEEN, 5, 'q');
    public static readonly Piece WHITE_KING = new Piece(PieceType.KING, 6, 'k');

    public static readonly Piece BLACK_PAWN = new Piece(PieceType.PAWN, 7, 'P');
    public static readonly Piece BLACK_KNIGHT = new Piece(PieceType.KNIGHT, 8, 'N');
    public static readonly Piece BLACK_BISHOP = new Piece(PieceType.BISHOP, 9, 'B');
    public static readonly Piece BLACK_ROOK = new Piece(PieceType.ROOK, 10, 'R');
    public static readonly Piece BLACK_QUEEN = new Piece(PieceType.QUEEN, 11, 'Q');
    public static readonly Piece BLACK_KING = new Piece(PieceType.KING, 12, 'K');

    public PieceType Type { get; private set; }
    public int Id { get; private set; }
    public char Symbol { get; private set; }

    private Piece(PieceType type, int id, char symbol)
    {
        Type = type;
        Id = id;
        Symbol = symbol;
    }

    public static bool operator ==(Piece a, object b) => a.Equals(b);
    public static bool operator !=(Piece a, object b) => !a.Equals(b);
    
    public override bool Equals(object obj) => obj switch
    {
        Piece piece => Id == piece.Id,
        PieceType otherType => Type == otherType,
        char otherSymbol => Symbol == otherSymbol,
        int otherId => Id == otherId,
        _ => false
    };
}

public enum PieceType
{
    NONE,
    PAWN,
    KNIGHT,
    BISHOP,
    ROOK,
    QUEEN,
    KING
}