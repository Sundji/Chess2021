using UnityEngine;

namespace Practice.Chess
{
    public class EventManager : MonoBehaviour
    {
        public CustomEvent<Vector2Int> EventCellSelected = new CustomEvent<Vector2Int>();
        public CustomEvent<Vector2Int> EventPieceSelected = new CustomEvent<Vector2Int>();

        public CustomEvent<PlayerColor> EventPlayerTurnEnded = new CustomEvent<PlayerColor>();
        public CustomEvent<PlayerColor> EventPlayerTurnStarted = new CustomEvent<PlayerColor>();

        public CustomEvent<Status> EventStatusChanged = new CustomEvent<Status>();

        public CustomEvent<PromotionPieceType> EventPromotionPieceChosen = new CustomEvent<PromotionPieceType>();
        public CustomEvent<Pawn> EventWaitingForPromotion = new CustomEvent<Pawn>();

        private static EventManager _EM;

        public static EventManager EM
        {
            get
            {
                if (_EM == null)
                    _EM = FindObjectOfType<EventManager>();
                return _EM;
            }
        }

        private void Awake()
        {
            if (_EM == null)
                _EM = this;
            else if (_EM != this)
                Destroy(gameObject);
        }
    }
}