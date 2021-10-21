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

        private Dictionary<Vector2Int, List<Vector2Int>> _allLegalPositions = new Dictionary<Vector2Int, List<Vector2Int>>();
        private King _kingBlack;
        private King _kingWhite;

        private bool _isPieceSelected = false;
        private Vector2Int _selectedPiecePosition;

        public Piece[,] Pieces { get { return _pieces; } }
        public King KingBlack { get { return _kingBlack; } }
        public King KingWhite { get { return _kingWhite; } }
        public List<Knight> KnightsBlack { get; } = new List<Knight>();
        public List<Knight> KnightsWhite { get; } = new List<Knight>();

        private void Awake()
        {
            _transform = transform;
            InitializeCells();
            InitializePieces();

            EventManager.EM.EventCellSelected.AddListener(OnCellSelected);
            EventManager.EM.EventPlayerTurnStarted.AddListener(OnPlayerTurnStarted);
        }

        private void OnDestroy()
        {
            if (EventManager.EM != null)
            {
                EventManager.EM.EventCellSelected.RemoveListener(OnCellSelected);
                EventManager.EM.EventPlayerTurnStarted.RemoveListener(OnPlayerTurnStarted);
            }
        }

        #region EVENT LISTENING SCRIPTS

        private void OnCellSelected(Vector2Int position)
        {
            PlayerColor activePlayerColor = GameManager.GM.ActivePlayerColor;

            Piece piece = _pieces[position.x, position.y];
            bool isSelectablePiece = (piece != null && piece.Color == activePlayerColor);

            if (isSelectablePiece)
            {
                if (_isPieceSelected)
                {
                    if (_selectedPiecePosition == position)
                        return;
                    ResetSelections();
                }

                _isPieceSelected = true;
                _selectedPiecePosition = position;
                piece.SetHighlight();

                _highlightedCellsPositions.AddRange(_allLegalPositions[position]);
                foreach (Vector2Int cellPosition in _highlightedCellsPositions)
                    _cells[cellPosition.x, cellPosition.y].SetHighlight();
            }

            bool isSelectableCell = (piece == null || (piece != null && piece.Color != activePlayerColor));
            if (isSelectableCell && _isPieceSelected && _highlightedCellsPositions.Contains(position))
            {
                piece = _pieces[_selectedPiecePosition.x, _selectedPiecePosition.y];
                ResetSelections();
                piece.MovePiece(this, position, _cells[position.x, position.y].WorldPosition);
                EventManager.EM.EventPlayerTurnEnded.Invoke(activePlayerColor);
            }
        }

        private void OnPlayerTurnStarted(PlayerColor color)
        {
            CheckGameStatus(color);
        }

        #endregion

        #region BOARD INITIALIZATION SCRIPTS

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

            KnightsWhite.Add((Knight)_pieces[1, 0]);
            KnightsWhite.Add((Knight)_pieces[6, 0]);
            KnightsBlack.Add((Knight)_pieces[1, 7]);
            KnightsBlack.Add((Knight)_pieces[6, 7]);

            _pieces[2, 0] = CreatePiece(_bishopPrefab, PlayerColor.WHITE, new Vector2Int(2, 0));
            _pieces[5, 0] = CreatePiece(_bishopPrefab, PlayerColor.WHITE, new Vector2Int(5, 0));
            _pieces[2, 7] = CreatePiece(_bishopPrefab, PlayerColor.BLACK, new Vector2Int(2, 7));
            _pieces[5, 7] = CreatePiece(_bishopPrefab, PlayerColor.BLACK, new Vector2Int(5, 7));

            _pieces[3, 0] = CreatePiece(_queenPrefab, PlayerColor.WHITE, new Vector2Int(3, 0));
            _pieces[3, 7] = CreatePiece(_queenPrefab, PlayerColor.BLACK, new Vector2Int(3, 7));

            _pieces[4, 0] = CreatePiece(_kingPrefab, PlayerColor.WHITE, new Vector2Int(4, 0));
            _pieces[4, 7] = CreatePiece(_kingPrefab, PlayerColor.BLACK, new Vector2Int(4, 7));

            _kingWhite = (King)_pieces[4, 0];
            _kingBlack = (King)_pieces[4, 7];
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

        private void CheckGameStatus(PlayerColor activePlayerColor)
        {
            _allLegalPositions.Clear();
            Status status = Status.IN_PROGRESS;

            Vector2Int kingPosition = activePlayerColor == PlayerColor.BLACK ? KingBlack.BoardPosition : KingWhite.BoardPosition;
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
            if (!isKingSafe)
                status = arePositionsFound ? Status.CHECK : Status.CHECK_MATE;

            Debug.Log(GameManager.GM.ActivePlayerColor + ": status " + status);
            EventManager.EM.EventStatusChanged.Invoke(status);
        }

        private void ResetSelections()
        {
            _isPieceSelected = false;
            _pieces[_selectedPiecePosition.x, _selectedPiecePosition.y].ResetHighlight();

            foreach (Vector2Int position in _highlightedCellsPositions)
                _cells[position.x, position.y].ResetHighlight();
            _highlightedCellsPositions.Clear();
        }
    }
}