using System;
using System.Collections.Generic;

namespace Chessed.Logic
{
    public static class FEN
    {
        private static readonly Dictionary<char, Func<Piece>> SYMBOL_TO_PIECE = new()
        {
            { 'P', () => new Pawn(Side.White) },
            { 'N', () => new Knight(Side.White) },
            { 'B', () => new Bishop(Side.White) },
            { 'R', () => new Rook(Side.White) },
            { 'Q', () => new Queen(Side.White) },
            { 'K', () => new King(Side.White) },
            
            { 'p', () => new Pawn(Side.Black) },
            { 'n', () => new Knight(Side.Black) },
            { 'b', () => new Bishop(Side.Black) },
            { 'r', () => new Rook(Side.Black) },
            { 'q', () => new Queen(Side.Black) },
            { 'k', () => new King(Side.Black) }
        };

        public static Piece[,] Parse(string fen)
        {
            Piece[,] pieces = new Piece[8, 8];
            
            string[] rows = fen.Split('/');
            for (int i = 0; i < rows.Length; i++)
            {
                string row = rows[i];
                int col = 0;

                foreach (char symbol in row)
                {
                    if (char.IsDigit(symbol))
                        col += (int)char.GetNumericValue(symbol);
                    else
                    {
                        pieces[col, i] = SYMBOL_TO_PIECE[symbol]();
                        col++;
                    }
                }
            }

            return pieces;
        }
    }
}