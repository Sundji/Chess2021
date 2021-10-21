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
        private List<Vector2Int> _knightsBlackPositions = new List<Vector2Int>();
        private List<Vector2Int> _knightsWhitePositions = new List<Vector2Int>();

        private Dictionary<Vector2Int, List<Vector2Int>> _allLegalPositions = new Dictionary<Vector2Int, List<Vector2Int>>();

        private bool _isPieceSelected = false;
        private Vector2Int _selectedPiecePosition;

        public Piece[,] Pieces { get { return _pieces; } }
        public Vector2Int KingBlackPosition { get { return _kingBlackPosition; } }
        public Vector2Int KingWhitePosition { get { return _kingWhitePosition; } }
        public List<Vector2Int> KnightsBlackPositions { get { return _knightsBlackPositions; } }
        public List<Vector2Int> KnightsWhitePositions { get { return _knightsWhitePositions; } }

        private void Awake()
        {
            _transform = transform;
            InitializeCells();
            InitializePieces();

            EventManager.EM.EventCellSelected.AddListener(OnCellSelected);
            EventManager.EM.EventPieceSelected.AddListener(OnPieceSelected);
            EventManager.EM.EventPlayerTurnStarted.AddListener(OnPlayerTurnStarted);
        }

        private void OnDestroy()
        {
            if (EventManager.EM != null)
            {
                EventManager.EM.EventCellSelected.RemoveListener(OnCellSelected);
                EventManager.EM.EventPieceSelected.RemoveListener(OnPieceSelected);
                EventManager.EM.EventPlayerTurnStarted.RemoveListener(OnPlayerTurnStarted);
            }
        }

        private Piece CreatePiece(Piece prefab, PlayerColor color, Vector2Int boardPosition)
        {
            Vector3 position = _cellsStartPosition + new Vector3(boardPosition.x, 0, boardPosition.y) * _cellsDistance;
            Quaternion rotation = color == PlayerColor.WHITE ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

            Piece piece = Instantiate(prefab, position, rotation, _transform);
            piece.SetUp(color, boardPosition);

            return piece;
        }

        private void OnCellSelected(Vector2Int cellPosition)
        {
            Vector2Int boardPosition = cellPosition;
            Vector3 worldPosition = _cells[cellPosition.x, cellPosition.y].WorldPosition;

            ResetSelections();
            _pieces[_selectedPiecePosition.x, _selectedPiecePosition.y].MovePiece(this, boardPosition, worldPosition);
            EventManager.EM.EventPlayerTurnEnded.Invoke(GameManager.GM.ActivePlayerColor);
        }

        private void OnPieceSelected(Vector2Int piecePosition)
        {
            ResetSelections();
            _isPieceSelected = true;
            _selectedPiecePosition = piecePosition;

            _highlightedCellsPositions.AddRange(_pieces[piecePosition.x, piecePosition.y].GetLegalPositions(this));
            foreach (Vector2Int cellPosition in _highlightedCellsPositions)
                _cells[cellPosition.x, cellPosition.y].SetHighlight();
        }

        private void OnPlayerTurnStarted(PlayerColor color)
        {
            CheckGameStatus(color);
            ResetSelections();
        }

        private void CheckGameStatus(PlayerColor activePlayerColor)
        {
            _allLegalPositions.Clear();
            Status status = Status.IN_PROGRESS;

            Vector2Int kingPosition = activePlayerColor == PlayerColor.BLACK ? KingBlackPosition : KingWhitePosition;
            King king = (King)(_pieces[kingPosition.x, kingPosition.y]);
            bool isKingSafe = king.CheckIfSafe(this);

            bool arePositionsFound = false;
            for (int i = 0; i < BOARD_DIMENSION; i++)
            {
                for (int j = 0; j < BOARD_DIMENSION; j++)
                {
                    Piece piece = _pieces[i, j];
                    if (piece != null && piece.Color == activePlayerColor)
                    {
                        List<Vector2Int> legalPositions = piece.GetLegalPositions(this);
                        arePositionsFound = arePositionsFound ? arePositionsFound : (legalPositions.Count > 0);
                        _allLegalPositions.Add(new Vector2Int(i, j), legalPositions);
                    }
                }
            }

            if (isKingSafe && !arePositionsFound)
                status = Status.STALEMATE;
            if (!isKingSafe && arePositionsFound)
                status = Status.CHECK;
            if (!isKingSafe && !arePositionsFound)
                status = Status.CHECK_MATE;

            Debug.Log("status is " + status);
            EventManager.EM.EventStatusChanged.Invoke(status);
        }

        private void ResetSelections()
        {
            if (_isPieceSelected)
            {
                _isPieceSelected = false;
                _pieces[_selectedPiecePosition.x, _selectedPiecePosition.y].Deselect();
            }

            foreach (Vector2Int position in _highlightedCellsPositions)
                _cells[position.x, position.y].ResetHighlight();
            _highlightedCellsPositions.Clear();
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
            _knightsWhitePositions.Add(new Vector2Int(1, 0));
            _knightsWhitePositions.Add(new Vector2Int(6, 0));
            _knightsBlackPositions.Add(new Vector2Int(1, 7));
            _knightsBlackPositions.Add(new Vector2Int(6, 7));

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

            _kingWhitePosition = new Vector2Int(4, 0);
            _kingBlackPosition = new Vector2Int(4, 7);
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
    }
}