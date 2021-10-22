using UnityEngine;

namespace Practice.Chess
{
    public class DataManager : MonoBehaviour
    {
        private static DataManager _DM;

        private MoveLibrary _moveLibrary;

        public static DataManager DM
        {
            get
            {
                if (_DM == null)
                    _DM = FindObjectOfType<DataManager>();
                return _DM;
            }
        }

        public Move LastMove { get { return _moveLibrary.Moves.Count > 0 ? _moveLibrary.Moves[_moveLibrary.Moves.Count - 1] : null; } }
        public MoveLibrary MoveLibrary { get { return _moveLibrary; } }

        private void Awake()
        {
            if (_DM == null)
                _DM = this;
            else if (_DM != this)
                Destroy(gameObject);
            _moveLibrary = new MoveLibrary();

            if (EventManager.EM != null)
                EventManager.EM.EventStatusChanged.AddListener(OnStatusChanged);
        }

        private void OnDestroy()
        {
            if (EventManager.EM != null)
                EventManager.EM.EventStatusChanged.RemoveListener(OnStatusChanged);
        }

        private void OnStatusChanged(Status status)
        {
            if (status == Status.CHECK_MATE || status == Status.FORFEIT || status == Status.STALEMATE)
            {
                _moveLibrary.Status = status.ToString();
                if (_moveLibrary.Moves.Count > 0)
                    DataReaderAndWriter.SaveMoveLibrary(_moveLibrary);
            }
        }

        public void AddMove(Vector2Int positionStart, Vector2Int positionEnd, PlayerColor playerColor, PieceType pieceType)
        {
            _moveLibrary.AddMove(positionStart, positionEnd, playerColor, pieceType);
        }

        public void LoadMoveLibrary()
        {
            if (!DataReaderAndWriter.TryLoadMoveLibrary(out _moveLibrary))
                _moveLibrary = new MoveLibrary();
        }
    }
}
