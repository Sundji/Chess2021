using UnityEngine;

namespace Practice.Chess
{
    public class ReplayBoard : MonoBehaviour
    {
        public const int BOARD_DIMENSION = 8;

        [Header("Cells Information")]
        [SerializeField] private float _cellsDistance = 1;
        [SerializeField] private Vector3 _cellsStartPosition = new Vector3();

        [Header("Pieces Information")]
        [SerializeField] private Piece _bishopPrefab = null;
        [SerializeField] private Piece _kingPrefab = null;
        [SerializeField] private Piece _knightPrefab = null;
        [SerializeField] private Piece _pawnPrefab = null;
        [SerializeField] private Piece _queenPrefab = null;
        [SerializeField] private Piece _rookPrefab = null;

        private Transform _transform;

        private Piece[,] _pieces = new Piece[BOARD_DIMENSION, BOARD_DIMENSION];

        private void Awake()
        {
            _transform = transform;
            InitializePieces();

            DesignData designData = DesignManager.DM.DesignData;
            GetComponent<Renderer>().materials = new Material[2] { designData.MaterialBlack, designData.MaterialWhite };
        }

        private void InitializePieces()
        {
            for (int i = 0; i < BOARD_DIMENSION; i++)
            {
                _pieces[i, 1] = CreatePiece(_pawnPrefab, PlayerColor.WHITE, new Vector2Int(i, 1));
                _pieces[i, 6] = CreatePiece(_pawnPrefab, PlayerColor.BLACK, new Vector2Int(i, 6));
            }

            _pieces[0, 0] = CreatePiece(_rookPrefab, PlayerColor.WHITE, new Vector2Int(0, 0));
            _pieces[7, 0] = CreatePiece(_rookPrefab, PlayerColor.WHITE, new Vector2Int(7, 0));
            _pieces[0, 7] = CreatePiece(_rookPrefab, PlayerColor.BLACK, new Vector2Int(0, 7));
            _pieces[7, 7] = CreatePiece(_rookPrefab, PlayerColor.BLACK, new Vector2Int(7, 7));

            _pieces[1, 0] = CreatePiece(_knightPrefab, PlayerColor.WHITE, new Vector2Int(1, 0));
            _pieces[6, 0] = CreatePiece(_knightPrefab, PlayerColor.WHITE, new Vector2Int(6, 0));
            _pieces[1, 7] = CreatePiece(_knightPrefab, PlayerColor.BLACK, new Vector2Int(1, 7));
            _pieces[6, 7] = CreatePiece(_knightPrefab, PlayerColor.BLACK, new Vector2Int(6, 7));

            _pieces[2, 0] = CreatePiece(_bishopPrefab, PlayerColor.WHITE, new Vector2Int(2, 0));
            _pieces[5, 0] = CreatePiece(_bishopPrefab, PlayerColor.WHITE, new Vector2Int(5, 0));
            _pieces[2, 7] = CreatePiece(_bishopPrefab, PlayerColor.BLACK, new Vector2Int(2, 7));
            _pieces[5, 7] = CreatePiece(_bishopPrefab, PlayerColor.BLACK, new Vector2Int(5, 7));

            _pieces[3, 0] = CreatePiece(_queenPrefab, PlayerColor.WHITE, new Vector2Int(3, 0));
            _pieces[3, 7] = CreatePiece(_queenPrefab, PlayerColor.BLACK, new Vector2Int(3, 7));

            _pieces[4, 0] = CreatePiece(_kingPrefab, PlayerColor.WHITE, new Vector2Int(4, 0));
            _pieces[4, 7] = CreatePiece(_kingPrefab, PlayerColor.BLACK, new Vector2Int(4, 7));
        }

        public void CreatePiece(PieceType type, PlayerColor color, Vector2Int boardPosition)
        {
            Piece piece = null;

            if (type == PieceType.BISHOP)
                piece = CreatePiece(_bishopPrefab, color, boardPosition);
            if (type == PieceType.KING)
                piece = CreatePiece(_kingPrefab, color, boardPosition);
            if (type == PieceType.KNIGHT)
                piece = CreatePiece(_knightPrefab, color, boardPosition);
            if (type == PieceType.PAWN)
                piece = CreatePiece(_pawnPrefab, color, boardPosition);
            if (type == PieceType.QUEEN)
                piece = CreatePiece(_queenPrefab, color, boardPosition);
            if (type == PieceType.ROOK)
                piece = CreatePiece(_rookPrefab, color, boardPosition);

            _pieces[boardPosition.x, boardPosition.y] = piece;
        }

        public Piece CreatePiece(Piece prefab, PlayerColor color, Vector2Int boardPosition)
        {
            Vector3 position = _cellsStartPosition + new Vector3(boardPosition.x, 0, boardPosition.y) * _cellsDistance;
            Quaternion rotation = color == PlayerColor.WHITE ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
            Piece piece = Instantiate(prefab, position, rotation, _transform);
            piece.SetUp(color, boardPosition);
            return piece;
        }

        public void EatPiece(Vector2Int positionStart)
        {
            Piece piece = _pieces[positionStart.x, positionStart.y];
            _pieces[positionStart.x, positionStart.y] = null;
            Destroy(piece.gameObject);
        }

        public void MovePiece(Vector2Int positionStart, Vector2Int positionEnd)
        {
            _pieces[positionEnd.x, positionEnd.y] = _pieces[positionStart.x, positionStart.y];
            _pieces[positionStart.x, positionStart.y] = null;
            _pieces[positionEnd.x, positionEnd.y].transform.position = _cellsStartPosition + new Vector3(positionEnd.x, 0, positionEnd.y) * _cellsDistance;
        }
    }
}