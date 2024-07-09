using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private BoardDisplay boardDisplay;
    
    public void HandleMove(int fromX, int fromY, int toX, int toY)
    {
        Piece piece = board.GetPieceOnCoord(fromX, fromY);
        
    }
}
