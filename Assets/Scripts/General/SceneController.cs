using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private UiController _uiController;
        [SerializeField] private string _currentScene;

        public bool Host;
        [SerializeField] private NetworkManager _networkManager;

        private void Start()
        {
            if (Host)
                _networkManager.StartHost();
            else
                _networkManager.StartClient();

            if (SceneManager.GetActiveScene().name != SceneNames.Essentials)
                LoadScene(SceneNames.Essentials, LoadSceneMode.Single);
            LoadScene(SceneNames.StartScene, LoadSceneMode.Additive);
        }

        public async void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
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