using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Practice.Chess
{
    public class UIManager : MonoBehaviour
    {
        [Header("Display Panel Information")]
        [SerializeField] private GameObject _displayPanel = null;
        [SerializeField] private Text _displayPanelText = null;
        [SerializeField] private List<Button> _displayPanelButtons = new List<Button>();

        [Header("Game Panel Information")]
        [SerializeField] private GameObject _gamePanel = null;
        [SerializeField] private List<Button> _gamePanelButtons = new List<Button>();

        [Header("Promotion Panel Information")]
        [SerializeField] private GameObject _promotionPanel = null;
        [SerializeField] private List<Button> _promotionPanelButtons = new List<Button>();

        private void Awake()
        {
            _displayPanel.SetActive(false);
            _gamePanel.SetActive(true);
            _promotionPanel.SetActive(false);

            SetUpAppearance();

            EventManager.EM.EventStatusChanged.AddListener(OnStatusChanged);
            EventManager.EM.EventWaitingForPromotion.AddListener(OnWaitingForPromotion);
        }

        private void OnDestroy()
        {
            if (EventManager.EM != null)
            {
                EventManager.EM.EventStatusChanged.RemoveListener(OnStatusChanged);
                EventManager.EM.EventWaitingForPromotion.RemoveListener(OnWaitingForPromotion);
            }
        }

        private void OnStatusChanged(Status status)
        {
            if (status == Status.CHECK || status == Status.IN_PROGRESS)
                return;

            string player = GameManager.GM.ActivePlayerColor == PlayerColor.BLACK ? "BLACK" : "WHITE";
            string text;

            if (status == Status.CHECK_MATE)
                text = "CHECK-MATE" + "\n" + player + " " + "LOST";
            else if (status == Status.FORFEIT)
                text = "FORFEIT" + "\n" + player + " " + "FORFEITED";
            else
                text = "STALEMATE";

            _displayPanelText.text = text;
            _displayPanel.SetActive(true);
            _gamePanel.SetActive(false);
            _promotionPanel.SetActive(false);
        }

        private void OnWaitingForPromotion(Pawn pawn)
        {
            _displayPanel.SetActive(false);
            _gamePanel.SetActive(false);
            _promotionPanel.SetActive(true);
            return;
        }

        private void SetUpAppearance()
        {
            DesignData designData = DesignManager.DM.DesignData;
            _displayPanel.GetComponent<Image>().color = designData.ColorPanelBackground;
            _displayPanelText.color = designData.ColorTextMain;
            foreach (Button button in _displayPanelButtons)
            {
                button.GetComponent<Image>().color = designData.ColorButtonBackground;
                button.GetComponentInChildren<Text>().color = designData.ColorText;
            }

            foreach (Button button in _gamePanelButtons)
                button.GetComponent<Image>().color = designData.ColorButtonBackground;

            _promotionPanel.GetComponent<Image>().color = designData.ColorPanelBackground;
            foreach (Button button in _promotionPanelButtons)
            {
                button.GetComponent<Image>().color = designData.ColorButtonBackground;
                Text text = button.GetComponentInChildren<Text>();
                text.color = designData.ColorText;
                PieceType pieceType = (PieceType)System.Enum.Parse(typeof(PieceType), text.text);
                button.onClick.AddListener(() => ChoosePromotion(pieceType));
            }
        }

        public void ChoosePromotion(PieceType pieceType)
        {
            _displayPanel.SetActive(false);
            _gamePanel.SetActive(true);
            _promotionPanel.SetActive(false);
            EventManager.EM.EventPromotionPieceChosen.Invoke(pieceType);
        }
    }
}