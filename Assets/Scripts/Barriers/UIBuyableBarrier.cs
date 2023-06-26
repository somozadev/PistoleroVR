using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIBuyableBarrier : MonoBehaviour
{
    [SerializeField] private List<BuyableBarrier> _barriers;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;

    [SerializeField] private InputAction _buyAction;

    private bool _currentlyInBarrier;
    private BuyableBarrier _currentBarrier;
    private PlayerData _playerData;

    private void Awake()
    {
        if (_barriers.Count == 0)
            foreach (BuyableBarrier barrier in FindObjectsOfType<BuyableBarrier>())
                _barriers.Add(barrier);

        if (_text == null)
            _text = GetComponentInChildren<TMP_Text>();
        if (_image == null)
            _image = GetComponentInChildren<Image>();

        _buyAction.performed += ctx => Buy();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _barriers.Count; i++)
        {
            BuyableBarrier barrier = _barriers[i];
            barrier.AddListenerPlayerEnter(delegate { ShowUnlockable(barrier); });
            barrier.AddListenerPLayerLeave(HideUnlockable);
        }

        _buyAction.Enable();
    }

    private void OnDisable()
    {
        for (int i = 0; i < _barriers.Count; i++)
        {
            BuyableBarrier barrier = _barriers[i];
            barrier.RemoveListenerPlayerEnter(delegate { ShowUnlockable(barrier); });
            barrier.RemoveListenerPLayerLeave(HideUnlockable);
        }

        _buyAction.Disable();
    }

    private void Buy()
    {
        if (_currentlyInBarrier)
        {
            if (GameManager.Instance.players.First().PlayerData._economy >= _currentBarrier.barrierPrice)
            {
                GameManager.Instance.players.First().PlayerData.Buy(_currentBarrier.barrierPrice);
                _currentBarrier.Buy();
                gameObject.SetActive(false);
            }
        }
    }

    private void ShowUnlockable(BuyableBarrier barrier)
    {
        _image.gameObject.SetActive(true);
        _currentBarrier = barrier;
        _currentlyInBarrier = true;
        _text.text = "Unlock path for <color=orange>" + barrier.barrierPrice + "$";
    }

    private void HideUnlockable()
    {
        _image.gameObject.SetActive(false);
        _currentBarrier = null;
        _currentlyInBarrier = false;
        _text.text = "";
    }
}