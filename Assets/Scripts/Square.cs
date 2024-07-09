using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour
{
    [SerializeField] private Image squareImage;
    [SerializeField] private Image pieceImage;

    public Image SquareImage => squareImage;
    public Image PieceImage => pieceImage;

    private void Update()
    {
        bool hasSprite = PieceImage.sprite != null;
        PieceImage.color = hasSprite ? Color.white : Color.clear;
    }
}
