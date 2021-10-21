using UnityEngine;

namespace Practice.Chess
{
    public class EventManager : MonoBehaviour
    {
        public CustomEvent<Vector2Int> EventPieceSelected = new CustomEvent<Vector2Int>();

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