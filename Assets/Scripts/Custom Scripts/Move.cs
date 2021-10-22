using UnityEngine;

namespace Practice.Chess
{
    [System.Serializable]
    public class Move
    {
        public static readonly Vector2Int DELETION_MARK = new Vector2Int(-1, -1);

        public Vector2Int PositionStart;
        public Vector2Int PositionEnd;
        public string Color;
        public string Piece;

        public Move(Vector2Int positionStart, Vector2Int positionEnd, string color, string piece)
        {
            PositionStart = positionStart;
            PositionEnd = positionEnd;
            Color = color;
            Piece = piece;
        }
    }
}