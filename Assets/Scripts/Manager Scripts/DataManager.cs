using UnityEngine;

namespace Practice.Chess
{
    public class DataManager : MonoBehaviour
    {
        private static DataManager _DM;

        private MoveLibrary _moveLibary;

        public static DataManager DM
        {
            get
            {
                if (_DM == null)
                    _DM = FindObjectOfType<DataManager>();
                return _DM;
            }
        }

        public Move LastMove { get { return _moveLibary.Moves.Count > 0 ? _moveLibary.Moves[_moveLibary.Moves.Count - 1] : null; } }

        private void Awake()
        {
            if (_DM == null)
                _DM = this;
            else if (_DM != this)
                Destroy(gameObject);

            _moveLibary = new MoveLibrary();
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
                _moveLibary.Status = status.ToString();
                DataReaderAndWriter.SaveMoveLibrary(_moveLibary);
            }
        }

        public void AddMove(Vector2Int positionStart, Vector2Int positionEnd, PlayerColor playerColor, PieceType pieceType)
        {
            _moveLibary.AddMove(positionStart, positionEnd, playerColor, pieceType);
        }
    }
}
