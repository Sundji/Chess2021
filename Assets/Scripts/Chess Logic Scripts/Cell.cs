using UnityEngine;

namespace Practice.Chess
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private GameObject _highlightObject = null;

        private Vector2Int _boardPosition;
        private Vector3 _worldPosition;

        public Vector3 WorldPosition { get { return _worldPosition; } }

        public void SetBoardPosition(Vector2Int boardPosition)
        {
            _boardPosition = boardPosition;
            _worldPosition = transform.position;
        }

        public void ResetHighlight()
        {
            _highlightObject.SetActive(false);
        }

        public void SetHighlight()
        {
            _highlightObject.SetActive(true);
        }

        public void Select()
        {
            EventManager.EM.EventCellSelected.Invoke(_boardPosition);
        }
    }
}