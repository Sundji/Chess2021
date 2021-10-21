using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Practice.Chess
{
    public class UIManager : MonoBehaviour
    {
        [Header("Display Panel Information")]
        [SerializeField] private GameObject _displayPanel;
        [SerializeField] private Text _displayPanelText;
        [SerializeField] private List<Button> _displayPanelButtons = new List<Button>();

        [Header("Game Panel Information")]
        [SerializeField] private GameObject _gamePanel;
        [SerializeField] private List<Button> _gamePanelButtons = new List<Button>();

        private void Awake()
        {
            _displayPanel.SetActive(false);
            _gamePanel.SetActive(true);
            EventManager.EM.EventStatusChanged.AddListener(OnStatusChanged);

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
        }

        private void OnDestroy()
        {
            if (EventManager.EM != null)
                EventManager.EM.EventStatusChanged.RemoveListener(OnStatusChanged);
        }

        private void OnStatusChanged(Status status)
        {
            if (status == Status.CHECK || status == Status.IN_PROGRESS)
                return;

            string player = GameManager.GM.ActivePlayerColor == PlayerColor.BLACK ? "BLACK" : "WHITE";
            string text = "";

            if (status == Status.CHECK_MATE)
                text = "CHECK-MATE" + "\n" + player + " " + "LOST";
            else if (status == Status.FORFEIT)
                text = "FORFEIT" + "\n" + player + " " + "FORFEITED";
            else if (status == Status.STALEMATE)
                text = "STALEMATE";

            _displayPanelText.text = text;
            _displayPanel.SetActive(true);
            _gamePanel.SetActive(false);
        }
    }
}