using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace General
{
    public class PlayerMenuCanvas : MonoBehaviour
    {
        [SerializeField] private InputAction _menuAction;

        [Header("Hands controllers InputActions references")] 
        [SerializeField]
        private InputActionProperty _leftHandMenuAction;

        [SerializeField] private bool _menuOpened;
        [SerializeField] private Canvas _canvas;

        private Toggle _rotationToggle;


        public TMP_Text playerIdText;


        private void Awake()
        {
            if (_canvas == null)
            {
                _canvas = GetComponentInChildren<Canvas>();
            }

            _rotationToggle = _canvas.GetComponentInChildren<Toggle>();


            SetupEvents();
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

        private void OpenCloseMenu()
        {
            if (!_menuOpened)
            {
                _menuOpened = true;
                _canvas.gameObject.SetActive(true);
            }
            else
            {
                _menuOpened = false;
                _canvas.gameObject.SetActive(false);
            }
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