using System.Runtime.Serialization;

namespace Chessed.Logic
{
    public class PawnPromotionMove : Move
    {
        public override MoveType Type => MoveType.Promotion;
        
        public override Square From { get; }
        public override Square To { get; }

        private readonly PromotionType promotionType;

        public PawnPromotionMove(Square from, Square to, PromotionType promotionType)
        {
            From = from;
            To = to;
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
        public Square From { get; }
        public Square To { get; }
        public Side Side { get; }
        public PromotionType type;

        public Promotion(Square from, Square to, Side side)
        {
            From = from;
            To = to;
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