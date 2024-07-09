using System;
using UnityEngine;

public class BoardDisplay : MonoBehaviour
{
    [SerializeField] private Color lightSquareColor = Color.white, darkSquareColor = Color.black;

    [SerializeField] private Sprite whitePawnSpr,
        whiteKnightSpr,
        whiteBishopSpr,
        whiteRookSpr,
        whiteQueenSpr,
        whiteKingSpr,
        blackPawnSpr,
        blackKnightSpr,
        blackBishopSpr,
        blackRookSpr,
        blackQueenSpr,
        blackKingSpr;
    
    [SerializeField] private Board board;

    private void Update()
    {
        ForEachSquare((index, square) => square.PieceImage.sprite = board.PieceGrid[index].Id switch
        {
            PieceId.WHITE_PAWN      => whitePawnSpr,
            PieceId.WHITE_KNIGHT    => whiteKnightSpr,
            PieceId.WHITE_BISHOP    => whiteBishopSpr,
            PieceId.WHITE_ROOK      => whiteRookSpr,
            PieceId.WHITE_QUEEN     => whiteQueenSpr,
            PieceId.WHITE_KING      => whiteKingSpr,
                
            PieceId.BLACK_PAWN      =>  blackPawnSpr,
            PieceId.BLACK_KNIGHT    => blackKnightSpr,
            PieceId.BLACK_BISHOP    => blackBishopSpr,
            PieceId.BLACK_ROOK      => blackRookSpr,
            PieceId.BLACK_QUEEN     => blackQueenSpr,
            PieceId.BLACK_KING      => blackKingSpr,
                
            _ => null
        });
    }

    private void OnValidate()
    {
        ForEachSquare((index, square) =>
            square.SquareImage.color = IsLightSquare(index) ? lightSquareColor : darkSquareColor);
    }

    private bool IsLightSquare(int index)
    {
        int x = index % 8;
        int y = index / 8;
        return (x + y) % 2 == 0;
    }

    private void ForEachSquare(Action<int, Square> action)
    {
        for (int i = 0; i < board.NumCells; i++)
            action(i, Squares[i]);
    }

    private Square[] Squares => GetComponentsInChildren<Square>();
}
