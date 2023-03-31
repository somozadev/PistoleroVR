using System;
using UnityEngine;
using UnityEngine.UI;

namespace General.UI_StartScene
{
    public class UIController_StartScene_Multiplayer : MonoBehaviour
    {
        [SerializeField] private Button _FindGame;
        [SerializeField] private Button _CreateGame;
        [SerializeField] private Button _JoinGame;

        [SerializeField] private Button[] _backButtons;

        [SerializeField] private GameObject _MainInnerCanvas;
        [SerializeField] private GameObject _JoinGameInnerCanvas;
        [SerializeField] private GameObject _FindGameInnerCanvas;
        [SerializeField] private GameObject _CreateGameInnerCanvas;

        private void Awake()
        {
            SetupButtonEvents();
        }

        #region ButtonTransitionsMethods

        private void SetupButtonEvents()
        {
            _JoinGame.onClick.AddListener(JoinGameUI);
            _FindGame.onClick.AddListener(FindGameUI);
            _CreateGame.onClick.AddListener(CreateGameUI);
            foreach (Button backButton in _backButtons)
                backButton.onClick.AddListener(BackToMainCanvas);
        }

        private void JoinGameUI()
        {
            _MainInnerCanvas.SetActive(true);
            _JoinGameInnerCanvas.SetActive(true);
        }

        private void CreateGameUI()
        {
            _MainInnerCanvas.SetActive(false);
            _CreateGameInnerCanvas.SetActive(true);
        }

        private void FindGameUI()
        {
            _MainInnerCanvas.SetActive(false);
            _FindGameInnerCanvas.SetActive(true);
        }

        private void BackToMainCanvas()
        {
            _MainInnerCanvas.SetActive(true);
            _JoinGameInnerCanvas.SetActive(false);
            _FindGameInnerCanvas.SetActive(false);
            _CreateGameInnerCanvas.SetActive(false);
        }

        #endregion
    }
}