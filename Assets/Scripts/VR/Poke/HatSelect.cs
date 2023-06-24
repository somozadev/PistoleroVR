using System;
using System.Collections;
using System.Globalization;
using General;
using General.Services;
using TMPro;
using Unity.Mathematics;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR.Poke
{
    public class HatSelect : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentBalance;


        [SerializeField] private XRBaseInteractable[] _hats;
        [SerializeField] private TMP_Text[] _hatsUI;
        [SerializeField] private int[] _hatsPrices;

        [SerializeField] private Transform _light;
        [SerializeField] public GameObject selectedHat;
        public XRBaseInteractable[] Hats => _hats;
        public TMP_Text[] HatsUI => _hatsUI;

        [SerializeField] private AnimationCurve _animationCurveHover = new(
            new Keyframe(0, 0),
            new Keyframe(.5f, .4f),
            new Keyframe(1, 1)
        );

        private void Awake()
        {
            OnEnable();
        }

        private void OnEnable()
        {
            UpdateShop();
            UpdateCurrentBalance();
            EventManager.EconomyUpdated += UpdateCurrentBalance;
            EventManager.PlayerDataLoaded += UpdateShop;
        }

        private void UpdateShop()
        {
            try
            {
                bool[] uhats = GameManager.Instance.players[0].PlayerData._unlockedHats;
                for (int i = 0; i < _hats.Length; i++)
                {
                    if (uhats[i + 1])
                    {
                        if (_hats[i].isActiveAndEnabled)
                        {
                            _hats[i].gameObject.SetActive(false);
                            _hatsUI[i].gameObject.SetActive(false);
                            _hatsPrices[i] = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void HatBought()
        {
            for (int i = 0; i < _hats.Length; i++)
            {
                if (selectedHat == _hats[i].transform.gameObject)
                {
                    _hatsUI[i].gameObject.SetActive(false);
                    _hats[i].gameObject.SetActive(false);
                }
            }
        }

        private void OnDisable()
        {
            EventManager.EconomyUpdated -= UpdateCurrentBalance;
            EventManager.PlayerDataLoaded += UpdateShop;
        }

        private void UpdateCurrentBalance()
        {
            // Debug.LogError(
            //     $"your balance <color=orange> {EconomyManager.Instance.value.ToString(CultureInfo.InvariantCulture)} $");
            currentBalance.text =
                $"your balance <color=orange> {EconomyManager.Instance.value.ToString(CultureInfo.InvariantCulture)} $";
        }

        private void Start()
        {
            for (int i = 0; i < _hatsPrices.Length; i++)
                _hatsUI[i].text = _hatsPrices[i] + " $";

            foreach (var hat in _hats)
            {
                if (hat.isActiveAndEnabled)
                {
                    hat.hoverEntered.AddListener(HoverEnterHat);
                    hat.hoverExited.AddListener(HoverExitHat);
                    hat.selectEntered.AddListener(SelectHat);
                }
            }
        }

        public int GetSelectedHatPrice()
        {
            int returner = 0;
            for (int i = 0; i < _hats.Length; i++)
            {
                if (selectedHat == _hats[i].transform.gameObject)
                    returner = _hatsPrices[i];
            }

            return returner;
        }

        public int GetSelectedHatId()
        {
            int id = 0;
            for (int i = 0; i < _hats.Length; i++)
            {
                if (selectedHat == _hats[i].transform.gameObject)
                    id = i+1;
            }

            return id;
        }

        private int GetSelectedHatPrice(int id)
        {
            int returner = 0;
            for (int i = 0; i < _hats.Length; i++)
            {
                if (i == id)
                    returner = _hatsPrices[i];
            }

            return returner;
        }

        public void HoverEnterHat(BaseInteractionEventArgs args)
        {
            if (args.interactorObject is XRRayInteractor)
            {
                var hat = args.interactableObject;
                StartCoroutine(HatHoverEnterAnim(hat));
            }
        }

        public void HoverExitHat(BaseInteractionEventArgs args)
        {
            if (args.interactorObject is XRRayInteractor)
            {
                var hat = args.interactableObject;
                ResetHatText(hat);
                StartCoroutine(HatHoverExitAnim(hat));
            }
        }

        public void SelectHat(BaseInteractionEventArgs args)
        {
            if (args.interactorObject is XRRayInteractor)
            {
                var hat = args.interactableObject;
                StartCoroutine(HatSelectedAnim(hat));
                UpdateHatText(hat);
                selectedHat = hat.transform.gameObject;
            }
        }

        private void ResetHatText(IXRInteractable hat)
        {
            for (int i = 0; i < _hats.Length; i++)
            {
                if (_hats[i].gameObject != hat.transform.gameObject)
                {
                    TMP_Text currentText = _hatsUI[i];
                    currentText.text = GetSelectedHatPrice(i) + " $";
                }
            }
        }

        private void UpdateHatText(IXRInteractable hat)
        {
            for (int i = 0; i < _hats.Length; i++)
            {
                if (_hats[i].gameObject == hat.transform.gameObject)
                {
                    TMP_Text currentText = _hatsUI[i];
                    if (EconomyManager.Instance.value >= GetSelectedHatPrice(i))
                    {
                        currentText.text = $"<color=green>{currentText.text}";
                    }
                    else
                    {
                        currentText.text = $"<color=red>{currentText.text}";
                    }
                }
            }
        }

        private IEnumerator HatHoverEnterAnim(IXRInteractable hat)
        {
            var elapsedTime = 0f;
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                var curvePercent = _animationCurveHover.Evaluate(elapsedTime / 1f);
                hat.transform.localScale =
                    Vector3.Slerp(hat.transform.localScale, new Vector3(120, 120, 120), curvePercent);
                yield return null;
            }
        }

        private IEnumerator HatHoverExitAnim(IXRInteractable hat)
        {
            var elapsedTime = 0f;
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                var curvePercent = _animationCurveHover.Evaluate(elapsedTime / 1f);
                hat.transform.localScale =
                    Vector3.Slerp(hat.transform.localScale, new Vector3(100, 100, 100), curvePercent);
                yield return null;
            }
        }

        private IEnumerator HatSelectedAnim(IXRInteractable hat)
        {
            _light.gameObject.SetActive(true);
            hat.transform.localScale = new Vector3(120, 120, 120);
            var elapsedTime = 0f;
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                var newDirection = Vector3.RotateTowards(_light.forward, (hat.transform.position - _light.position),
                    elapsedTime / 1f, 0f);
                _light.rotation = Quaternion.LookRotation(newDirection);
                yield return null;
            }
        }
    }
}