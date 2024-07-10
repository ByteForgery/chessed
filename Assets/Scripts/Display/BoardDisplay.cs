using System;
using UnityEngine;
using Chessed.Logic;

namespace Chessed
{
    public class BoardDisplay : MonoBehaviour
    {
        [SerializeField] private Color lightSquareColor = Color.white, darkSquareColor = Color.black;
        [SerializeField] private PieceSpriteSet whitePieceSprites, blackPieceSprites;
        [SerializeField] private GameManager manager;

        private SquareDisplay[] squareDisplays;

        private GameState GameState => manager.GameState;
        private Board Board => GameState.Board;

        private void Awake()
        {
            squareDisplays = GetComponentsInChildren<SquareDisplay>();
        }

        private void Update()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    UpdatePieceDisplay(x, y);
                }
            }
            
            UpdateValidMoveHighlights(manager.selectedSquare);
        }

        private void UpdatePieceDisplay(int x, int y)
        {
            int index = Board.PosToIndex(x, y);

            Piece piece = Board[x, y];
            if (piece == null)
            {
                squareDisplays[index].PieceSprite = null;
                return;
            }
                    
            PieceSpriteSet spriteSet = piece.side switch
            {
                Side.White => whitePieceSprites,
                Side.Black => blackPieceSprites
            };

            squareDisplays[index].PieceSprite = spriteSet[piece.Type];
        }

        private void UpdateValidMoveHighlights(Square square)
        {
            foreach (SquareDisplay squareDisplay in squareDisplays)
                squareDisplay.HideValidMove();

            if (square == null) return;
            
            if (Board[square] == null) return;
            
            foreach (Move move in GameState.LegalMovesForPiece(square))
            {
                if (!move.To.IsValid) continue;
                
                int index = Board.SquareToIndex(move.To);
                SquareDisplay squareDisplay = squareDisplays[index];
                
                Piece toPiece = Board[move.To];
                if (toPiece == null)
                    squareDisplay.ShowValidMove();
                else
                    squareDisplay.ShowValidCapture();
            }
        }

        private void OnValidate()
        {
            SquareDisplay[] squareDisplays = GetComponentsInChildren<SquareDisplay>();
            for (int i = 0; i < 64; i++)
            {
                Square pos = Board.IndexToSquare(i);

                Color squareColor = pos.Color switch
                {
                    Side.White => lightSquareColor,
                    Side.Black => darkSquareColor,
                    _ => Color.magenta
                };

                squareDisplays[i].Color = squareColor;
            }
        }
    }

    [Serializable]
    public struct PieceSpriteSet
    {
        [SerializeField] private Sprite pawn, bishop, knight, rook, queen, king;

        public Sprite this[PieceType type] => type switch
        {
            PieceType.Pawn => pawn,
            PieceType.Knight => knight,
            PieceType.Bishop => bishop,
            PieceType.Rook => rook,
            PieceType.Queen => queen,
            PieceType.King => king,
            _ => null
        };
    }
}
