using UnityEngine;
using UnityEngine.EventSystems;

namespace Chessed
{
    public class SquareInteractionHandler : MonoBehaviour, IPointerDownHandler
    {
        [HideInInspector] public int index;

        [SerializeField] private BoardInteractionHandler boardInteractionHandler;

        public void OnPointerDown(PointerEventData eventData) => boardInteractionHandler.OnSquareInteract(index);
    }
}
