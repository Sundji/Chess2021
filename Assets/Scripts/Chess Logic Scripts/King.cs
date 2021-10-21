using System.Collections.Generic;
using UnityEngine;

namespace Practice.Chess
{
    public class King : Piece
    {
        public Vector2Int BoardPosition { get { return _boardPosition; } }

        public override bool CheckIfCanAttackOpponentKing(Vector2Int kingPosition)
        {
            return !(Mathf.Abs(_boardPosition.x - kingPosition.x) > 1 || Mathf.Abs(_boardPosition.y - kingPosition.y) > 1);
        }

        protected override bool CheckIfMovingAffectsSameKing(Vector2Int oldPosition, Vector2Int newPosition, Board board)
        {
            Piece pieceAtNewPosition = board.Pieces[newPosition.x, newPosition.y];
            board.Pieces[newPosition.x, newPosition.y] = board.Pieces[oldPosition.x, oldPosition.y];
            board.Pieces[oldPosition.x, oldPosition.y] = null;
            _boardPosition = newPosition;
            
            bool isSafe = CheckIfSafe(board);

            _boardPosition = oldPosition;
            board.Pieces[oldPosition.x, oldPosition.y] = board.Pieces[newPosition.x, newPosition.y];
            board.Pieces[newPosition.x, newPosition.y] = pieceAtNewPosition;

            return !isSafe;
        }

        public override List<Vector2Int> GetAvailablePositions(Board board)
        {
            List<Vector2Int> positions = new List<Vector2Int>()
            {
                _boardPosition + new Vector2Int(-1, -1),
                _boardPosition + new Vector2Int(-1, 0),
                _boardPosition + new Vector2Int(-1, 1),
                _boardPosition + new Vector2Int(0, -1),
                _boardPosition + new Vector2Int(0, 1),
                _boardPosition + new Vector2Int(1, -1),
                _boardPosition + new Vector2Int(1, 0),
                _boardPosition + new Vector2Int(1, 1)
            };

            for (int index = positions.Count - 1; index >= 0; index--)
            {
                Vector2Int position = positions[index];
                if (position.x < 0 || position.x >= Board.BOARD_DIMENSION || position.y < 0 || position.y >= Board.BOARD_DIMENSION)
                    positions.RemoveAt(index);
                else if (board.Pieces[position.x, position.y] != null && board.Pieces[position.x, position.y].Color == _color)
                    positions.RemoveAt(index);
            }

            // TODO: Check if castling available

            return positions;
        }

        public bool CheckIfSafe(Board board)
        {
            int x = _boardPosition.x;
            int y = _boardPosition.y;

            #region Check left, right, down & up

            for (int i = x - 1; i >= 0; i--)
            {
                Piece piece = board.Pieces[i, y];
                if (piece != null)
                {
                    if (piece.Color != _color)
                        if (piece.CheckIfCanAttackOpponentKing(_boardPosition))
                            return false;
                    break;
                }
            }

            for (int i = x + 1; i < Board.BOARD_DIMENSION; i++)
            {
                Piece piece = board.Pieces[i, y];
                if (piece != null)
                {
                    if (piece.Color != _color)
                        if (piece.CheckIfCanAttackOpponentKing(_boardPosition))
                            return false;
                    break;
                }
            }

            for (int j = y - 1; j >= 0; j--)
            {
                Piece piece = board.Pieces[x, j];
                if (piece != null)
                {
                    if (piece.Color != _color)
                        if (piece.CheckIfCanAttackOpponentKing(_boardPosition))
                            return false;
                    break;
                }
            }

            for (int j = y + 1; j < Board.BOARD_DIMENSION; j++)
            {
                Piece piece = board.Pieces[x, j];
                if (piece != null)
                {
                    if (piece.Color != _color)
                        if (piece.CheckIfCanAttackOpponentKing(_boardPosition))
                            return false;
                    break;
                }
            }

            #endregion

            #region Check diagonals

            for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
            {
                Piece piece = board.Pieces[i, j];
                if (piece != null)
                {
                    if (piece.Color != _color)
                        if (piece.CheckIfCanAttackOpponentKing(_boardPosition))
                            return false;
                    break;
                }
            }

            for (int i = x - 1, j = y + 1; i >= 0 && j < Board.BOARD_DIMENSION; i--, j++)
            {
                Piece piece = board.Pieces[i, j];
                if (piece != null)
                {
                    if (piece.Color != _color)
                        if (piece.CheckIfCanAttackOpponentKing(_boardPosition))
                            return false;
                    break;
                }
            }

            for (int i = x + 1, j = y - 1; i < Board.BOARD_DIMENSION && j >= 0; i++, j--)
            {
                Piece piece = board.Pieces[i, j];
                if (piece != null)
                {
                    if (piece.Color != _color)
                        if (piece.CheckIfCanAttackOpponentKing(_boardPosition))
                            return false;
                    break;
                }
            }

            for (int i = x + 1, j = y + 1; i < Board.BOARD_DIMENSION && j < Board.BOARD_DIMENSION; i--, j--)
            {
                Piece piece = board.Pieces[i, j];
                if (piece != null)
                {
                    if (piece.Color != _color)
                        if (piece.CheckIfCanAttackOpponentKing(_boardPosition))
                            return false;
                    break;
                }
            }

            #endregion

            #region Check knights

            foreach (Vector2Int knightPosition in _color == PlayerColor.BLACK ? board.KnightsWhitePositions : board.KnightsBlackPositions)
            {
                Knight knight = (Knight)board.Pieces[knightPosition.x, knightPosition.y];
                if (knight.GetAvailablePositions(board).Contains(_boardPosition))
                {
                    Debug.Log("Danger from cell " + knightPosition);
                    return false;
                }
            }

            #endregion

            return true;
        }
    }
}