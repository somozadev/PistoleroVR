using System;
using General.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace General.UI_StartScene
{
    public class UIController_StartScene_Singleplayer : MonoBehaviour
    {
        [SerializeField] private Button _joinSinglePlayer;


        private void OnEnable()
        {
            _joinSinglePlayer.onClick.AddListener(LoadSinglePLayerScene);
        }

        private void OnDisable()
        {
            _joinSinglePlayer.onClick.RemoveListener(LoadSinglePLayerScene);
        }

    [ContextMenu("singleplayer")]
        private void LoadSinglePLayerScene()
        {
            GameManager.Instance.sceneController.LoadScene("E_SinglePlayerScene", LoadSceneMode.Single);
            AudioManager.Instance.PlayThemes();
        }
    }
}