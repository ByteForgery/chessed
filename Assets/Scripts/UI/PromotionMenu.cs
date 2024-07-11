using Chessed.Logic;
using UnityEngine;

namespace Chessed
{
    public class PromotionMenu : MonoBehaviour
    {
        [SerializeField] private GameObject title, whiteMenu, blackMenu;
        [SerializeField] private GameManager manager;

        private void Update()
        {
            Promotion promotion = manager.runningPromotion;
            if (promotion == null)
            {
                title.SetActive(false);
                whiteMenu.SetActive(false);
                blackMenu.SetActive(false);
                return;
            }
            
            title.SetActive(true);
            GameObject menu = promotion.Side switch
            {
                Side.White => whiteMenu,
                Side.Black => blackMenu,
                _ => null
            };

            if (menu == null) return;
            
            menu.SetActive(true);
        }

        public void OnPromoteToKnight() => OnPromote(PromotionType.Knight);
        public void OnPromoteToBishop() => OnPromote(PromotionType.Bishop);
        public void OnPromoteToRook() => OnPromote(PromotionType.Rook);
        public void OnPromoteToQueen() => OnPromote(PromotionType.Queen);

        public void OnPromote(PromotionType type)
        {
            manager.runningPromotion.type = type;
            manager.GameState.HandlePromotion(manager.runningPromotion);
            manager.runningPromotion = null;
        }
    }
}