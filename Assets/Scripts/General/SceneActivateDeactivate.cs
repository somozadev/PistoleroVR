using Enemies.BT;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
{
    public class SceneActivateDeactivate : MonoBehaviour
    {
        [SerializeField] private GameObject scene;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private WavesManager _wavesManager;

        private void OnEnable()
        {
            scene.SetActive(false);
            EventManager.LoadingStarts += HideScene;
            EventManager.LoadingEnds += ShowScene;
        }

        private void OnDisable()
        {
            EventManager.LoadingStarts -= HideScene;
            EventManager.LoadingEnds -= ShowScene;
        }

        private void ShowScene()
        {
            scene.SetActive(true);
            if (SceneManager.GetActiveScene().name == "E_SinglePlayerScene")
            {
                GameManager.Instance.players[0].PlayerIngameCanvas.EnableCanvas();
                GameManager.Instance.players[0].PlayerHealth.ResetHp();
                GameManager.Instance.players[0].PlayerData._economy = 0;

            }
            else
                GameManager.Instance.players[0].PlayerIngameCanvas.DisableCanvas();
        }

        private void HideScene() => scene.SetActive(false);
    }
}