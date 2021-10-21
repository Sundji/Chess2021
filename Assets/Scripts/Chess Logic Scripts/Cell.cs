using UnityEngine;

namespace Practice.Chess
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private GameObject _highlightObject = null;

        private Vector2Int _boardPosition;

        private bool _isHighlighted = false;

        public void SetBoardPosition(Vector2Int boardPosition)
        {
            _boardPosition = boardPosition;
        }

        public void ResetHighlight()
        {
            _isHighlighted = false;
            _highlightObject.SetActive(false);
        }

        public void SetHighlight()
        {
            _isHighlighted = true;
            _highlightObject.SetActive(true);
        }

        public void Select()
        {
            if (_isHighlighted)
                EventManager.EM.EventCellSelected.Invoke(_boardPosition);
        }
    }
}