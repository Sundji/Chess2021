using System.Collections.Generic;
using UnityEngine;

namespace Practice.Chess
{
    public class Rook : Piece
    {
        public override bool CheckIfCanAttackOpponentKing(Vector2Int kingPosition)
        {
            return kingPosition.x == _boardPosition.x || kingPosition.y == _boardPosition.y;
        }

        public override List<Vector2Int> GetAvailablePositions(Board board)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            for (int i = _boardPosition.x - 1; i >= 0; i--)
            {
                Piece piece = board.Pieces[i, _boardPosition.y];
                Vector2Int position = new Vector2Int(i, _boardPosition.y);
                if (piece == null)
                    positions.Add(position);
                else
                {
                    if (piece.Color != _color)
                        positions.Add(position);
                    break;
                }
            }

            for (int i = _boardPosition.x + 1; i < Board.BOARD_DIMENSION; i++)
            {
                Piece piece = board.Pieces[i, _boardPosition.y];
                Vector2Int position = new Vector2Int(i, _boardPosition.y);
                if (piece == null)
                    positions.Add(position);
                else
                {
                    if (piece.Color != _color)
                        positions.Add(position);
                    break;
                }
            }

            for (int j = _boardPosition.y - 1; j >= 0; j--)
            {
                Piece piece = board.Pieces[_boardPosition.x, j];
                Vector2Int position = new Vector2Int(_boardPosition.x, j);
                if (piece == null)
                    positions.Add(position);
                else
                {
                    if (piece.Color != _color)
                        positions.Add(position);
                    break;
                }
            }

            for (int j = _boardPosition.y + 1; j < Board.BOARD_DIMENSION; j++)
            {
                Piece piece = board.Pieces[_boardPosition.x, j];
                Vector2Int position = new Vector2Int(_boardPosition.x, j);
                if (piece == null)
                    positions.Add(position);
                else
                {
                    if (piece.Color != _color)
                        positions.Add(position);
                    break;
                }
            }

            return positions;
        }
    }
}