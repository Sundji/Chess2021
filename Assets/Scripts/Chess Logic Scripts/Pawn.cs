using System.Collections.Generic;
using UnityEngine;

namespace Practice.Chess
{
    public class Pawn : Piece
    {
        private bool _wasMoved = false;

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

            // TODO: Check for en passant

            return positions;
        }

        public override void MovePiece(Board board, Vector2Int boardPosition, Vector3 worldPosition)
        {
            _wasMoved = true;
            base.MovePiece(board, boardPosition, worldPosition);
        }
    }
}