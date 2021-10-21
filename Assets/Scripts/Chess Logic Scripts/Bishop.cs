using System.Collections.Generic;
using UnityEngine;

namespace Practice.Chess
{
    public class Bishop : Piece
    {
        public override bool CheckIfCanAttackOpponentKing(Vector2Int kingPosition)
        {
            return Mathf.Abs(kingPosition.x - _boardPosition.x) == Mathf.Abs(kingPosition.y - _boardPosition.y);
        }

        public override List<Vector2Int> GetAvailablePositions(Board board)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            for (int i = _boardPosition.x - 1, j = _boardPosition.y - 1; i >= 0 && j >= 0; i--, j--)
            {
                Piece piece = board.Pieces[i, j];
                Vector2Int position = new Vector2Int(i, j);
                if (piece == null)
                    positions.Add(position);
                else
                {
                    if (piece.Color != _color)
                        positions.Add(position);
                    break;
                }
            }

            for (int i = _boardPosition.x - 1, j = _boardPosition.y + 1; i >= 0 && j < Board.BOARD_DIMENSION; i--, j++)
            {
                Piece piece = board.Pieces[i, j];
                Vector2Int position = new Vector2Int(i, j);
                if (piece == null)
                    positions.Add(position);
                else
                {
                    if (piece.Color != _color)
                        positions.Add(position);
                    break;
                }
            }

            for (int i = _boardPosition.x + 1, j = _boardPosition.y - 1; i < Board.BOARD_DIMENSION && j >= 0; i++, j--)
            {
                Piece piece = board.Pieces[i, j];
                Vector2Int position = new Vector2Int(i, j);
                if (piece == null)
                    positions.Add(position);
                else
                {
                    if (piece.Color != _color)
                        positions.Add(position);
                    break;
                }
            }

            for (int i = _boardPosition.x + 1, j = _boardPosition.y + 1; i < Board.BOARD_DIMENSION && j < Board.BOARD_DIMENSION; i++, j++)
            {
                Piece piece = board.Pieces[i, j];
                Vector2Int position = new Vector2Int(i, j);
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