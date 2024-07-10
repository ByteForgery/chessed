using Chessed.Logic;
using UnityEngine;

namespace Chessed
{
    public class BoardInteractionHandler : MonoBehaviour
    {
        [SerializeField] private GameManager manager;

        private GameState GameState => manager.GameState;
        private Board Board => GameState.Board;

        private SquareInteractionHandler[] squareInteractionHandlers;

        private void Awake()
        {
            squareInteractionHandlers = GetComponentsInChildren<SquareInteractionHandler>();
            
            for (int i = 0; i < 64; i++)
                squareInteractionHandlers[i].index = i;
        }

        public void OnSquareInteract(int index)
        {
            if (manager.runningPromotion != null) return;
            
            Square interactedSquare = Board.IndexToSquare(index);
            if (manager.selectedSquare == interactedSquare)
            {
                manager.selectedSquare = null;
                return;
            }

            if (manager.selectedSquare != null)
            {
                foreach (Move move in manager.GameState.LegalMovesForPiece(manager.selectedSquare))
                {
                    if (interactedSquare != move.To) continue;

                    if (move.Type == MoveType.Promotion)
                    {
                        manager.ExecutePromotion(move.From, move.To, manager.GameState.CurrentPlayer);
                    } else
                        manager.ExecuteMove(move);
                    
                    manager.selectedSquare = null;
                    return;
                }
            }

            Piece interactedPiece = Board[interactedSquare];
            if (interactedPiece != null && manager.GameState.CurrentPlayer != interactedPiece.side)
            {
                manager.selectedSquare = null;
                return;
            }
            
            manager.selectedSquare = interactedSquare;
        }
    }
}
