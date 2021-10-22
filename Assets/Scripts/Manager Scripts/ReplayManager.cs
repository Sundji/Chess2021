using System.Collections;
using UnityEngine;

namespace Practice.Chess
{
    public class ReplayManager : MonoBehaviour
    {
        [SerializeField] private ReplayBoard _replayBoard = null;

        private static ReplayManager _RM;

        public static ReplayManager RM
        {
            get
            {
                if (_RM == null)
                    _RM = FindObjectOfType<ReplayManager>();
                return _RM;
            }
        }

        private IEnumerator _coroutine;

        private MoveLibrary _moveLibrary;
        private int _index = 0;

        private void Awake()
        {
            if (_RM == null)
                _RM = this;
            else if (RM != this)
                Destroy(gameObject);

            DataManager.DM.LoadMoveLibrary();
            _moveLibrary = DataManager.DM.MoveLibrary;
        }

        private IEnumerator Autoplay(float timeDistance)
        {
            while (_index < _moveLibrary.Moves.Count)
            {
                yield return new WaitForSeconds(timeDistance);
                NextMove();
            }
        }

        public void NextMove()
        {
            if (_index >= _moveLibrary.Moves.Count)
                return;

            Move move = _moveLibrary.Moves[_index];
            _index++;

            if (move.PositionEnd == Move.DELETION_MARK)
            {
                _replayBoard.EatPiece(move.PositionStart);
                NextMove();
            }
            else
                _replayBoard.MovePiece(move.PositionStart, move.PositionEnd);
        }

        public void PreviousMove()
        {
            if (_index <= 0)
                return;

            _index--;
            Move move = _moveLibrary.Moves[_index];

            if (move.PositionEnd == Move.DELETION_MARK)
            {
                PlayerColor color = (PlayerColor)System.Enum.Parse(typeof(PlayerColor), move.Color);
                PieceType pieceType = (PieceType)System.Enum.Parse(typeof(PieceType), move.Piece);
                _replayBoard.CreatePiece(pieceType, color, move.PositionStart);
            }
            else
                _replayBoard.MovePiece(move.PositionEnd, move.PositionStart);

            if (_index - 1 >= 0 && _moveLibrary.Moves[_index - 1].PositionEnd == Move.DELETION_MARK)
                PreviousMove();
        }

        public void StartAutoplay(float timeDistance)
        {
            StartCoroutine("Autoplay", timeDistance);
        }

        public void StopAutoplay()
        {
            StopCoroutine("Autoplay");
        }
    }
}