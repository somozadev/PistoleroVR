using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace General
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingLayout;
        [SerializeField] private Image _loadingSlider;
        [SerializeField] private float _target;
        [SerializeField] private int _awaitingLoadingTime = 300;


        public async Task LoadingScene(AsyncOperation asyncLoad)
        {
            do
            {
                await Task.Delay(_awaitingLoadingTime);
                _target = asyncLoad.progress;
            } while (asyncLoad.progress < 0.9f);

            await Task.Delay(1000);
        }

        public async Task LoadingScene(Task signInAnon)
        {
            LoadingSceneStart();
            do
            {
                await Task.Delay(_awaitingLoadingTime);
                _target += 0.1f;
            } while (_target < 1f && !signInAnon.IsCompletedSuccessfully);

            LoadingSceneEnd();
        }

        public async void LoadingScene()
        {
            LoadingSceneStart();
            do
            {
                await Task.Delay(_awaitingLoadingTime);
                _target += 0.1f;
            } while (_target < 1f);

            LoadingSceneEnd();
        }

        public async Task LoadingScene(AsyncOperation asyncLoad, Task signInAnon)
        {
            LoadingSceneStart();
            do
            {
                await Task.Delay(_awaitingLoadingTime);
                _target = asyncLoad.progress;
            } while (asyncLoad.progress < 0.9f && !signInAnon.IsCompletedSuccessfully);

            await Task.Delay(1000);
            LoadingSceneEnd();
        }

        public void LoadingSceneStart()
        {
            EventManager.OnLoadingStarts();
            enabled = true;
            _target = 0f;
            _loadingSlider.fillAmount = 0f;
            _loadingLayout.SetActive(true);
        }

        public void LoadingSceneEnd()
        {
            _target = 1f;
            _loadingSlider.fillAmount = 1f;
            _loadingLayout.SetActive(false);
            enabled = false;
            EventManager.OnLoadingEnds();
        }

        private void Update()
        {
            _loadingSlider.fillAmount = Mathf.MoveTowards(_loadingSlider.fillAmount, _target, 3 * Time.deltaTime);
        }
    }
}