using System.Collections.Generic;
using UnityEngine;

namespace Practice.Chess
{
    public class Knight : Piece
    {
        public override bool CheckIfCanAttackOpponentKing(Vector2Int kingPosition)
        {
            return false;
        }

        public override List<Vector2Int> GetAvailablePositions(Board board)
        {
            List<Vector2Int> positions = new List<Vector2Int>
            {
                _boardPosition + new Vector2Int(-2, -1),
                _boardPosition + new Vector2Int(-2, 1),
                _boardPosition + new Vector2Int(2, -1),
                _boardPosition + new Vector2Int(2, 1),
                _boardPosition + new Vector2Int(-1, -2),
                _boardPosition + new Vector2Int(-1, 2),
                _boardPosition + new Vector2Int(1, -2),
                _boardPosition + new Vector2Int(1, 2)
            };

            for (int index = positions.Count - 1; index >= 0; index--)
            {
                Vector2Int position = positions[index];
                if (position.x < 0 || position.x >= Board.BOARD_DIMENSION || position.y < 0 || position.y >= Board.BOARD_DIMENSION)
                    positions.RemoveAt(index);
                else if (board.Pieces[position.x, position.y] != null && board.Pieces[position.x, position.y].Color == _color)
                    positions.RemoveAt(index);
            }

            return positions;
        }
    }
}