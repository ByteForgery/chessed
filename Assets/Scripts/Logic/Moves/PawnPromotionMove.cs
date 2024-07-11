using System.Runtime.Serialization;

namespace Chessed.Logic
{
    public class PawnPromotionMove : Move
    {
        public override MoveType Type => MoveType.Promotion;
        
        public override MoveSquares Squares { get; }

        private readonly PromotionType promotionType;

        public PawnPromotionMove(Square from, Square to, PromotionType promotionType)
        {
            Squares = new MoveSquares(from, to);
            this.promotionType = promotionType;
        }

        public override MoveResult Execute(Board board)
        {
            Piece pawn = board[From];
            bool isCapture = !board.IsSquareEmpty(To);
            
            board[From] = null;

            Piece promotionPiece = CreatePromotionPiece(pawn.side);
            promotionPiece.hasMoved = true;
            board[To] = promotionPiece;

            bool isCheck = board.IsInCheck(pawn.side.Opponent());
            return new MoveResult(true, isCapture, isCheck);
        }

        private Piece CreatePromotionPiece(Side side) => promotionType switch
        {
            PromotionType.Knight => new Knight(side),
            PromotionType.Bishop => new Bishop(side),
            PromotionType.Rook => new Rook(side),
            PromotionType.Queen => new Queen(side),
            _ => null
        };
    }

    public class Promotion
    {
        public MoveSquares MoveSquares { get; }
        public Side Side { get; }
        public PromotionType type;

        public Square From => MoveSquares.From;
        public Square To => MoveSquares.To;

        public Promotion(MoveSquares squares, Side side)
        {
            MoveSquares = squares;
            Side = side;
        }
    }

    public enum PromotionType
    {
        Knight,
        Bishop,
        Rook,
        Queen
    }
}