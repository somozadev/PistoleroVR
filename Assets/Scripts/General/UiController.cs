using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace General
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingLayout;
        [SerializeField] private Image _loadingSlider;
        [SerializeField] private float _target;

        public async Task LoadingScene(AsyncOperation asyncLoad)
        {
            do
            {
                await Task.Delay(3000);
                _target = asyncLoad.progress;
            } while (asyncLoad.progress < 0.9f);

            await Task.Delay(1000);
        }

        public void LoadingSceneStart()
        {
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
        }

        private void Update()
        {
            Debug.Log("tetsetet ");
            _loadingSlider.fillAmount = Mathf.MoveTowards(_loadingSlider.fillAmount, _target, 3 * Time.deltaTime);
        }
    }
}