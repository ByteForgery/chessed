using System.Collections.Generic;

public struct Piece
{
    public static readonly Piece NONE = new Piece(PieceId.NONE, ' ');

    public static readonly Piece WHITE_PAWN = new Piece(PieceId.WHITE_PAWN, 'P');
    public static readonly Piece WHITE_KNIGHT = new Piece(PieceId.WHITE_KNIGHT, 'N');
    public static readonly Piece WHITE_BISHOP = new Piece(PieceId.WHITE_BISHOP, 'B');
    public static readonly Piece WHITE_ROOK = new Piece(PieceId.WHITE_ROOK, 'R');
    public static readonly Piece WHITE_QUEEN = new Piece(PieceId.WHITE_QUEEN, 'Q');
    public static readonly Piece WHITE_KING = new Piece(PieceId.WHITE_KING, 'K');

    public static readonly Piece BLACK_PAWN = new Piece(PieceId.BLACK_PAWN, 'p');
    public static readonly Piece BLACK_KNIGHT = new Piece(PieceId.BLACK_KNIGHT, 'n');
    public static readonly Piece BLACK_BISHOP = new Piece(PieceId.BLACK_BISHOP, 'b');
    public static readonly Piece BLACK_ROOK = new Piece(PieceId.BLACK_ROOK, 'r');
    public static readonly Piece BLACK_QUEEN = new Piece(PieceId.BLACK_QUEEN, 'q');
    public static readonly Piece BLACK_KING = new Piece(PieceId.BLACK_KING, 'k');
    
    public static readonly Dictionary<char, Piece> SYMBOL_TO_PIECE = new Dictionary<char, Piece>
    {
        { WHITE_PAWN.Symbol,    WHITE_PAWN },
        { WHITE_KNIGHT.Symbol,  WHITE_KNIGHT },
        { WHITE_BISHOP.Symbol,  WHITE_BISHOP },
        { WHITE_ROOK.Symbol,    WHITE_ROOK },
        { WHITE_QUEEN.Symbol,   WHITE_QUEEN },
        { WHITE_KING.Symbol,    WHITE_KING },
        
        { BLACK_PAWN.Symbol,    BLACK_PAWN },
        { BLACK_KNIGHT.Symbol,  BLACK_KNIGHT },
        { BLACK_BISHOP.Symbol,  BLACK_BISHOP },
        { BLACK_ROOK.Symbol,    BLACK_ROOK },
        { BLACK_QUEEN.Symbol,   BLACK_QUEEN },
        { BLACK_KING.Symbol,    BLACK_KING }
    };

    public int Id { get; private set; }
    public char Symbol { get; private set; }

    private Piece(int id, char symbol)
    {
        Id = id;
        Symbol = symbol;
    }

    public List<(int, int)> GetValidMoves(int x, int y, Board board)
    {
        return Id switch
        {
            PieceId.WHITE_PAWN => GetPawnMoves(x, y, true, board),
            PieceId.BLACK_PAWN => GetPawnMoves(x, y, false, board),
            PieceId.WHITE_KNIGHT or PieceId.BLACK_KNIGHT => GetKnightMoves(x, y, board),
            PieceId.WHITE_BISHOP or PieceId.BLACK_BISHOP => GetBishopMoves(x, y, board),
            PieceId.WHITE_ROOK or PieceId.BLACK_ROOK => GetRookMoves(x, y, board),
            PieceId.WHITE_QUEEN or PieceId.BLACK_QUEEN => GetQueenMoves(x, y, board),
            PieceId.WHITE_KING or PieceId.BLACK_KING => GetKnightMoves(x, y, board),
            _ => new List<(int, int)>()
        };
    }

    private List<(int, int)> GetPawnMoves(int x, int y, bool isWhite, Board board)
    {
        var moves = new List<(int, int)>();
        int dir = isWhite ? 1 : -1;
        
        if (board.IsEmpty(x, y + dir))
            moves.Add((x, y + dir));
        
        if (board.IsEnemy(x - 1, y + dir, isWhite))
            moves.Add((x - 1, y + dir));
        if (board.IsEnemy(x + 1, y + dir, isWhite))
            moves.Add((x + 1, y + dir));

        if ((isWhite && y == 1) || (!isWhite && y == 6))
        {
            if (board.IsEmpty(x, y + dir) && board.IsEmpty(x, y + dir * 2))
                moves.Add((x, y + dir * 2));
        }

        return moves;
    }

    private List<(int, int)> GetKnightMoves(int x, int y, Board board)
    {
        var moves = new List<(int, int)>();
        int[,] offsets = { { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 }, { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 } };

        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int newX = x + offsets[i, 0];
            int newY = y + offsets[i, 1];
            
            if (board.IsValidCoord(newX, newY) && !board.IsSameSide(x, y, newX, newY))
                moves.Add((newX, newY));
        }

        return moves;
    }

    private List<(int, int)> GetBishopMoves(int x, int y, Board board) => 
        GetSlidingMoves(x, y, new[] { (1, 1), (1, -1), (-1, 1), (-1, -1) }, board);

    private List<(int, int)> GetRookMoves(int x, int y, Board board) => 
        GetSlidingMoves(x, y, new[] { (1, 0), (-1, 0), (0, 1), (0, -1) }, board);

    private List<(int, int)> GetQueenMoves(int x, int y, Board board) =>
        GetSlidingMoves(x, y, new[] { (1, 1), (1, -1), (-1, 1), (-1, -1), (1, 0), (-1, 0), (0, 1), (0, -1) }, board);
    
    private List<(int, int)> GetKingMoves(int x, int y, Board board)
    {
        var moves = new List<(int, int)>();
        int[,] offsets = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };

        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int newX = x + offsets[i, 0];
            int newY = y + offsets[i, 1];
            
            if (board.IsValidCoord(newX, newY) && !board.IsSameSide(x, y, newX, newY))
                moves.Add((newX, newY));
        }

        return moves;
    }

    private List<(int, int)> GetSlidingMoves(int x, int y, (int, int)[] dirs, Board board)
    {
        var moves = new List<(int, int)>();

        foreach (var (dirX, dirY) in dirs)
        {
            int newX = x, newY = y;
            while (true)
            {
                newX += dirX;
                newY += dirY;

                if (!board.IsValidCoord(newX, newY))
                    break;

                if (board.IsEmpty(newX, newY))
                {
                    moves.Add((newX, newY));
                } else
                {
                    if (board.IsEnemy(newX, newY, board.GetPieceOnCoord(x, y).IsWhite))
                        moves.Add((newX, newY));
                    break;
                }
            }
        }

        return moves;
    }

    public static bool operator ==(Piece a, object b) => a.Equals(b);
    public static bool operator !=(Piece a, object b) => !a.Equals(b);
    
    public override bool Equals(object obj) => obj switch
    {
        Piece piece => Id == piece.Id,
        char otherSymbol => Symbol == otherSymbol,
        int otherId => Id == otherId,
        _ => false
    };

    public bool IsWhite => Id <= PieceId.WHITE_KING;
}

public static class PieceId
{
    public const int NONE = 0;
        
    public const int WHITE_PAWN = 1;
    public const int WHITE_KNIGHT = 2;
    public const int WHITE_BISHOP = 3;
    public const int WHITE_ROOK = 4;
    public const int WHITE_QUEEN = 5;
    public const int WHITE_KING = 6;

    public const int BLACK_PAWN = 7;
    public const int BLACK_KNIGHT = 8;
    public const int BLACK_BISHOP = 9;
    public const int BLACK_ROOK = 10;
    public const int BLACK_QUEEN = 11;
    public const int BLACK_KING = 12;
}