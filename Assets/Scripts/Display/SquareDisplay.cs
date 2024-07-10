using Chessed.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Chessed
{
    public class SquareDisplay : MonoBehaviour
    {
        [SerializeField] private Image squareImage;
        [SerializeField] private Image pieceImage;
        [SerializeField] private GameObject validMoveHighlight, validCaptureHighlight;

        public Color Color
        {
            get => squareImage.color;
            set => squareImage.color = value;
        }

        public Sprite PieceSprite
        {
            get => pieceImage.sprite;
            set
            {
                pieceImage.gameObject.SetActive(value != null);
                pieceImage.sprite = value;
            }
        }

        public void ShowValidMove() => validMoveHighlight.SetActive(true);
        public void ShowValidCapture() => validCaptureHighlight.SetActive(true);

        public void HideValidMove()
        {
            validMoveHighlight.SetActive(false);
            validCaptureHighlight.SetActive(false);
        }
    }
}
