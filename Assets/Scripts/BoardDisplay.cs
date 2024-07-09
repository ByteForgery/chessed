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
