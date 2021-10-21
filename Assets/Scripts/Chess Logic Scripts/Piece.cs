using System.Collections.Generic;
using UnityEngine;

namespace Practice.Chess
{
    public abstract class Piece : MonoBehaviour
    {
        [SerializeField] protected Material _materialBlack = null;
        [SerializeField] protected Material _materialWhite = null;
        [SerializeField] protected Color _emissionColorBlack = new Color(255, 255, 255);
        [SerializeField] protected Color _emissionColorWhite = new Color(255, 255, 255);

        protected Renderer _renderer;
        protected Transform _transform;

        protected PlayerColor _color = PlayerColor.WHITE;

        protected Vector2Int _boardPosition;

        public PlayerColor Color { get { return _color; } }

        protected void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _transform = transform;
        }

        protected virtual bool CheckIfMovingAffectsSameKing(Vector2Int oldPosition, Vector2Int newPosition, Board board)
        {
            Piece pieceAtNewPosition = board.Pieces[newPosition.x, newPosition.y];
            board.Pieces[newPosition.x, newPosition.y] = board.Pieces[oldPosition.x, oldPosition.y];
            board.Pieces[oldPosition.x, oldPosition.y] = null;
            _boardPosition = newPosition;

            Vector2Int kingPosition = _color == PlayerColor.BLACK ? board.KingBlackPosition : board.KingWhitePosition;
            King king = (King)(board.Pieces[kingPosition.x, kingPosition.y]);
            bool isKingSafe = king.CheckIfSafe(board);

            _boardPosition = oldPosition;
            board.Pieces[oldPosition.x, oldPosition.y] = board.Pieces[newPosition.x, newPosition.y];
            board.Pieces[newPosition.x, newPosition.y] = pieceAtNewPosition;

            return !isKingSafe;
        }

        public abstract bool CheckIfCanAttackOpponentKing(Vector2Int kingPosition);

        public abstract List<Vector2Int> GetAvailablePositions(Board board);

        public virtual void EatPiece(Board board)
        {
            board.Pieces[_boardPosition.x, _boardPosition.y] = null;
            Destroy(gameObject);
        }

        public virtual void MovePiece(Board board, Vector2Int boardPosition, Vector3 worldPosition)
        {
            if (board.Pieces[boardPosition.x, boardPosition.y] != null)
                board.Pieces[boardPosition.x, boardPosition.y].EatPiece(board);

            board.Pieces[boardPosition.x, boardPosition.y] = this;
            board.Pieces[_boardPosition.x, _boardPosition.y] = null;
            _boardPosition = boardPosition;
            _transform.position = worldPosition;
        }

        public List<Vector2Int> GetLegalPositions(Board board)
        {
            List<Vector2Int> positions = GetAvailablePositions(board);

            for (int index = positions.Count - 1; index >= 0; index--)
            {
                if (CheckIfMovingAffectsSameKing(_boardPosition, positions[index], board))
                    positions.RemoveAt(index);
            }

            return positions;
        }

        public void Deselect()
        {
            _renderer.material.DisableKeyword("_EMISSION");
        }

        public void Select()
        {
            if (GameManager.GM.ActivePlayerColor == _color)
            {
                _renderer.material.SetColor("_EMISISON", _color == PlayerColor.WHITE ? _emissionColorWhite : _emissionColorBlack);
                _renderer.material.EnableKeyword("_EMISSION");
                EventManager.EM.EventPieceSelected.Invoke(_boardPosition);
            }
        }

        public void SetUp(PlayerColor color, Vector2Int boardPosition)
        {
            _color = color;
            _boardPosition = boardPosition;
            _renderer.material = color == PlayerColor.BLACK ? _materialBlack : _materialWhite;
        }
    }
}