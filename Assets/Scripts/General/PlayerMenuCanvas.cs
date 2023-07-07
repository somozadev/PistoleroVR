using System;
using General.Sound;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace General
{
    public class PlayerMenuCanvas : MonoBehaviour
    {
        [SerializeField] private InputAction _menuAction;

        [Header("Hands controllers InputActions references")] [SerializeField]
        private InputActionProperty _leftHandMenuAction;

        [SerializeField] private bool _menuOpened;
        [SerializeField] private Canvas _canvas;

        private Toggle _rotationToggle;


        public TMP_Text playerIdText;

        [SerializeField] private TMP_Text _playerHeight;
        [SerializeField] private Slider _playerHeightSlider;
        [SerializeField] private Slider _musicSlider;


        [Header("Tabs")] [Space(20)] [SerializeField]
        private GameObject _faqGroup;

        [SerializeField] private GameObject _menuGroup;
        [SerializeField] private GameObject _debugroup;


        public void OpenFaqGroup()
        {
            _faqGroup.SetActive(true);
            _menuGroup.SetActive(false);
            _debugroup.SetActive(false);
        }

        public void OpenDebugGroup()
        {
            _debugroup.SetActive(true);
            _faqGroup.SetActive(false);
            _menuGroup.SetActive(false);
        }

        public void OpenMenuGroup()
        {
            _menuGroup.SetActive(true);
            _faqGroup.SetActive(false);
            _debugroup.SetActive(false);
        }

        private void HeightSlider()
        {
            _playerHeight.text = "HEIGHT: " + _playerHeightSlider.value.ToString("0:0.00");
        }

        private void Awake()
        {
            if (_canvas == null)
            {
                _canvas = GetComponentInChildren<Canvas>();
            }

            _rotationToggle = _canvas.GetComponentInChildren<Toggle>();


            SetupEvents();
        }

        private void Start()
        {
            _playerHeightSlider.value = GetComponentInChildren<XROrigin>().CameraYOffset;
            _playerHeightSlider.onValueChanged.AddListener(delegate { UpdatePlayerHeight(); });
            _musicSlider.onValueChanged.AddListener(delegate { UpdateMusicValue(); });
            _playerHeightSlider.onValueChanged.AddListener(delegate { HeightSlider(); });
            _musicSlider.value = AudioManager.Instance.GetMusicVol();
        }


        private void UpdatePlayerHeight()
        {
            GetComponentInChildren<XROrigin>().CameraYOffset = _playerHeightSlider.value;
        }

        private void UpdateMusicValue()
        {
            AudioManager.Instance.SetMusicVol(_musicSlider.value);
        }

        public void MenuButtonPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
                OpenCloseMenu();
        }

        private void SetupEvents()
        {
            _rotationToggle.onValueChanged.AddListener(ChangeTurningMode);
            // _menuAction.performed += ctx => OpenCloseMenu();
            _leftHandMenuAction.action.performed += ctx => OpenCloseMenu();
        }

        private void ChangeTurningMode(bool value)
        {
            GetComponentInParent<Player>().PlayerMovement
                .UpadteRotationType(!value ? MovementVR.RotationType.Snap : MovementVR.RotationType.Continuous);
        }


        public void OpenCloseMenu()
        {
            if (!_menuOpened)
            {
                _menuOpened = true;
                Time.timeScale = 0;
                _canvas.gameObject.SetActive(true);
            }
            else
            {
                _menuOpened = false;
                Time.timeScale = 1;
                _canvas.gameObject.SetActive(false);
            }
        }

        public void MenuButton()
        {
            if (GameManager.Instance.objectPoolingManager.GetPoolByName("entititesPooling")!=null)
            {
                GameManager.Instance.objectPoolingManager.DeleteObjectPooling("entititesPooling");
            }

            GameManager.Instance.players[0].PlayerData._economy = 0;
            GameManager.Instance.players[0].PlayerHealth.ResetHp();
            GameManager.Instance.sceneController.LoadScene("E_StartScene", LoadSceneMode.Single);
            OpenCloseMenu();
            AudioManager.Instance.PlayStartingTheme();
            GameManager.Instance.players[0].PlayerMovement.ResetTo(Vector3.zero);
        }

        private void OnEnable()
        {
            _menuAction.Enable();
        }

        private void OnDisable()
        {
            _menuAction.Disable();
        }
    }
}