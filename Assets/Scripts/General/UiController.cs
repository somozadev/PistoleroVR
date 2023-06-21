using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

namespace General
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingLayout;
        [SerializeField] private Image _loadingSlider;
        [SerializeField] private float _target;
        [SerializeField] private int _awaitingLoadingTime = 300;

        [Header("Waves countdown")] [SerializeField]
        private Canvas _countdownCanvas;

        [SerializeField] private TMP_Text countdown;
        private int countdownNumber = 3;

        [SerializeField] AnimationCurve _animationCurve = new(
            new Keyframe(0, 0),
            new Keyframe(.5f, 1)
        );

        private float _initialFontSize;
        private float _maxFontSize = 350f;

        private void Awake()
        {
            _initialFontSize = countdown.fontSize;
        }
        #region Countdown
        [ContextMenu("Countdown")]
        public void NewWave()
        {
            StartCoroutine(NewCountdown());
        }

        private IEnumerator NewCountdown()
        {
            _countdownCanvas.gameObject.SetActive(true);
            yield return StartCoroutine(Animate());
            _countdownCanvas.gameObject.SetActive(false);
        }

        private IEnumerator Animate()
        {
            if (countdownNumber == -1)
                yield break;
            float curvePercent;
            var elapsedTime = 0f;
            countdown.text = countdownNumber.ToString();
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                curvePercent = _animationCurve.Evaluate(elapsedTime / 1f);
                countdown.fontSize = Mathf.LerpUnclamped(countdown.fontSize, _maxFontSize, curvePercent);
                yield return null;
            }

            while (elapsedTime > 0)
            {
                elapsedTime -= Time.deltaTime;
                curvePercent = _animationCurve.Evaluate(elapsedTime / 1f);
                countdown.fontSize = Mathf.LerpUnclamped(countdown.fontSize, _initialFontSize, curvePercent);
                yield return null;
            }

            countdownNumber--;
            yield return StartCoroutine(Animate());
        }
        #endregion

        #region LoadingLayout

        public async Task LoadingScene(AsyncOperation asyncLoad)
        {
            do
            {
                await Task.Delay(_awaitingLoadingTime);
                _target = asyncLoad.progress;
            } while (asyncLoad.progress < 0.9f);

            await Task.Delay(1000);
        }

        public async Task LoadingScene(Task signInAnon) //initial "load" to follow progress bar to loginstatus with unity services 
        {
            LoadingSceneStart();
            do
            {
                await Task.Delay(_awaitingLoadingTime);
                _loadingSlider.fillAmount = Mathf.MoveTowards(_loadingSlider.fillAmount, _target, 3 * Time.deltaTime); //Mathf.Lerp(_loadingSlider.fillAmount, 1, _awaitingLoadingTime + Time.deltaTime); //
                _target += 0.1f;
            } while (_target < 1f && !signInAnon.IsCompletedSuccessfully);

            LoadingSceneEnd();
        }

        public async void LoadingScene() //normal scene loading
        {
            LoadingSceneStart();
            do
            {
                await Task.Delay(_awaitingLoadingTime);
                _loadingSlider.fillAmount = Mathf.MoveTowards(_loadingSlider.fillAmount, _target, 3 * Time.deltaTime);
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
            // enabled = false;
            EventManager.OnLoadingEnds();
        }

        // private void Update()
        // {
        //     _loadingSlider.fillAmount = Mathf.MoveTowards(_loadingSlider.fillAmount, _target, 3 * Time.deltaTime);
        // }

        #endregion
    }
}