using System.Collections.Generic;
using UnityEngine;

namespace Practice.Chess
{
    public class Board : MonoBehaviour
    {
        public const int BOARD_DIMENSION = 8;

        [Header("Cells Information")]
        [SerializeField] private float _cellsDistance = 1;
        [SerializeField] private Cell _cellPrefab = null;
        [SerializeField] private Vector3 _cellsStartPosition = new Vector3();

        [Header("Pieces Information")]
        [SerializeField] private Piece _bishopPrefab = null;
        [SerializeField] private Piece _kingPrefab = null;
        [SerializeField] private Piece _knightPrefab = null;
        [SerializeField] private Piece _pawnPrefab = null;
        [SerializeField] private Piece _queenPrefab = null;
        [SerializeField] private Piece _rookPrefab = null;

        private Transform _transform;

        private Cell[,] _cells = new Cell[BOARD_DIMENSION, BOARD_DIMENSION];
        private List<Vector2Int> _highlightedCellsPositions = new List<Vector2Int>();

        private Piece[,] _pieces = new Piece[BOARD_DIMENSION, BOARD_DIMENSION];
        private Vector2Int _kingBlackPosition;
        private Vector2Int _kingWhitePosition;

        private bool _isPieceSelected = false;
        private Vector2Int _selectedPiecePosition;

        public Vector2Int KingBlackPosition { get { return _kingBlackPosition; } }
        public Vector2Int KingWhitePosition { get { return _kingWhitePosition; } }

        private void Awake()
        {
            _transform = transform;
            InitializeCells();
            InitializePieces();
        }

        private void Start()
        {
            EventManager.EM.EventCellSelected.AddListener(OnCellSelected);
            EventManager.EM.EventPieceSelected.AddListener(OnPieceSelected);
        }

        private void OnDestroy()
        {
            if (EventManager.EM != null)
            {
                EventManager.EM.EventCellSelected.RemoveListener(OnCellSelected);
                EventManager.EM.EventPieceSelected.RemoveListener(OnPieceSelected);
            }
        }

        #region Methods for board initalization

        private Piece CreatePiece(Piece prefab, PlayerColor color, Vector2Int boardPosition)
        {
            Vector3 position = _cellsStartPosition + new Vector3(boardPosition.x, 0, boardPosition.y) * _cellsDistance;
            Quaternion rotation = color == PlayerColor.WHITE ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

            Piece piece = Instantiate(prefab, position, rotation, _transform);
            piece.SetUp(color, boardPosition);

            return piece;
        }

        private void InitializePieces()
        {
            // Pawns
            for (int i = 0; i < BOARD_DIMENSION; i++)
            {
                _pieces[i, 1] = CreatePiece(_pawnPrefab, PlayerColor.WHITE, new Vector2Int(i, 1));
                _pieces[i, 6] = CreatePiece(_pawnPrefab, PlayerColor.BLACK, new Vector2Int(i, 6));
            }

            // Rooks
            _pieces[0, 0] = CreatePiece(_rookPrefab, PlayerColor.WHITE, new Vector2Int(0, 0));
            _pieces[7, 0] = CreatePiece(_rookPrefab, PlayerColor.WHITE, new Vector2Int(7, 0));
            _pieces[0, 7] = CreatePiece(_rookPrefab, PlayerColor.BLACK, new Vector2Int(0, 7));
            _pieces[7, 7] = CreatePiece(_rookPrefab, PlayerColor.BLACK, new Vector2Int(7, 7));

            // Knights
            _pieces[1, 0] = CreatePiece(_knightPrefab, PlayerColor.WHITE, new Vector2Int(1, 0));
            _pieces[6, 0] = CreatePiece(_knightPrefab, PlayerColor.WHITE, new Vector2Int(6, 0));
            _pieces[1, 7] = CreatePiece(_knightPrefab, PlayerColor.BLACK, new Vector2Int(1, 7));
            _pieces[6, 7] = CreatePiece(_knightPrefab, PlayerColor.BLACK, new Vector2Int(6, 7));

            // Bishops
            _pieces[2, 0] = CreatePiece(_bishopPrefab, PlayerColor.WHITE, new Vector2Int(2, 0));
            _pieces[5, 0] = CreatePiece(_bishopPrefab, PlayerColor.WHITE, new Vector2Int(5, 0));
            _pieces[2, 7] = CreatePiece(_bishopPrefab, PlayerColor.BLACK, new Vector2Int(2, 7));
            _pieces[5, 7] = CreatePiece(_bishopPrefab, PlayerColor.BLACK, new Vector2Int(5, 7));

            // Queens
            _pieces[3, 0] = CreatePiece(_queenPrefab, PlayerColor.WHITE, new Vector2Int(3, 0));
            _pieces[3, 7] = CreatePiece(_queenPrefab, PlayerColor.BLACK, new Vector2Int(3, 7));

            // Kings
            _pieces[4, 0] = CreatePiece(_kingPrefab, PlayerColor.WHITE, new Vector2Int(4, 0));
            _pieces[4, 7] = CreatePiece(_kingPrefab, PlayerColor.BLACK, new Vector2Int(4, 7));

            _kingWhitePosition = new Vector2Int(3, 0);
            _kingBlackPosition = new Vector2Int(3, 7);
        }

        private void InitializeCells()
        {
            for (int i = 0; i < BOARD_DIMENSION; i++)
            {
                for (int j = 0; j < BOARD_DIMENSION; j++)
                {
                    Cell cell = Instantiate(_cellPrefab, _cellsStartPosition + new Vector3(i, 0, j) * _cellsDistance, Quaternion.identity, _transform);
                    cell.SetBoardPosition(new Vector2Int(i, j));
                    _cells[i, j] = cell;
                }
            }
        }

        #endregion

        private void OnCellSelected(Vector2Int cellPosition)
        {
            Debug.Log("Cell selected!");
            ResetSelections();
        }

        private void OnPieceSelected(Vector2Int piecePosition)
        {
            if (piecePosition != _selectedPiecePosition)
            {
                if (_isPieceSelected)
                    _pieces[_selectedPiecePosition.x, _selectedPiecePosition.y].Deselect();
                _isPieceSelected = true;
                _selectedPiecePosition = piecePosition;
            }
        }

        private void ResetSelections()
        {
            _isPieceSelected = false;
            foreach (Vector2Int position in _highlightedCellsPositions)
                _cells[position.x, position.y].ResetHighlight();
        }
    }
}