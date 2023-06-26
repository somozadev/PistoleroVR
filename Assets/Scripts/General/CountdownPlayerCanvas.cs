using System.Collections;
using General.Sound;
using TMPro;
using UnityEngine;

namespace General
{
    public class CountdownPlayerCanvas : MonoBehaviour
    {
        [Header("Waves countdown")] [SerializeField]
        private GameObject _countdownCanvas;

        [SerializeField] private TMP_Text countdown;
        private int countdownNumber = 10;

        [SerializeField] AnimationCurve _animationCurve = new(
            new Keyframe(0, 0),
            new Keyframe(.5f, 1)
        );

        public void SetCountdownNumber(int value)
        {
            countdownNumber = value;
        }

        private float _initialFontSize;
        private readonly float _maxFontSize = 350f;

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
            countdownNumber = 10;
            EventManager.OnNewWave();
        }

        private IEnumerator Animate()
        {
            if (countdownNumber == -1)
                yield break;
            float curvePercent;
            var elapsedTime = 0f;
            countdown.text = countdownNumber.ToString();
            AudioManager.Instance.PlayOneShot("Bip");
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
    }
}