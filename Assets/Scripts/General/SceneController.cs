using System;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private UiController _uiController;
        [SerializeField] private string _currentScene;

        private void OnValidate()
        {
            if (_uiController == null)
                _uiController = FindObjectOfType<UiController>();
        }

        private async void Start()
        {
            // if (SceneManager.GetActiveScene().name != SceneNames.Essentials)
            //     await LoadScene(SceneNames.Essentials, LoadSceneMode.Single);
            // await LoadScene(SceneNames.StartScene, LoadSceneMode.Additive);

            await Task.Delay(100);
                // LoadScene("LoadingScene", LoadSceneMode.Additive);
                await _uiController.LoadingScene(GameManager.Instance.gameServices.SignInAnon());
                
        }

        // public void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        // {
        //     _currentScene = sceneName;
        //     
        // }
        public void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            // if (_currentScene != "")
            //     SceneManager.UnloadSceneAsync(_currentScene);
            _currentScene = sceneName;
            _uiController.LoadingSceneStart();
            SceneManager.LoadScene(sceneName, loadSceneMode);
            _uiController.LoadingScene();
        }

        public async Task LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            // if (_currentScene != "")
            //     SceneManager.UnloadSceneAsync(_currentScene);
            _currentScene = sceneName;
            _uiController.LoadingSceneStart();
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            asyncLoad.allowSceneActivation = false;
            await _uiController.LoadingScene(asyncLoad, GameManager.Instance.gameServices.SignInAnon());
            asyncLoad.allowSceneActivation = true;
        }
    }
}