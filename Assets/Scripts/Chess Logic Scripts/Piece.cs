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

        protected PlayerColor _color = PlayerColor.WHITE;

        protected Vector2Int _boardPosition;

        public PlayerColor Color { get { return _color; } }

        protected void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        public abstract List<Vector2Int> GetAvailablePositions(Board board);

        public void Deselect()
        {
            _renderer.material.DisableKeyword("_EMISSION");
        }

        public void Select()
        {
            _renderer.material.SetColor("_EMISISON", _color == PlayerColor.WHITE ? _emissionColorWhite : _emissionColorBlack);
            _renderer.material.EnableKeyword("_EMISSION");
            EventManager.EM.EventPieceSelected.Invoke(_boardPosition);
        }

        public void SetUp(PlayerColor color, Vector2Int boardPosition)
        {
            _color = color;
            _boardPosition = boardPosition;
            _renderer.material = color == PlayerColor.BLACK ? _materialBlack : _materialWhite;
        }
    }
}