using System.Collections.Generic;
using UnityEngine;

namespace Practice.Chess
{
    public class Pawn : Piece
    {
        private bool _wasMoved = false;

        private bool _canDoEnPassant = false;
        private Vector2Int _enPassantPosition;

        public override bool CheckIfCanAttackOpponentKing(Vector2Int kingPosition)
        {
            int directionFactor = _color == PlayerColor.WHITE ? 1 : -1;
            if (kingPosition == _boardPosition + new Vector2(1, 1 * directionFactor) || kingPosition == _boardPosition + new Vector2(-1, 1 * directionFactor))
                return true;
            return false;
        }

        public override List<Vector2Int> GetAvailablePositions(Board board)
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            int directionFactor = _color == PlayerColor.WHITE ? 1 : -1;

            int y = _boardPosition.y + 1 * directionFactor;
            if (y >= 0 && y < Board.BOARD_DIMENSION)
            {
                int x = _boardPosition.x;

                if (board.Pieces[x, y] == null)
                {
                    positions.Add(new Vector2Int(x, y));
                    y += 1 * directionFactor;
                    if (!_wasMoved && (y >= 0 && y < Board.BOARD_DIMENSION) && board.Pieces[x, y] == null)
                        positions.Add(new Vector2Int(x, y));
                    y -= 1 * directionFactor;
                }

                if (x - 1 >= 0 && (board.Pieces[x - 1, y] != null && board.Pieces[x - 1, y].Color != _color))
                    positions.Add(new Vector2Int(x - 1, y));

                if (x + 1 < Board.BOARD_DIMENSION && (board.Pieces[x + 1, y] != null && board.Pieces[x + 1, y].Color != _color))
                    positions.Add(new Vector2Int(x + 1, y));
            }

            Move lastMove = DataManager.DM.LastMove;
            if (lastMove != null)
            {
                PlayerColor color = (PlayerColor)System.Enum.Parse(typeof(PlayerColor), lastMove.Color);
                PieceType pieceType = (PieceType)System.Enum.Parse(typeof(PieceType), lastMove.Piece);
                if (color != _color && pieceType == PieceType.PAWN && Mathf.Abs(lastMove.PositionStart.y - lastMove.PositionEnd.y) == 2)
                {
                    if (lastMove.PositionEnd.y == _boardPosition.y && Mathf.Abs(lastMove.PositionEnd.x - _boardPosition.x) == 1)
                    {
                        _canDoEnPassant = true;
                        if (lastMove.PositionEnd.x < _boardPosition.x)
                            _enPassantPosition = _boardPosition + new Vector2Int(-1, 1 * directionFactor);
                        else
                            _enPassantPosition = _boardPosition + new Vector2Int(1, 1 * directionFactor);
                        positions.Add(_enPassantPosition);
                    }
                }
                else
                    _canDoEnPassant = false;
            }

            return positions;
        }

        public override void MovePiece(Board board, Vector2Int boardPosition, Vector3 worldPosition)
        {
            int directionFactor = _color == PlayerColor.BLACK ? -1 : 1;
            if (_canDoEnPassant && _enPassantPosition == boardPosition)
                board.Pieces[boardPosition.x, boardPosition.y - 1 * directionFactor].EatPiece(board);

            _wasMoved = true;
            base.MovePiece(board, boardPosition, worldPosition);

            if ((_color == PlayerColor.BLACK && _boardPosition.y == 0) || (_color == PlayerColor.WHITE && _boardPosition.y == Board.BOARD_DIMENSION - 1))
                EventManager.EM.EventWaitingForPromotion.Invoke(this);
        }

        public void Promote(Piece piece, Board board)
        {
            piece.MovePiece(board, _boardPosition, transform.position);
        }
    }
}