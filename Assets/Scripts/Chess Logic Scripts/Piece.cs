using System.Collections.Generic;
using UnityEngine;

namespace Practice.Chess
{
    public abstract class Piece : MonoBehaviour
    {
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

            Vector2Int kingPosition = _color == PlayerColor.BLACK ? board.KingBlack.BoardPosition : board.KingWhite.BoardPosition;
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

            PieceType type = PieceType.BISHOP;
            if (this.GetType() == typeof(Bishop))
                type = PieceType.BISHOP;
            else if (this.GetType() == typeof(King))
                type = PieceType.KING;
            else if (this.GetType() == typeof(Knight))
                type = PieceType.KNIGHT;
            else if (this.GetType() == typeof(Pawn))
                type = PieceType.PAWN;
            else if (this.GetType() == typeof(Queen))
                type = PieceType.QUEEN;
            else
                type = PieceType.ROOK;
            DataManager.DM.AddMove(_boardPosition, Move.DELETION_MARK, _color == PlayerColor.BLACK ? PlayerColor.BLACK : PlayerColor.WHITE, type);

            Destroy(gameObject);
        }

        public virtual void MovePiece(Board board, Vector2Int boardPosition, Vector3 worldPosition)
        {
            if (board.Pieces[boardPosition.x, boardPosition.y] != null)
                board.Pieces[boardPosition.x, boardPosition.y].EatPiece(board);

            PieceType type = PieceType.BISHOP;
            if (this.GetType() == typeof(Bishop))
                type = PieceType.BISHOP;
            else if (this.GetType() == typeof(King))
                type = PieceType.KING;
            else if (this.GetType() == typeof(Knight))
                type = PieceType.KNIGHT;
            else if (this.GetType() == typeof(Pawn))
                type = PieceType.PAWN;
            else if (this.GetType() == typeof(Queen))
                type = PieceType.QUEEN;
            else
                type = PieceType.ROOK;
            DataManager.DM.AddMove(_boardPosition, boardPosition, _color, type);

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

        public void ResetHighlight()
        {
            _renderer.material.DisableKeyword("_EMISSION");
        }

        public void SetHighlight()
        {
            DesignData designData = DesignManager.DM.DesignData;
            _renderer.material.SetColor("_EMISISON", _color == PlayerColor.BLACK ? designData.EmissionColorBlack : designData.EmissionColorWhite);
            _renderer.material.EnableKeyword("_EMISSION");
        }

        public void SetUp(PlayerColor color, Vector2Int boardPosition)
        {
            DesignData designData = DesignManager.DM.DesignData;
            _color = color;
            _boardPosition = boardPosition;
            _renderer.material = color == PlayerColor.BLACK ? designData.MaterialBlack : designData.MaterialWhite;
        }
    }
}