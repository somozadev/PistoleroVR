using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private UiController _uiController;
        [SerializeField] private string _currentScene;

        private void OnEnable()
        {
            if (_uiController == null)
                _uiController = FindObjectOfType<UiController>();
        }

        private async void Start()
        {
            await Task.Delay(100);
            await _uiController.LoadingScene(GameManager.Instance.gameServices.SignInAnon());
        }

        public void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            _currentScene = sceneName;
            _uiController.LoadingSceneStart();
            SceneManager.LoadScene(sceneName, loadSceneMode);
            _uiController.LoadingScene();
        }
    }
}