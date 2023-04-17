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

        private async void Start()
        {
            if (SceneManager.GetActiveScene().name != SceneNames.Essentials)
                await LoadScene(SceneNames.Essentials, LoadSceneMode.Single);
            await LoadScene(SceneNames.StartScene, LoadSceneMode.Additive);

            GameManager.Instance.onGameManagerLoaded.AddListener(() =>
            {
                LoadScene("LoadingScene", LoadSceneMode.Additive);
            });
        }

        // public void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        // {
        //     _currentScene = sceneName;
        //     
        // }

        public async Task LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            if (_currentScene != "")
                SceneManager.UnloadSceneAsync(_currentScene);
            _currentScene = sceneName;
            _uiController.LoadingSceneStart();
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            asyncLoad.allowSceneActivation = false;
            await _uiController.LoadingScene(asyncLoad, GameManager.Instance.gameServices.SignInAnon());
            asyncLoad.allowSceneActivation = true;
            _uiController.LoadingSceneEnd();
        }
    }
}