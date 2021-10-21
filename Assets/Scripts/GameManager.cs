using UnityEngine;

namespace Practice.Chess
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _GM;

        private PlayerColor _activePlayer = PlayerColor.WHITE;

        public static GameManager GM
        {
            get
            {
                if (_GM == null)
                    _GM = FindObjectOfType<GameManager>();
                return _GM;
            }
        }

        public PlayerColor ActivePlayer { get { return _activePlayer; } }

        private void Awake()
        {
            if (_GM == null)
                _GM = this;
            else if (_GM != this)
                Destroy(gameObject);
        }
    }
}