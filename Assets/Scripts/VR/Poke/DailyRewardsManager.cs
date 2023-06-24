using System;
using System.Collections;
using System.Threading.Tasks;
using General;
using General.Services;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR.Poke
{
    public class DailyRewardsManager : MonoBehaviour
    {
        [SerializeField] private GameObject _rewardObject;
        [SerializeField] private TMP_Text _pannelText;

        [SerializeField] private TimeManager _timer;
        [SerializeField] private XRBaseInteractable _interactable;
        [SerializeField] private GameObject _toolTip;
        [SerializeField] private GameObject _coinAmountParticle;
        private Vector3 coinLocalPosInit;


        [SerializeField] private AnimationCurve _animationCurve = new(
            new Keyframe(0, 0),
            new Keyframe(.5f, .4f),
            new Keyframe(1, 1)
        );

        public bool _rewarded;

        private void OnEnable()
        {
            _timer = GameManager.Instance.GetComponent<TimeManager>();
            coinLocalPosInit = _coinAmountParticle.transform.localPosition;
            _interactable.selectEntered.AddListener(Select);
            EventManager.TimerStarted += RewardClaimed;
            EventManager.TimerEnded += RewardClaimed;
        }

        private void OnDisable()
        {
            EventManager.TimerStarted -= RewardClaimed;
            EventManager.TimerEnded -= RewardClaimed;
        }

        private void RewardClaimed()
        {
            _rewarded = _timer.rewardClaimed;
            if (_rewarded)
            {
                _interactable.enabled = false;
                _pannelText.text = ("<color=orange>" + _timer.remainingTime + "</color>");
            }
            else
            {
                _interactable.enabled = true;
                _pannelText.text = "daily <color=orange>rewards!</color>";
            }
        }

        private void Update()
        {
            if (_rewarded)
            {
                _pannelText.text = ("<color=orange>" + _timer.remainingTime + "</color>");
            }
        }

        private async void Select(BaseInteractionEventArgs args)
        {
            if (args.interactorObject is XRRayInteractor)
            {
                if (_rewarded) return;
                _rewarded = true;
                await GameManager.Instance.gameServices.GrantRandomCurrency();
                StartCoroutine(SelectedCor());
            }
        }


        private IEnumerator SelectedCor()
        {
            _toolTip.gameObject.SetActive(false);
            var elapsedTime = 0f;
            _coinAmountParticle.SetActive(true);
            _coinAmountParticle.GetComponentInChildren<TMP_Text>().text =
                $"<color=yellow>{EconomyManager.Instance.lastAddedValue} $</color>";
            while (elapsedTime < 2)
            {
                elapsedTime += Time.deltaTime;
                var curvePercent = _animationCurve.Evaluate(elapsedTime / 1f);
                _coinAmountParticle.transform.localScale = Vector3.Slerp(_coinAmountParticle.transform.localScale,
                    new Vector3(0.00826f, 0.00826f, 0.00826f), curvePercent);
                _coinAmountParticle.transform.localPosition = Vector3.Slerp(_coinAmountParticle.transform.localPosition,
                    new Vector3(0, 1.5f, 0), curvePercent);
                yield return null;
            }

            _coinAmountParticle.SetActive(false);
            _coinAmountParticle.transform.localScale = Vector3.zero;
            _coinAmountParticle.transform.localPosition = coinLocalPosInit;
            _toolTip.gameObject.SetActive(true);
            
            // _rewardObject.SetActive(false);
            //start timer again 

            EventManager.OnRewardClaimed();
            _rewarded = true;
            RewardClaimed();
        }
    }
}