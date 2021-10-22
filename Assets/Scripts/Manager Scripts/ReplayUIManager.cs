using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Practice.Chess
{
    public class ReplayUIManager : MonoBehaviour
    {
        [SerializeField] private Button _exitButton = null;
        [SerializeField] private Button _returnToGameButton = null;

        [Header("Manual Replay Panel")]
        [SerializeField] private Image _manualReplayPanelBackground = null;
        [SerializeField] private List<Button> _manualReplayButtons = new List<Button>();

        [Header("Automated Replay Panel")]
        [SerializeField] private Image _automatedReplayPanelBackground = null;
        [SerializeField] private Button _playButton = null;
        [SerializeField] private Button _stopButton = null;
        [SerializeField] private InputField _replaySpeedInputField = null;
        [SerializeField] private Vector2 _replaySpeedRange = Vector2.one;

        private float _replaySpeed;

        private void Awake()
        {
            _replaySpeedInputField.onValueChanged.AddListener(UpdateValue);
            _replaySpeedInputField.text = _replaySpeedRange.x.ToString();
            _replaySpeed = _replaySpeedRange.x;

            _playButton.onClick.AddListener(Play);
            _stopButton.onClick.AddListener(Stop);

            SetUpAppearance();
        }

        private void Play()
        {
            foreach (Button button in _manualReplayButtons)
                button.enabled = false;
            _playButton.enabled = false;
            _stopButton.enabled = true;
            ReplayManager.RM.StartAutoplay(_replaySpeed);
        }

        private void Stop()
        {
            foreach (Button button in _manualReplayButtons)
                button.enabled = true;
            _playButton.enabled = true;
            _stopButton.enabled = false;
            ReplayManager.RM.StopAutoplay();
        }

        private void UpdateValue(string argument)
        {
            float value = float.Parse(_replaySpeedInputField.text);
            value = Mathf.Clamp(value, _replaySpeedRange.x, _replaySpeedRange.y);
            _replaySpeedInputField.text = value.ToString();
            _replaySpeed = value;
        }

        private void SetUpAppearance()
        {
            DesignData designData = DesignManager.DM.DesignData;
            _automatedReplayPanelBackground.color = designData.ColorPanelBackground;
            _manualReplayPanelBackground.color = designData.ColorPanelBackground;

            _exitButton.GetComponent<Image>().color = designData.ColorButtonBackground;
            _returnToGameButton.GetComponent<Image>().color = designData.ColorButtonBackground;

            foreach (Button button in _manualReplayButtons)
                button.GetComponent<Image>().color = designData.ColorButtonBackground;

            _playButton.GetComponent<Image>().color = designData.ColorButtonBackground;
            _playButton.GetComponentInChildren<Text>().color = designData.ColorText;
            _stopButton.GetComponent<Image>().color = designData.ColorButtonBackground;
            _stopButton.GetComponentInChildren<Text>().color = designData.ColorText;
        }

        public void ReturnToGame()
        {
            SceneManager.LoadScene(0);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}