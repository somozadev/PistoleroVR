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
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;

    [SerializeField] private InputAction _buyAction;

    [SerializeField] private bool _currentlyInBarrier;
    [SerializeField] private BuyableBarrier _currentBarrier;
    [SerializeField] private BuyableBarrier[] _groupBarriers;
    private PlayerData _playerData;

    private void Awake()
    {
        _currentBarrier = GetComponentInParent<BuyableBarrier>();
        if (_text == null)
            _text = GetComponentInChildren<TMP_Text>();
        if (_image == null)
            _image = GetComponentInChildren<Image>();
        _text.text = "";
        _buyAction.performed += ctx => Buy();
        _playerData = GameManager.Instance.players.First().PlayerData;
    }

    private void OnEnable()
    {
        _currentBarrier.AddListenerPlayerEnter(delegate { ShowUnlockable(_currentBarrier); });
        _currentBarrier.AddListenerPLayerLeave(HideUnlockable);
        _buyAction.Enable();
    }

    private void OnDisable()
    {
        _currentBarrier.RemoveListenerPlayerEnter(delegate { ShowUnlockable(_currentBarrier); });
        _currentBarrier.RemoveListenerPLayerLeave(HideUnlockable);
        _buyAction.Disable();
    }

    private void Buy()
    {
        if (_currentlyInBarrier)
        {
            if (_playerData._economy >= _currentBarrier.barrierPrice)
            {
                _playerData.Buy(_currentBarrier.barrierPrice);
                foreach (var barrier in _groupBarriers)
                {
                    barrier.Buy();
                    barrier.gameObject.SetActive(false);
                }
            }
        }
    }

    private void ShowUnlockable(BuyableBarrier barrier)
    {
        _image.gameObject.SetActive(true);
        _currentlyInBarrier = true;
        _text.text = "Unlock path for <color=orange>" + barrier.barrierPrice + "$";
    }

    private void HideUnlockable()
    {
        _image.gameObject.SetActive(false);
        _currentlyInBarrier = false;
        _text.text = "";
    }
}