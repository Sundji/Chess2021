using System.Collections.Generic;
using UnityEngine;

namespace Practice.Chess
{
    [System.Serializable]
    public class MoveLibrary
    {
        public List<Move> Moves = new List<Move>();
        public string Status;

        public void AddMove(Vector2Int positionStart, Vector2Int positionEnd, PlayerColor color, PieceType pieceType)
        {
            string colorAsString = color.ToString();
            string pieceTypeAsString = pieceType.ToString();
            Moves.Add(new Move(positionStart, positionEnd, colorAsString, pieceTypeAsString));
        }
    }
}