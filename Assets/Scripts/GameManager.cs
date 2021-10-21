using UnityEngine;

namespace Practice.Chess
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _GM;

        private PlayerColor _activePlayerColor = PlayerColor.WHITE;
        private Status _status = Status.IN_PROGRESS;

        public static GameManager GM
        {
            get
            {
                if (_GM == null)
                    _GM = FindObjectOfType<GameManager>();
                return _GM;
            }
        }

        public PlayerColor ActivePlayerColor { get { return _activePlayerColor; } }
        public Status Status { get { return _status; } }

        private void Awake()
        {
            if (_GM == null)
                _GM = this;
            else if (_GM != this)
                Destroy(gameObject);

            EventManager.EM.EventPlayerTurnEnded.AddListener(OnPlayerTurnEnded);
        }

        private void Start()
        {
            StartPlayerTurn();
        }

        private void OnDestroy()
        {
            if (EventManager.EM != null)
            {
                EventManager.EM.EventPlayerTurnEnded.RemoveListener(OnPlayerTurnEnded);
            }    
        }

        private void OnPlayerTurnEnded(PlayerColor color)
        {
            _activePlayerColor = color == PlayerColor.BLACK ? PlayerColor.WHITE : PlayerColor.BLACK;
            StartPlayerTurn();
        }

        private void OnStatusChange(Status status)
        {
            _status = status;
            Debug.Log(status);
        }

        private void StartPlayerTurn()
        {
            EventManager.EM.EventPlayerTurnStarted.Invoke(_activePlayerColor);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnPlayerTurnEnded(_activePlayerColor);
            }
        }
    }
}