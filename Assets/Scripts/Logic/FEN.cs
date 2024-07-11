using System;
using System.Text;

namespace Chessed.Logic
{
    public static class FEN
    {
        public static string FromGame(Side currentPlayer, Board board)
        {
            StringBuilder sb = new();
            
            AppendPiecePlacement(board, sb);
            sb.Append(' ');
            AppendCurrentPlayer(currentPlayer, sb);
            sb.Append(' ');
            AppendCastlingRights(board, sb);
            sb.Append(' ');
            AppendEnPassant(currentPlayer, board, sb);

            return sb.ToString();
        }
        
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
                        pieces[col, i] = SymbolToPiece(symbol);
                        col++;
                    }
                }
            }

            return pieces;
        }

        private static void AppendPiecePlacement(Board board, StringBuilder sb)
        {
            for (int y = 0; y < 8; y++)
            {
                if (y != 0)
                    sb.Append('/');
                
                AppendRowData(y, board, sb);
            }
        }
        
        private static void AppendRowData(int y, Board board, StringBuilder sb)
        {
            int skips = 0;

            for (int x = 0; x < 8; x++)
            {
                if (board.IsSquareEmpty(x, y))
                {
                    skips++;
                    continue;
                }

                if (skips > 0)
                {
                    sb.Append(skips);
                    skips = 0;
                }

                sb.Append(PieceToSymbol(board[x, y]));
            }

            if (skips > 0)
                sb.Append(skips);
        }

        private static void AppendCurrentPlayer(Side currentPlayer, StringBuilder sb)
        {
            char playerChar = currentPlayer switch
            {
                Side.White => 'w',
                Side.Black => 'b',
                _ => throw new ArgumentException($"Current player cannot be {currentPlayer}!")
            };

            sb.Append(playerChar);
        }

        private static void AppendCastlingRights(Board board, StringBuilder sb)
        {
            bool canWhiteCastleKS = board.HasCastleRightKS(Side.White);
            bool canWhiteCastleQS = board.HasCastleRightQS(Side.White);
            bool canBlackCastleKS = board.HasCastleRightKS(Side.Black);
            bool canBlackCastleQS = board.HasCastleRightQS(Side.Black);

            if (!(canWhiteCastleKS || canWhiteCastleQS || canBlackCastleKS || canBlackCastleQS))
            {
                sb.Append('-');
                return;
            }

            if (canWhiteCastleKS) sb.Append('K');
            if (canWhiteCastleQS) sb.Append('Q');
            if (canBlackCastleKS) sb.Append('k');
            if (canBlackCastleQS) sb.Append('q');
        }

        private static void AppendEnPassant(Side currentPlayer, Board board, StringBuilder sb)
        {
            if (!board.CanCaptureEnPassant(currentPlayer))
            {
                sb.Append('-');
                return;
            }

            Square square = board.GetPawnSkipSquare(currentPlayer.Opponent());
            sb.Append(square.Algebraic);
        }

        private static Piece SymbolToPiece(char symbol)
        {
            Side side = char.IsUpper(symbol) ? Side.White : Side.Black;
            
            foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
            {
                char typeSymbol = side switch
                {
                    Side.White => type.WhiteSymbol(),
                    Side.Black => type.BlackSymbol(),
                    _ => throw new ArgumentException($"Side of piece cannot be {side.ToString()}")
                };

                if (typeSymbol == symbol)
                    return type.Create(side);
            }

            return null;
        }

        private static char PieceToSymbol(Piece piece) => piece.side switch
        {
            Side.White => piece.Type.WhiteSymbol(),
            Side.Black => piece.Type.BlackSymbol(),
            _ => throw new ArgumentException($"Side of piece cannot be {piece.side.ToString()}")
        };
    }
}