using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public int fromX, fromY, toX, toY;
    public Piece piece;
    public Piece capturedPiece;
    public bool isPromotion;
    public Piece promotionPiece;

    public Move(int fromX, int fromY, int toX, int toY, Piece piece, Piece capturedPiece = default, 
        bool isPromotion = false, Piece promotionPiece = default)
    {
        this.fromX = fromX;
        this.fromY = fromY;
        this.toX = toX;
        this.toY = toY;
        this.piece = piece;
        this.capturedPiece = capturedPiece;
        this.isPromotion = isPromotion;
        this.promotionPiece = promotionPiece;
    }

    public void Execute(Board board)
    {
        capturedPiece = board.GetPieceOnCoord(toX, toY);
        board.SetPieceOnCoord(toX, toY, isPromotion ? promotionPiece : piece);
        board.SetPieceOnCoord(fromX, fromY, Piece.NONE);
    }
}
